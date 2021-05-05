using System;
using System.Linq;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Application.Catalogs.Commands;
using Dwapi.Bot.Core.Application.Indices.Commands;
using Dwapi.Bot.Core.Application.Matching.Commands;
using Dwapi.Bot.Core.Domain.Catalogs.Dtos;
using Dwapi.Bot.Core.Domain.Indices.Dto;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Dwapi.Bot.Controllers
{
    [Route("api/[controller]")]
    public class WorkerController : Controller
    {
        private readonly IMediator _mediator;

        public WorkerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Scan")]
        public async Task<ActionResult> Post([FromBody] ScanDto command)
        {
            if (command.Level < 0 || command.Level > 2)
                return BadRequest(
                    new
                    {
                        Status = "Unknown Scan Level, Allowed only 0 Site,1 InterSite,2 Both",
                        Date = DateTime.Now
                    });

            try
            {
                var clearJobResult = await _mediator.Send(new ClearIndex((ScanLevel) command.Level,command.DataSet));

                if (clearJobResult.IsFailure)
                    throw new Exception(clearJobResult.Error);

                return Ok(
                    new
                    {
                        Status = "Scan STARTED",
                        Date = DateTime.Now,
                        Ref = clearJobResult.Value
                    });
            }
            catch (Exception e)
            {
                var msg = $"Error executing {nameof(ClearIndex)}(s)";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }

        [HttpPost("ReBlock")]
        public async Task<ActionResult> ReBlock([FromBody] ReBlockDto command)
        {
            if (command.Level < 0 || command.Level > 2)
                return BadRequest(
                    new
                    {
                        Status = "Unknown Scan Level, Allowed only 0 Site,1 InterSite,2 Both",
                        Date = DateTime.Now
                    });

            try
            {
                var reBlockJobResult = await _mediator.Send(new ReBlockIndex((ScanLevel) command.Level));

                if (reBlockJobResult.IsFailure)
                    throw new Exception(reBlockJobResult.Error);

                return Ok(
                    new
                    {
                        Status = "ReBlock STARTED",
                        Date = DateTime.Now,
                        Ref = reBlockJobResult.Value
                    });
            }
            catch (Exception e)
            {
                var msg = $"Error executing {nameof(ReBlockIndex)}(s)";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }

        [HttpPost("LoadSites")]
        public async Task<ActionResult> Post([FromBody] LoadSites command)
        {
            if (null==command)
                return BadRequest(
                    new
                    {
                        Status = "No command available",
                        Date = DateTime.Now
                    });

            try
            {
                var clearJobResult = await _mediator.Send(new LoadSites());
                var loadSubjects = await _mediator.Send(new LoadSubjects("jx"));
                var subjectJobResult = await _mediator.Send(new MarkPreferredSubjects("jx3"));
                var extractsJobResult = await _mediator.Send(new MarkPreferredExtracts("jx4"));

                return Ok(
                    new
                    {
                        Status = "Sites Loaded",
                        Date = DateTime.Now,
                        Ref = ""
                    });
            }
            catch (Exception e)
            {
                var msg = $"Error executing {nameof(CleanUpSites)}(s)";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }

        [HttpPost("CleanDwh")]
        public async Task<ActionResult> Post([FromBody] CleanSiteDto command)
        {
            if (null==command)
            return BadRequest(
                    new
                    {
                        Status = "No command available",
                        Date = DateTime.Now
                    });

            try
            {
                var clearJobResult = command.SiteCodes.Any()
                    ? await _mediator.Send(new CleanUpSites(command.SiteCodes))
                    : await  _mediator.Send(new CleanUpSites());

                return Ok(
                    new
                    {
                        Status = "Clean Up STARTED",
                        Date = DateTime.Now,
                        Ref = ""
                    });
            }
            catch (Exception e)
            {
                var msg = $"Error executing {nameof(CleanUpSites)}(s)";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }
    }
}
