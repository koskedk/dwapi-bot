using System;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Application.Indices.Commands;
using Dwapi.Bot.Core.Application.Matching.Commands;
using Dwapi.Bot.Core.Domain.Indices;
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
        private readonly ISubjectIndexRepository _repository;

        public WorkerController(IMediator mediator, ISubjectIndexRepository repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        [HttpPost("Refresh")]
        public async Task<ActionResult> Post([FromBody] RefreshIndexDto command)
        {
            if (command.BatchSize <= 0)
                return BadRequest();

            try
            {
                var clearJobResult = await _mediator.Send(new ClearIndex());

                if (clearJobResult.IsFailure)
                    throw new Exception(clearJobResult.Error);

                var results = await _mediator.Send(new RefreshIndex(command.BatchSize, clearJobResult.Value));

                if (results.IsSuccess)
                    return Ok("Refreshing STARTED");

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
        public async Task<ActionResult> Scan([FromBody] ScanDto command)
        {

            if (null == command)
                return BadRequest();

            try
            {
                if (command.AllSites)
                {
                    var blockResult = await _mediator.Send(new BlockSubject());

                    if (blockResult.IsFailure)
                        throw new Exception(blockResult.Error);

                    var result = await _mediator.Send(new ScanSubject(blockResult.Value));

                    if (result.IsSuccess)
                        return Ok("Scan STARTED");

                    throw new Exception(result.Error);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                var msg = $"Error executing {nameof(ScanSubject)}(s)";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }

        [HttpPost("ResumeScan")]
        public async Task<ActionResult> ResumeScan([FromBody] ScanDto command)
        {

            if (null == command)
                return BadRequest();

            try
            {
                if (command.AllSites)
                {
                    var result = await _mediator.Send(new ScanSubject(string.Empty));
                    if (result.IsSuccess)
                        return Ok("Resume Scan STARTED");

                    throw new Exception(result.Error);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                var msg = $"Error executing {nameof(ScanSubject)}(s)";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }

        [HttpPost("Inter/Scan")]
        public async Task<ActionResult> InterScan([FromBody] ScanDto command)
        {

            if (null == command)
                return BadRequest();

            try
            {
                if (command.AllSites)
                {
                    var blockResult = await _mediator.Send(new BlockSubject(ScanLevel.InterSite));

                    if (blockResult.IsFailure)
                        throw new Exception(blockResult.Error);

                    var result = await _mediator.Send(new ScanSubject(blockResult.Value, ScanLevel.InterSite));

                    if (result.IsSuccess)
                        return Ok("Scan STARTED");

                    throw new Exception(result.Error);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                var msg = $"Error executing {nameof(ScanSubject)}(s)";
                Log.Error(e, msg);
                return StatusCode(500, $"{msg} {e.Message}");
            }
        }

        [HttpPost("Inter/ResumeScan")]
        public async Task<ActionResult> InterResumeScan([FromBody] ScanDto command)
        {

            if (null == command)
                return BadRequest();

            try
            {
                if (command.AllSites)
                {
                    var result = await _mediator.Send(new ScanSubject(string.Empty, ScanLevel.InterSite));
                    if (result.IsSuccess)
                        return Ok("Resume Scan STARTED");

                    throw new Exception(result.Error);
                }
                else
                {
                    return BadRequest();
                }
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
