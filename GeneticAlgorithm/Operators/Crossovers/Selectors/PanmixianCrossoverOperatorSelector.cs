using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Operators.Crossovers.Selectors
{
    public class PanmixianCrossoverOperatorSelector<I> : CrossoverOperatorSelector<I>
        where I : ICalculatedIndividual
    {
        protected Random Random { get; }

        public PanmixianCrossoverOperatorSelector(Random random, IIndividualsSelector<I> individualsSelector) : base(individualsSelector)
        {
            Random = random ?? throw new ArgumentNullException(nameof(random), "Random should not be null");
        }

        /// <summary>
        /// Find second parent to individual from parents pull
        /// </summary>
        /// <param name="firstParent">First parent to find pair</param>
        /// <param name="parentsPull">All individuals pull</param>
        /// <returns>Second parent</returns>
        protected override I SelectSecondParent(I firstParent, ICollection<I> parentsPull)
        {
            if (parentsPull.Count == 1)
                return firstParent;

            var index = Random.Next(0, parentsPull.Count - 1);

            if (parentsPull.ElementAt(index).Equals(firstParent))
                return parentsPull.ElementAt(index + 1);
            else
                return parentsPull.ElementAt(index);
        }
    }
}
