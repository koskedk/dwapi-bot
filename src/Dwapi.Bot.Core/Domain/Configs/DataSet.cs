using System;
using Dwapi.Bot.SharedKernel.Model;

namespace Dwapi.Bot.Core.Domain.Configs
{
    public class DataSet:Entity<Guid>
    {
        public string Name { get; set; }
        public string Description{ get; set; }
        public string Definition { get; set; }

        public DataSet()
        {
        }

        public DataSet(string name, string description, string definition)
        {
            Name = name;
            Description = description;
            Definition = definition;
        }

        public void UpdateDefinition(string definition)
        {
            Definition = definition;
        }
    }
}