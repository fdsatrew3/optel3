using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Core
{
    public interface IFitnessFunctionCalculator<I>
        where I : IIndividual
    {
        /// <summary>
        /// Calculate fitness function value of individual
        /// </summary>
        /// <param name="individual">Target individual</param>
        /// <returns>Fitness function value</returns>
        double Calculate(I individual);
    }
}
