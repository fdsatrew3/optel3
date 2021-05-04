namespace OPTEL.Optimization.Algorithms
{
    public interface IFitnessCalculator<T>
    {
        /// <summary>
        /// Calculate fitness value of dicision
        /// </summary>
        /// <param name="dicision">Target dicision</param>
        /// <returns>Fitness function value</returns>
        double Calculate(T dicision);
    }
}
