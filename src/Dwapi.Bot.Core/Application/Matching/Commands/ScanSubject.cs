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

        public ScanSubject(SubjectField field = SubjectField.PKV, int size = 500, int blockSize = 500)
        {
            Level = ScanLevel.InterSite;
            Size = size;
            BlockSize = blockSize;
            Field = field;
        }

        public ScanSubject(string levelCode, SubjectField field = SubjectField.PKV, int size = 500, int blockSize = 500)
        :this(field, size, blockSize)
        {
            Level = ScanLevel.Site;
            LevelCode = levelCode;
        }
    }
}
