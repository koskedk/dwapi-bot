using Dwapi.Bot.Core.Domain.Indices.Dto;
using MediatR;

namespace Dwapi.Bot.Core.Application.Indices.Events
{
    public class IndexSiteBlocked:INotification
    {
        public SubjectBlockDto SubjectBlock { get;  }

        public IndexSiteBlocked(SubjectBlockDto subjectBlock)
        {
            SubjectBlock = subjectBlock;
        }
    }
}