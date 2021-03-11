using Optimization.Algorithms.Core;
using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Core
{
    public interface IGeneticAlgorithmsDecisions<I> : IOptimizationAlgorithmDecisions<I>
        where I : IIndividual
    {

    }
}
