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

namespace Dwapi.Bot.Core.Application.Matching.Commands
{
    public class ScanSubjectInter : IRequest<Result>
    {
        public ScanLevel Level { get; }
        public string LevelCode { get; }
        public int Size { get; }
        public int BlockSize { get; }
        public SubjectField Field { get; }



        public ScanSubjectInter(SubjectField field = SubjectField.PKV, int size = 500, int blockSize = 500)
        {
            Level = ScanLevel.InterSite;
            Size = size;
            BlockSize = blockSize;
            Field = field;
        }

        public ScanSubjectInter(string levelCode, SubjectField field = SubjectField.PKV, int size = 500, int blockSize = 500)
        :this(field, size, blockSize)
        {
            Level = ScanLevel.Site;
            LevelCode = levelCode;
        }
    }

     public class ScanSubjectInterHandler : IRequestHandler<ScanSubjectInter, Result>
    {
        private readonly IMediator _mediator;
        private readonly ISubjectIndexRepository _repository;
        private readonly IMatchConfigRepository _configRepository;
        private readonly IJaroWinklerScorer _scorer;

        public ScanSubjectInterHandler(IMediator mediator, ISubjectIndexRepository repository, IJaroWinklerScorer scorer, IMatchConfigRepository configRepository)
        {
            _mediator = mediator;
            _repository = repository;
            _scorer = scorer;
            _configRepository = configRepository;
        }

        public async Task<Result> Handle(ScanSubjectInter request, CancellationToken cancellationToken)
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
                    Log.Debug($"Scanning page {page}/{pageCount}...");
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

                    int subIndex = 0;
                    var subjectsCount = subjects.Count;
                    foreach (var subject in subjects)
                    {
                        subIndex++;
                        Log.Debug($"Scanning page {page}/{pageCount} | Subject {subIndex} of {subjectsCount}...");
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
                Log.Error(e, $"{nameof(ScanSubjectInterHandler)} Error");
                return Result.Failure(e.Message);
            }
        }


    }
}
