using System;
using System.Linq;
using Optimization.Algorithms;

using Optimization.Algorithms.Genetic.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.FinalConditionCheckers
{
    public class PopulationFitnessDescendingFinalConditionChecker<I> : IFinalConditionChecker<IPopulation<I>>
        where I : ICalculatedIndividual
    {
        protected int GenerationCount { get; }

        private int _currentGeneration;
        private double _bestFitness;

        public PopulationFitnessDescendingFinalConditionChecker(int generationCount)
        {
            if (generationCount < 1)
                throw new ArgumentOutOfRangeException(nameof(generationCount), "Generation coune should be greater than 1");

            GenerationCount = generationCount;
        }

        /// <summary>
        /// Set start settings to checker
        /// </summary>
        public void Begin()
        {
            _currentGeneration = 0;
            _bestFitness = double.MinValue;
        }

        /// <summary>
        /// Check if thet population is final
        /// </summary>
        /// <param name="currentPopulation">Current population</param>
        /// <returns>If that is final operation</returns>
        public bool IsStateFinal(IPopulation<I> currentPopulation)
        {
            var bestFitness = currentPopulation.Individuals.Max(x => x.FitnessFunctionValue);

            if (_bestFitness >= bestFitness)
            {
                _currentGeneration++;
            }
            else
            {
                _currentGeneration = 0;
                _bestFitness = bestFitness;
            }

            return _currentGeneration >= GenerationCount;
        }
    }
}
