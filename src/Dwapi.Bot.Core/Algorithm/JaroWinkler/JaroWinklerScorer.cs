using System;

namespace Dwapi.Bot.Core.Algorithm.JaroWinkler
{
    public class JaroWinklerScorer : IJaroWinklerScorer
    {
        private readonly F23.StringSimilarity.JaroWinkler _jaroWinkler;

        public JaroWinklerScorer()
        {
            _jaroWinkler = new F23.StringSimilarity.JaroWinkler();
        }

        public double Generate(string inputA, string inputB, bool strict = true)
        {
            if ((strict) && (string.IsNullOrWhiteSpace(inputA) || string.IsNullOrWhiteSpace(inputB)))
                throw new ArgumentNullException("missing values");

            if ((!strict) && (string.IsNullOrWhiteSpace(inputA) || string.IsNullOrWhiteSpace(inputB)))
                return 0;


            return _jaroWinkler.Similarity(inputA.ToLower(), inputB.ToLower());

        }

        public double GenerateExact(string inputA, string inputB, bool strict = false)
        {
            if ((strict) && (string.IsNullOrWhiteSpace(inputA) || string.IsNullOrWhiteSpace(inputB)))
                throw new ArgumentNullException("missing values");

            if ((!strict) && (string.IsNullOrWhiteSpace(inputA) || string.IsNullOrWhiteSpace(inputB)))
                return 0;

            if (inputA.ToLower() == inputB.ToLower())
                return 1;

            return 0;
        }
    }
}
