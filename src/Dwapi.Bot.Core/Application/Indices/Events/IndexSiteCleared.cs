using Dwapi.Bot.Core.Domain.Indices.Dto;
using MediatR;

namespace Dwapi.Bot.Core.Application.Indices.Events
{
    public class IndexSiteCleared:INotification
    {
        public SubjectSiteDto SubjectSite { get;  }

        public IndexSiteCleared(SubjectSiteDto subjectSite)
        {
            SubjectSite = subjectSite;
        }
    }
}