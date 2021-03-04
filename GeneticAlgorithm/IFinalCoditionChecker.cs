namespace GeneticAlgorithm
{
    public interface IFinalCoditionChecker<I>
        where I : IIndividual
    {
        /// <summary>
        /// Set start settings to checker
        /// </summary>
        void Begin();

        /// <summary>
        /// Check if thet population is final
        /// </summary>
        /// <param name="currentPopulation">Current population</param>
        /// <returns>If that is final operation</returns>
        bool IsPopulationIsFinal(IPopulation<I> currentPopulation);
    }
}
