namespace OPTEL.Optimization.Algorithms.Bruteforce.Core
{
    public interface IFitnessFunctionCalculator<I>
    {
        /// <summary>
        /// Calculate fitness function value
        /// </summary>
        /// <param name="individual">Target object</param>
        /// <returns>Fitness function value</returns>
        double Calculate(I individual);
    }
}
