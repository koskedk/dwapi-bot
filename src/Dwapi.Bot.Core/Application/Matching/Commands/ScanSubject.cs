using CSharpFunctionalExtensions;
using Dwapi.Bot.SharedKernel.Enums;
using MediatR;

namespace Dwapi.Bot.Core.Application.Matching.Commands
{
    public class ScanSubject : IRequest<Result>
    {
        public ScanLevel Level { get; }
        public string LevelCode { get; }
        public int Size { get; }
        public int BlockSize { get; }
        public SubjectField Field { get; }

        public ScanSubject( int size = 500, int blockSize = 500, SubjectField field = SubjectField.PKV)
        {
            Level = ScanLevel.InterSite;
            Size = size;
            BlockSize = blockSize;
            Field = field;
        }

        public ScanSubject(string levelCode, int size = 500, int blockSize = 500, SubjectField field = SubjectField.PKV)
        :this(size,blockSize,field)
        {
            Level = ScanLevel.Site;
            LevelCode = levelCode;
        }
    }
}
