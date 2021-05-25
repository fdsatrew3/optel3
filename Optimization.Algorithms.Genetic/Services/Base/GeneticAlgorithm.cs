using System.Linq;
using System.Collections.Generic;
using Optimization.Algorithms.Core;
using Optimization.Algorithms.Utilities.Extensions;

using Optimization.Algorithms.Genetic.Core;
using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Services.Base
{
    public class GeneticAlgorithm<I> : IGeneticAlgorithm<I>, IGeneticAlgorithmsDecisions<I>
        where I : class, ICalculatedIndividual, ICloneableIndividual
    {
        protected IGeneticAlgorithmSetting<I> GeneticAlgorithmSetting { get; }

        public GeneticAlgorithm(IGeneticAlgorithmSetting<I> geneticAlgorithmSetting)
        {
            GeneticAlgorithmSetting = geneticAlgorithmSetting;
        }

        I IOptimizationAlgorithm<I>.GetResolve()
        {
            I bestResolve = null;

            foreach (var resolve in GetResolvesInternal())
            {
                if (bestResolve == null || bestResolve.FitnessFunctionValue < resolve.FitnessFunctionValue)
                {
                    bestResolve = resolve.Clone() as I;
                }
            }

            return bestResolve;
        }

        IEnumerable<I> IOptimizationAlgorithm<IEnumerable<I>>.GetResolve()
        {
            I bestResolve = null;

            foreach(var resolve in GetResolvesInternal())
            {
                if (bestResolve == null || bestResolve.FitnessFunctionValue < resolve.FitnessFunctionValue)
                {
                    bestResolve = resolve.Clone() as I;
                    yield return resolve;
                }
            }
        }

        private IEnumerable<I> GetResolvesInternal()
        {
            var currentPopulation = GeneticAlgorithmSetting.StartPopulationCreator.CreateStartPopulation(GeneticAlgorithmSetting.MaxPopulationCount);

            GeneticAlgorithmSetting.FinalConditionCheckers.ForEach(x => x.Begin());

            while (true)
            {
                var children = GeneticAlgorithmSetting.CrossoverOperator.CreateChildren(currentPopulation);

                currentPopulation.AddIndividuals(children);

                GeneticAlgorithmSetting.MutationOperator.MakeMutation(currentPopulation);

                currentPopulation = GeneticAlgorithmSetting.PopulationSelector.SelectPopulation(currentPopulation);

                yield return GeneticAlgorithmSetting.BestSelector.SelectBestIndividual(currentPopulation);

                if (GeneticAlgorithmSetting.FinalConditionCheckers.Any(x => x.IsStateFinal(currentPopulation)))
                    break;
            }
        }
    }
}
