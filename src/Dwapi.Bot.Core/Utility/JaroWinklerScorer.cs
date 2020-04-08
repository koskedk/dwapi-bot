using System;
using F23.StringSimilarity;

namespace Dwapi.Bot.Core.Utility
{
    public class JaroWinklerScorer:IJaroWinklerScorer
    {
        private readonly JaroWinkler _jaroWinkler;

        public JaroWinklerScorer()
        {
            _jaroWinkler=new JaroWinkler();
        }

        public double Generate(string inputA, string inputB)
        {
            if (string.IsNullOrWhiteSpace(inputA) || string.IsNullOrWhiteSpace(inputB))
                throw new ArgumentNullException("missing values");

            return _jaroWinkler.Similarity(inputA.ToLower(), inputB.ToLower());
        }
    }
}
