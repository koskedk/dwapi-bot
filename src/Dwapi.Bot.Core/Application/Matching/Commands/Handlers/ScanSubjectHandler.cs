using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Dwapi.Bot.Core.Algorithm.JaroWinkler;
using Dwapi.Bot.Core.Application.Matching.Events;
using Dwapi.Bot.Core.Domain.Indices;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.Matching.Commands.Handlers
{
    public class ScanSubjectHandler : IRequestHandler<ScanSubject, Result>
    {
        private readonly IMediator _mediator;
        private readonly ISubjectIndexRepository _repository;
        private readonly IJaroWinklerScorer _scorer;

        public ScanSubjectHandler(IMediator mediator, ISubjectIndexRepository repository, IJaroWinklerScorer scorer)
        {
            _mediator = mediator;
            _repository = repository;
            _scorer = scorer;
        }

        public async Task<Result> Handle(ScanSubject request, CancellationToken cancellationToken)
        {
            Log.Debug($"scanning {request.Level}:{request.LevelCode} ...");
            try
            {
                int page = 1;
                var totalRecords = await _repository.GetRecordCount();
                var pageCount = _repository.PageCount(request.Size, totalRecords);

                while (page <= pageCount)
                {
                    // Subjects

                    var subjects =await  _repository.Read(page, request.Size);

                    foreach (var subject in subjects)
                    {
                       var scores=new List<SubjectIndexScore>();
                        // Block

                        int blockPage = 1;
                        var totalBlockRecords = await _repository.GetBlockRecordCount(subject, request.Level);
                        var blockPageCount = _repository.PageCount(request.BlockSize, totalBlockRecords);
                        while (blockPage <= blockPageCount)
                        {
                            // SCORE

                            var otherSubjects =
                                await _repository.ReadBlock(page, request.BlockSize, subject, request.Level);

                            foreach (var otherSubject in otherSubjects)
                            {
                                var score = new SubjectIndexScore();
                                score.GenerateScore(subject, otherSubject, _scorer,request.Field);

                                scores.Add(score);
                            }

                            await _repository.CreateOrUpdateAsync<SubjectIndexScore,Guid>(scores);

                            // BLOCK NOTIFY

                            blockPage++;
                        }
                    }

                    // SUBJECT NOTIFY

                    page++;
                }


                return Result.Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, $"{nameof(ScanSubjectHandler)} Error");
                return Result.Fail(e.Message);
            }
        }


    }
}
