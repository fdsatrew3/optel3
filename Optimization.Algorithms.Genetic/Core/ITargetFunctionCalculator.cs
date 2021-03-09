namespace Optimization.Algorithms.Genetic.Data
{
    public interface ITargetFunctionCalculator<I>
        where I : IIndividual
    {
        /// <summary>
        /// Calculate target function value of individual
        /// </summary>
        /// <param name="individual">Target individual</param>
        /// <returns>Target function value</returns>
        double Calculate(I individual);
    }
}