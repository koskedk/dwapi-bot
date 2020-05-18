using AutoMapper;
using Dwapi.Bot.Core.Domain.Readers;

namespace Dwapi.Bot.Core.Domain.Indices.Dto
{
    public class SubjectIndexProfile:Profile
    {
        public SubjectIndexProfile()
        {
            CreateMap<MasterPatientIndex, SubjectIndex>()
                .ForMember(x=>x.Id,o=>o.Ignore())
                .ForMember(x=>x.MpiId, o=>o.MapFrom(s=>s.Id));
        }
    }
}
