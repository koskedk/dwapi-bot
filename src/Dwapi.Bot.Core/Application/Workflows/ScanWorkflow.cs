using System.Threading;
using System.Threading.Tasks;
using Dwapi.Bot.Core.Application.Indices.Commands;
using Dwapi.Bot.Core.Application.Indices.Events;
using Dwapi.Bot.Core.Application.Matching.Commands;
using Dwapi.Bot.Core.Application.Matching.Events;
using Dwapi.Bot.Core.Application.Workflows;
using Dwapi.Bot.SharedKernel.Enums;
using Dwapi.Bot.SharedKernel.Interfaces.App;
using MediatR;
using Serilog;

namespace Dwapi.Bot.Core.Application.WorkFlows
{
    public class ScanWorkFlow : IScanWorkflow
    {
        private readonly IAppSetting _setting;
        private readonly IMediator _mediator;

        public ScanWorkFlow(IAppSetting setting, IMediator mediator)
        {
            _setting = setting;
            _mediator = mediator;
        }

        public Task Handle(IndexCleared notification, CancellationToken cancellationToken)
        {
            if (_setting.WorkflowEnabled)
            {
                _mediator.Send(
                    new RefreshIndex(_setting.BatchSize, notification.JobId, notification.Level, notification.Dataset),
                    cancellationToken);
            }

            return Task.CompletedTask;
        }

        public Task Handle(IndexRefreshed notification, CancellationToken cancellationToken)
        {
            if (_setting.WorkflowEnabled)
            {
                if (notification.Level == ScanLevel.Both)
                {
                    _mediator.Send(new BlockIndex(notification.JobId, ScanLevel.Site, true), cancellationToken);
                }
                else
                {
                    _mediator.Send(new BlockIndex(notification.JobId, notification.Level), cancellationToken);
                }
            }

            return Task.CompletedTask;
        }

        public Task Handle(PartIndexBlocked notification, CancellationToken cancellationToken)
        {
            if (_setting.WorkflowEnabled)
            {
                _mediator.Send(new BlockIndex(notification.JobId, ScanLevel.InterSite, false,true), cancellationToken);
            }

            return Task.CompletedTask;
        }

        public Task Handle(IndexBlocked notification, CancellationToken cancellationToken)
        {
            if (_setting.WorkflowEnabled)
            {
                if (notification.Level == ScanLevel.Both)
                {
                    _mediator.Send(
                        new ScanIndex(notification.JobId, ScanLevel.Site, SubjectField.PKV, _setting.BlockSize),
                        cancellationToken);

                    _mediator.Send(
                        new ScanIndex(notification.JobId, ScanLevel.InterSite, SubjectField.PKV, _setting.BlockSize),
                        cancellationToken);
                }
                else
                {
                    _mediator.Send(
                        new ScanIndex(notification.JobId, notification.Level, SubjectField.PKV, _setting.BlockSize),
                        cancellationToken);
                }
            }

            return Task.CompletedTask;
        }

        public Task Handle(IndexScanned notification, CancellationToken cancellationToken)
        {
            Log.Debug(new string('=', 50));
            Log.Debug(new string('+', 50));
            Log.Debug("SCAN COMPLETED");
            Log.Debug("SCAN COMPLETED");
            Log.Debug("SCAN COMPLETED");
            Log.Debug("SCAN COMPLETED");
            Log.Debug(new string('+', 50));
            Log.Debug(new string('=', 50));
            return Task.CompletedTask;
        }
    }
}
