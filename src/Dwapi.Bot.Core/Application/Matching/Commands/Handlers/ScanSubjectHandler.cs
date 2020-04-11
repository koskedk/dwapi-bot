using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Dwapi.Bot.Core.Algorithm.JaroWinkler;
using Dwapi.Bot.Core.Application.Matching.Events;
using Dwapi.Bot.Core.Domain.Configs;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Utility;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Matching.Commands.Handlers
{
    public class ScanSubjectHandler : IRequestHandler<ScanSubject, Result>
    {
        private readonly IMediator _mediator;
        private readonly ISubjectIndexRepository _repository;
        private readonly IMatchConfigRepository _configRepository;
        private readonly IJaroWinklerScorer _scorer;

        public ScanSubjectHandler(IMediator mediator, ISubjectIndexRepository repository, IJaroWinklerScorer scorer, IMatchConfigRepository configRepository)
        {
            _mediator = mediator;
            _repository = repository;
            _scorer = scorer;
            _configRepository = configRepository;
        }

        public async Task<Result> Handle(ScanSubject request, CancellationToken cancellationToken)
        {
            Log.Debug($"scanning {request.Level}:{request.LevelCode} ...");

            var configs = _configRepository.GetConfigs().ToList();

            try
            {
                int page = 1;
                int totalRecords;

                if (request.Level == ScanLevel.Site)
                {
                    totalRecords = await _repository.GetRecordCount(request.Level, request.LevelCode);
                }
                else
                {
                    totalRecords= await _repository.GetRecordCount();
                }

                var pageCount = Custom.PageCount(request.Size, totalRecords);

                while (page <= pageCount)
                {
                    // Subjects
                    List<SubjectIndex> subjects;

                    if (request.Level == ScanLevel.Site)
                    {
                        subjects = await _repository.Read(page, request.Size, request.Level, request.LevelCode);
                    }
                    else
                    {
                        subjects = await _repository.Read(page, request.Size);
                    }

                    foreach (var subject in subjects)
                    {
                        var scores = new List<SubjectIndexScore>();
                        // Block

                        int blockPage = 1;
                        var totalBlockRecords = await _repository.GetBlockRecordCount(subject, request.Level);
                        var blockPageCount = Custom.PageCount(request.BlockSize, totalBlockRecords);

                        while (blockPage <= blockPageCount)
                        {
                            // SCORE

                            var otherSubjects =
                                await _repository.ReadBlock(page, request.BlockSize, subject, request.Level);

                            foreach (var otherSubject in otherSubjects)
                            {
                                var score = SubjectIndexScore.GenerateScore(subject, otherSubject, request.Level,
                                    _scorer, request.Field,request.LevelCode,configs);
                                scores.Add(score);
                            }

                            await _repository.CreateOrUpdateAsync<SubjectIndexScore, Guid>(scores);

                            // BLOCK NOTIFY

                            blockPage++;
                        }

                        // SUBJECT NOTIFY
                        await _mediator.Publish(new SubjectScanned(subject.Id, request.Level), cancellationToken);
                    }

                    page++;
                }

                return Result.Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(ScanSubjectHandler)} Error");
                return Result.Failure(e.Message);
            }
        }


    }
}
