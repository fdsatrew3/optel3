using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.Algorithms.Genetic.Services.IndividualsSelectors;
using Optimization.Algorithms.Genetic.Services.Operators.Mutations;

using OPTEL.Optimization.Algorithms.Genetic.Data;
using OPTEL.Optimization.Algorithms.Genetic.Services.Util.Base;
using OPTEL.Optimization.Algorithms.Genetic.Services.Util.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Operators.Mutations
{
    public class InversionMutationOperator : MutationOperatorBase<ProductionPlan>
    {
        private readonly Random _random;
        private readonly IProductionPlanUnwarper _productionPlanUnwarper;
        private readonly IOrderPositionSwaper _orderPositionSwaper;

        public InversionMutationOperator(
            Random random, 
            IProductionPlanUnwarper productionPlanUnwarper, 
            IOrderPositionSwaper orderPositionSwaper,
            IndividualsSelector<ProductionPlan> individualsSelector
            ) : base(individualsSelector)
        {
            _random = random ?? throw new ArgumentNullException(nameof(random));
            _productionPlanUnwarper = productionPlanUnwarper ?? throw new ArgumentNullException(nameof(productionPlanUnwarper));
            _orderPositionSwaper = orderPositionSwaper ?? throw new ArgumentNullException(nameof(orderPositionSwaper));
        }

        protected override void MakeMutation(ProductionPlan individual)
        {
            var unwarpedPlan = _productionPlanUnwarper.Unwarp(individual);

            var firstIndex = _random.Next(unwarpedPlan.Count);
            var secondIndex = _random.Next(unwarpedPlan.Count);

            if (firstIndex != secondIndex)
            {
                IEnumerable<OrderPosition> orderPositions;

                if (firstIndex <= secondIndex)
                    orderPositions = unwarpedPlan.Skip(firstIndex).Take(secondIndex - firstIndex);
                else
                    orderPositions = unwarpedPlan.Skip(secondIndex).Take(firstIndex - secondIndex);

                Inverse(individual, orderPositions.ToArray());
            }
        }

        private void Inverse(ProductionPlan individual, ICollection<OrderPosition> orderPositions)
        {
            var firstIndex = 0;
            var lastIndex = orderPositions.Count - 1;

            while(firstIndex < lastIndex)
            {
                _orderPositionSwaper.Swap(individual, orderPositions.ElementAt(firstIndex++), orderPositions.ElementAt(lastIndex--));
            }
        }
    }
}
