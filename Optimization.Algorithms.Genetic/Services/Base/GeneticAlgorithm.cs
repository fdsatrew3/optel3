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
            var currentPopulation = GeneticAlgorithmSetting.StartPopulationCreator.CreateStartPopulation(GeneticAlgorithmSetting.MaxPopulationCount);

            GeneticAlgorithmSetting.FinalCoditionCheckers.ForEach(x => x.Begin());

            while (true)
            {
                var children = GeneticAlgorithmSetting.CrossoverOperator.CreateChildren(currentPopulation);

                currentPopulation.AddIndividuals(children);

                GeneticAlgorithmSetting.MutationOperator.MakeMutation(currentPopulation);

                currentPopulation = GeneticAlgorithmSetting.PopulationSelector.SelectPopulation(currentPopulation);

                if (GeneticAlgorithmSetting.FinalCoditionCheckers.Any(x => x.IsPopulationIsFinal(currentPopulation)))
                    break;
            }

            var bestIndividual = GeneticAlgorithmSetting.BestSelector.SelectBestIndividual(currentPopulation);

            return bestIndividual;
        }

        IEnumerable<I> IOptimizationAlgorithm<IEnumerable<I>>.GetResolve()
        {
            var currentPopulation = GeneticAlgorithmSetting.StartPopulationCreator.CreateStartPopulation(GeneticAlgorithmSetting.MaxPopulationCount);

            GeneticAlgorithmSetting.FinalCoditionCheckers.ForEach(x => x.Begin());

            while (true)
            {
                var children = GeneticAlgorithmSetting.CrossoverOperator.CreateChildren(currentPopulation);

                currentPopulation.AddIndividuals(children);

                GeneticAlgorithmSetting.MutationOperator.MakeMutation(currentPopulation);

                currentPopulation = GeneticAlgorithmSetting.PopulationSelector.SelectPopulation(currentPopulation);

                if (GeneticAlgorithmSetting.FinalCoditionCheckers.Any(x => x.IsPopulationIsFinal(currentPopulation)))
                    break;
                else
                    yield return GeneticAlgorithmSetting.BestSelector.SelectBestIndividual(currentPopulation).Clone() as I;
            }

            yield return GeneticAlgorithmSetting.BestSelector.SelectBestIndividual(currentPopulation);
        }
    }
}
