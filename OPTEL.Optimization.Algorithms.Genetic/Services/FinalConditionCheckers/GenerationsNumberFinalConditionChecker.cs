using System;
using Optimization.Algorithms.Genetic.Core;
using Optimization.Algorithms.Genetic.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.FinalConditionCheckers
{
    public class GenerationsNumberFinalConditionChecker<I> : IFinalCoditionChecker<I>
        where I : ICalculatedIndividual
    {
        protected int GenerationCount { get; }

        private int _currentGeneration;

        public GenerationsNumberFinalConditionChecker(int generationCount)
        {
            if (generationCount < 1)
                throw new ArgumentOutOfRangeException(nameof(generationCount), "Generation count should be greater than 1");

            GenerationCount = generationCount;
        }

        /// <summary>
        /// Set start settings to checker
        /// </summary>
        public void Begin()
        {
            _currentGeneration = 0;
        }

        /// <summary>
        /// Check if thet population is final
        /// </summary>
        /// <param name="currentPopulation">Current population</param>
        /// <returns>If that is final operation</returns>
        public bool IsPopulationIsFinal(IPopulation<I> currentPopulation)
        {
            _currentGeneration++;

            return _currentGeneration > GenerationCount;
        }
    }
}
