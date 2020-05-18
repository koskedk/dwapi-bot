using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Dwapi.Bot.Core.Application.Configs.Queries;
using Dwapi.Bot.Core.Application.Indices.Commands;
using Dwapi.Bot.Core.Application.Matching.Commands;
using Dwapi.Bot.Core.Domain.Configs;
using Dwapi.Bot.Core.Domain.Indices;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Dwapi.Bot.Controllers
{
    [Route("api/[controller]")]
    public class WorkerController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ISubjectIndexRepository _repository;

        public WorkerController(IMediator mediator, ISubjectIndexRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        [HttpPost("Refresh")]
        public async Task<ActionResult> Get(RefreshIndex command)
        {
            if (command.BatchSize <= 0)
                return BadRequest();

            try
            {
                var results = await _mediator.Send(command);

                if (results.IsSuccess)
                    return Ok("Refreshing...");

                throw new Exception(results.Error);
            }
            catch (Exception e)
            {
                var msg = $"Error executing {nameof(RefreshIndex)}(s)";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }

        [HttpPost("Scan")]
        public async Task<ActionResult> Get(ScanDto command)
        {
            var results = new List<Result>();
            var siteCodes = new List<int>();

            if (null==command)
                return BadRequest();

            try
            {
                if (command.AllSites)
                {
                    var sites = await _repository.GetSubjectSiteDtos();
                    siteCodes.AddRange(sites.Select(x=>x.SiteCode).ToList());
                }
                else
                {
                    siteCodes = command.Sites.ToList();
                }

                var commands = command.GenerateCommands(siteCodes);
                foreach (var scanCommand in commands)
                {
                    var result=  await _mediator.Send(scanCommand);
                    results.Add(result);
                }

                if (results.Any(x=>!x.IsFailure))
                    return Ok("Scanning...");

                var scb=new StringBuilder("Errors scanning:");
                foreach (var result in results)
                {
                    scb.AppendLine(result.Error);
                }
                throw new Exception(scb.ToString());
            }
            catch (Exception e)
            {
                var msg = $"Error executing {nameof(ScanSubject)}(s)";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }
    }
}
