using System.Linq;

using Utilities.Extensions;

namespace GeneticAlgorithm.Base
{
    public class GeneticAlgorithm<I> : IGeneticAlgorithm<I>
        where I : class, ICalculatedIndividual
    {
        protected IGeneticAlgorithmSetting<I> GeneticAlgorithmSetting { get; }

        public GeneticAlgorithm(IGeneticAlgorithmSetting<I> geneticAlgorithmSetting)
        {
            GeneticAlgorithmSetting = geneticAlgorithmSetting;
        }

        public virtual I GetResolve()
        {
            var currentPopulation = GeneticAlgorithmSetting.StartPopulationCreator.CreateStartPopulation(GeneticAlgorithmSetting.MaxPopulationCount);

            GeneticAlgorithmSetting.FinalCoditionCheckers.ForEach(x => x.Begin());

            while(true)
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
    }
}
