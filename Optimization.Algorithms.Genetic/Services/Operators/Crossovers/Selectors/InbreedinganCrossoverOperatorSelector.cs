using System;
using System.Collections.Generic;

using Optimization.Algorithms.Genetic.Core;
using Optimization.Algorithms.Genetic.Data;

namespace Optimization.Algorithms.Genetic.Services.Operators.Crossovers.Selectors
{
    public class InbreedinganCrossoverOperatorSelector<I> : CrossoverOperatorSelector<I>
        where I : ICalculatedIndividual
    {
        protected IFitnessFunctionCalculator<I> FitnessFunctionCalculation { get; }

        public InbreedinganCrossoverOperatorSelector(IIndividualsSelector<I> individualsSelector) : base(individualsSelector)
        {

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

            I result = default;
            double firstParentFitnessFunctionValue = firstParent.FitnessFunctionValue;
            double minDifference = double.MaxValue;

            foreach(var individual in parentsPull)
            {
                if (individual.Equals(firstParent))
                {
                    continue;
                }
                else
                {
                    var individualFitnessFunctionValue = individual.FitnessFunctionValue;
                    var difference = Math.Abs(firstParentFitnessFunctionValue - individualFitnessFunctionValue);

                    if (minDifference > difference)
                    {
                        result = individual;
                        minDifference = difference;
                    }
                }
            }

            return result;
        }
    }
}
