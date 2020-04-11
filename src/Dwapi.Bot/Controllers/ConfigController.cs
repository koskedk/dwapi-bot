using System;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Application.Configs.Queries;
using Dwapi.Bot.Core.Domain.Configs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Dwapi.Bot.Controllers
{
    [Route("api/[controller]")]
    public class ConfigController : Controller
    {
        private readonly IMediator _mediator;

        public ConfigController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Match")]
        public async Task<ActionResult> Get()
        {
            try
            {
                var results = await _mediator.Send(new GetConfig());

                if (results.IsSuccess)
                    return Ok(results.Value);

                throw new Exception(results.Error);
            }
            catch (Exception e)
            {
                var msg = $"Error loading {nameof(MatchConfig)}(s)";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }
    }
}
