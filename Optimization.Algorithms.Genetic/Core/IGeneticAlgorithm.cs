using Optimization.Algorithms.Core;
using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Core
{
    public interface IGeneticAlgorithm<I> : IOptimizationAlgorithm<I>
        where I : IIndividual
    {

    }
}
