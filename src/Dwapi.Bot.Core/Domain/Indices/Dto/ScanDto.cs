using System.Collections.Generic;
using Dwapi.Bot.Core.Application.Matching.Commands;
using Dwapi.Bot.SharedKernel.Enums;

namespace Dwapi.Bot.Core.Domain.Indices.Dto
{
    public class ScanDto
    {
        public bool AllSites { get; set; }
        public int[] Sites { get; set; }

        public List<ScanSubject> GenerateCommands(List<int>  siteCodes)
        {
            var commands=new List<ScanSubject>();
            if (AllSites)
            {
                foreach (var site in siteCodes)
                {
                    commands.Add(new ScanSubject(site.ToString()));
                }
            }
            else
            {
                foreach (var site in Sites)
                {
                    commands.Add(new ScanSubject(site.ToString()));
                }
            }
            return commands;
        }
    }
}
