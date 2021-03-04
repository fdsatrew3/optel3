namespace GeneticAlgorithm
{
    public interface IFitnessFunctionCalculation<I>
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
