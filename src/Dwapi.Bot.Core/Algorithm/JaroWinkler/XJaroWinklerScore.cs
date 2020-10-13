using System;
using Lucene.Net.Search.Spell;

namespace Dwapi.Bot.Core.Algorithm.JaroWinkler
{
    public class XJaroWinklerScorer : IJaroWinklerScorer
    {
        private readonly JaroWinklerDistance _jaroWinkler;

        public XJaroWinklerScorer()
        {
            _jaroWinkler = new JaroWinklerDistance();
            _jaroWinkler.Threshold = -1;
        }

        public double Generate(string inputA, string inputB, bool strict = true)
        {
            if ((strict) && (string.IsNullOrWhiteSpace(inputA) || string.IsNullOrWhiteSpace(inputB)))
                throw new ArgumentNullException("missing values");

            if ((!strict) && (string.IsNullOrWhiteSpace(inputA) || string.IsNullOrWhiteSpace(inputB)))
                return 0;


            return _jaroWinkler.GetDistance(inputA.ToLower(), inputB.ToLower());

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
