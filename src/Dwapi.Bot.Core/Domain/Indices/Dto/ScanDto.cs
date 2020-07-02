using System.Collections.Generic;
using Dwapi.Bot.Core.Application.Matching.Commands;
using Dwapi.Bot.SharedKernel.Enums;

namespace Dwapi.Bot.Core.Domain.Indices.Dto
{
    public class ScanDto
    {
        public bool AllSites { get; set; }
        public int[] Sites { get; set; }
    }
}
