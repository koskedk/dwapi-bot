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
        public string LevelCode { get; }
        public int Size { get; }
        public int BlockSize { get; }
        public SubjectField Field { get; }

        public ScanSubjectInter(string levelCode,SubjectField field = SubjectField.PKV, int size = 500, int blockSize = 500)
        {
            LevelCode = levelCode;
            Size = size;
            BlockSize = blockSize;
            Field = field;
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
           Log.Debug($"scanning {request.LevelCode} ...");

            var configs = _configRepository.GetConfigs().ToList();

            try
            {

                var blocks = await _repository.GetInterSiteBlocks();
                var siteBlocks = blocks.ToList();


                int blockCount = 1;

                foreach (var siteBlock in siteBlocks)
                {
                    int page = 1;
                    var totalRecords = await _repository.GetRecordCount(ScanLevel.InterSite,siteBlock);

                    var pageCount = Custom.PageCount(request.Size, totalRecords);

                    while (page <= pageCount)
                    {
                        Log.Debug($"Scanning page {page}/{pageCount}...");

                        // Subjects
                        var subjectIndices = await _repository.Read(page, request.Size, ScanLevel.InterSite, siteBlock);
                        var subjects = subjectIndices.ToList();
                        int subIndex = 0;
                        var subjectsCount = subjects.Count;

                        foreach (var subject in subjects)
                        {
                            subIndex++;
                            Log.Debug($"Scanning page {page}/{pageCount} | Subject {subIndex} of {subjectsCount}...");
                            var scores = new List<SubjectIndexScore>();

                                // SCORE

                                var otherSubjects = subjects.Where(x => x.Id != subject.Id);

                                foreach (var otherSubject in otherSubjects)
                                {
                                    var score = SubjectIndexScore.GenerateScore(subject, otherSubject, ScanLevel.InterSite,
                                        _scorer, request.Field, request.LevelCode, configs);
                                    scores.Add(score);
                                }

                                await _repository.Merge<SubjectIndexScore, Guid>(scores);

                                await _mediator.Publish(new BlockScanned(siteBlock, ScanLevel.InterSite, ScanStatus.Scanned),
                                    cancellationToken);  await _mediator.Publish(new BlockScanned(siteBlock, ScanLevel.Site, ScanStatus.Scanned),
                                    cancellationToken);
                        }

                        page++;
                    }

                    blockCount++;
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
