using Dwapi.Bot.Core.Application.Indices.Commands;

namespace Dwapi.Bot.Core.Domain.Indices.Dto
{
    public class RefreshIndexDto
    {
        public int BatchSize { get; set; }

        public RefreshIndexDto()
        {
        }

        public RefreshIndex GenerateCommand()
        {
            return new RefreshIndex(BatchSize);
        }
    }
}
