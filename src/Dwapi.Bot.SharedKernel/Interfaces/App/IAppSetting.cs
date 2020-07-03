namespace Dwapi.Bot.SharedKernel.Interfaces.App
{
    public interface IAppSetting
    {
        bool WorkflowEnabled { get;  }
        int BlockSize { get; }
        int BatchSize { get;  }
    }
}
