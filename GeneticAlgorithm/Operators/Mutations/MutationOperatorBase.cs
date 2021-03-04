using System;

namespace GeneticAlgorithm.Operators.Mutations
{
    public abstract class MutationOperatorBase<I> : IMutationOperator<I>
        where I : ICalculatedIndividual
    {
        protected IIndividualsSelector<I> IndividualsSelector { get; }

        public MutationOperatorBase(IIndividualsSelector<I> individualsSelector)
        {
            IndividualsSelector = individualsSelector ?? throw new ArgumentNullException(nameof(individualsSelector), "Individuals selector should not be null");
        }

        /// <summary>
        /// Making mutation on population
        /// </summary>
        /// <param name="population">Population for mutation</param>
        /// <returns>Summary population</returns>
        public virtual void MakeMutation(IPopulation<I> population)
        {
            foreach(var individual in IndividualsSelector.SelectIndividuals(population.Individuals))
            {
                MakeMutation(individual);
            }
        }

        /// <summary>
        /// Making mutation on individual cc
        /// </summary>
        /// <param name="individual">Individual for mutation</param>
        protected abstract void MakeMutation(I individual);
    }
}
