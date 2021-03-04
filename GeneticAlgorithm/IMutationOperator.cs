namespace GeneticAlgorithm
{
    public interface IMutationOperator<I>
        where I : ICalculatedIndividual
    {
        /// <summary>
        /// Making mutation on population
        /// </summary>
        /// <param name="population">Current population</param>
        /// <returns>Summary population</returns>
        void MakeMutation(IPopulation<I> population);
    }
}
