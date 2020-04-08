namespace Dwapi.Bot.Core.Utility
{
    public interface IScorer<T,TA,TB>
    {
        T Generate(TA inputA, TB inputB);
    }
}
