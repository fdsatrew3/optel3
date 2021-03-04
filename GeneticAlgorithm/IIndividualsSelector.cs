using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public interface IIndividualsSelector<I>
        where I : ICalculatedIndividual
    {
        /// <summary>
        /// Select some individuals from collection
        /// </summary>
        /// <param name="individualsCollection">Collection of individuals to select</param>
        /// <returns>Selected individuals</returns>
        IEnumerable<I> SelectIndividuals(ICollection<I> individualsCollection);
    }
}
