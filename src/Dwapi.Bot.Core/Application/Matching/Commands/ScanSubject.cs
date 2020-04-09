using CSharpFunctionalExtensions;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;

namespace Dwapi.Bot.Core.Application.Matching.Commands
{
    public class ScanSubject:IRequest<Result>
    {
        public ScanLevel Level { get; }
        public string LevelCode { get;  }
        public int Size { get;  }
        public int BlockSize { get;  }
        public SubjectField Field { get; }

        public ScanSubject(ScanLevel level, string levelCode, int size, int blockSize, SubjectField field)
        {
            Level = level;
            LevelCode = levelCode;
            Size = size;
            BlockSize = blockSize;
            Field = field;
        }
    }
}
