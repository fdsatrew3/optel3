using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Core
{
    public interface IStartPopulationCreator<I>
        where I : ICalculatedIndividual
    {
        /// <summary>
        /// Create start population
        /// </summary>
        /// <returns>Start population</returns>
        IPopulation<I> CreateStartPopulation(int count);
    }
}
