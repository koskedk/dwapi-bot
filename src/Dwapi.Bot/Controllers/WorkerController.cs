using System;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Application.Indices.Commands;
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
                var clearJobResult = await _mediator.Send(new ClearIndex((ScanLevel) command.Level));

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
    }
}
