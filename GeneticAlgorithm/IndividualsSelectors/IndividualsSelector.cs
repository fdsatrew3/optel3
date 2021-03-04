using System;
using System.Collections.Generic;

namespace GeneticAlgorithm.IndividualsSelectors
{
    public abstract class IndividualsSelector<I> : IIndividualsSelector<I>
        where I : ICalculatedIndividual
    {
        /// <summary>
        /// Select some individuals from collection
        /// </summary>
        /// <param name="individualsCollection">Collection of individuals to select</param>
        /// <returns>Selected individuals</returns>
        public IEnumerable<I> SelectIndividuals(ICollection<I> individualsCollection)
        {
            if (individualsCollection.Count <= 0)
                throw new ArgumentException("Current population individuals count should be grater than 0", nameof(individualsCollection));

            return SelectIndividualsInternal(individualsCollection);
        }

        /// <summary>
        /// Select some individuals from collection
        /// </summary>
        /// <param name="individualsCollection">Collection of individuals to select</param>
        /// <returns>Selected individuals</returns>
        protected abstract IEnumerable<I> SelectIndividualsInternal(ICollection<I> individualsCollection);
    }
}
