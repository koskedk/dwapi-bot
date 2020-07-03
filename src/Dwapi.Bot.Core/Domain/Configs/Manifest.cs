using System;
using Dwapi.Bot.SharedKernel.Model;

namespace Dwapi.Bot.Core.Domain.Configs
{
    public class Manifest:Entity<Guid>
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public DateTime LogDate { get; set; }=DateTime.Now;

        public Manifest()
        {
        }

        public Manifest(string name,string version)
        {
            Version = version;
            Name = name;
        }
    }
}
