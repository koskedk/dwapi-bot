using Dwapi.Bot.SharedKernel.Interfaces.App;

namespace Dwapi.Bot.Core.Application.Common
{
    public class AppSetting:IAppSetting
    {
        public bool WorkflowEnabled { get;  }
        public int BlockSize { get;  }
        public int BatchSize { get;  }

        public AppSetting(bool workflowEnabled, int blockSize, int batchSize)
        {
            WorkflowEnabled = workflowEnabled;
            BlockSize = blockSize;
            BatchSize = batchSize;
        }
    }
}
