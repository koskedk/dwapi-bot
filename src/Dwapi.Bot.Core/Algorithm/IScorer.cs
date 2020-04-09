namespace Dwapi.Bot.Core.Algorithm
{
    public interface IScorer<T,TA,TB>
    {
        T Generate(TA inputA, TB inputB);
    }
}
