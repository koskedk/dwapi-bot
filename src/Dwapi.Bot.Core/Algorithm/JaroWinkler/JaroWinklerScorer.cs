using System;

namespace Dwapi.Bot.Core.Algorithm.JaroWinkler
{
    public class JaroWinklerScorer:IJaroWinklerScorer
    {
        private readonly F23.StringSimilarity.JaroWinkler _jaroWinkler;

        public JaroWinklerScorer()
        {
            _jaroWinkler=new F23.StringSimilarity.JaroWinkler();
        }

        public double Generate(string inputA, string inputB)
        {
            if (string.IsNullOrWhiteSpace(inputA) || string.IsNullOrWhiteSpace(inputB))
                throw new ArgumentNullException("missing values");

            return _jaroWinkler.Similarity(inputA.ToLower(), inputB.ToLower());
        }
    }
}
