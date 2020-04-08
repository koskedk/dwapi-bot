using AutoMapper;
using Dwapi.Bot.Core.Domain.Readers;

namespace Dwapi.Bot.Core.Domain.Indices.Dto
{
    public class PatientIndexProfile:Profile
    {
        public PatientIndexProfile()
        {
            CreateMap<MasterPatientIndex, PatientIndex>();
        }
    }
}
