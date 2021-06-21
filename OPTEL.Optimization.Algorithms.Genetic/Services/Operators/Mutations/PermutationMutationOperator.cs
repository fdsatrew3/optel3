using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.Algorithms.Genetic.Core;
using Optimization.Algorithms.Genetic.Services.Operators.Mutations;
using Optimization.Algorithms.Utilities.Extensions;

using OPTEL.Optimization.Algorithms.Genetic.Data;
using OPTEL.Optimization.Algorithms.Genetic.Services.Util.Base;
using OPTEL.Optimization.Algorithms.Genetic.Services.Util.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Operators.Mutations
{
    public class PermutationMutationOperator : MutationOperatorBase<ProductionPlan>
    {
        private readonly double _probability;
        private readonly Random _random;
        private readonly IProductionPlanUnwarper _productionPlanUnwarper;
        private readonly IOrderPositionSwaper _orderPositionSwaper;

        public PermutationMutationOperator(
            double probability, 
            Random random, 
            IProductionPlanUnwarper productionPlanUnwarper, 
            IOrderPositionSwaper orderPositionSwaper, 
            IIndividualsSelector<ProductionPlan> individualsSelector
            ) : base(individualsSelector)
        {
            if (probability < 0 || probability > 1)
                throw new ArgumentOutOfRangeException(nameof(probability), "Probability should be in range between 0 and 1.");

            _probability = probability;
            _random = random ?? throw new ArgumentNullException(nameof(random));
            _productionPlanUnwarper = productionPlanUnwarper ?? throw new ArgumentNullException(nameof(productionPlanUnwarper));
            _orderPositionSwaper = orderPositionSwaper ?? throw new ArgumentNullException(nameof(orderPositionSwaper));
        }

        protected override void MakeMutation(ProductionPlan individual)
        {
            var unwarpedPlan = _productionPlanUnwarper.Unwarp(individual);

            foreach(var orderPostition in SelectOrdersToMutate(unwarpedPlan))
            {
                var swapElement = _random.NextElement(unwarpedPlan);

                _orderPositionSwaper.Swap(individual, orderPostition, swapElement);
            }
        }

        private IEnumerable<OrderPosition> SelectOrdersToMutate(IEnumerable<OrderPosition> orderPositions)
        {
            foreach(var orderPosition in orderPositions)
            {
                if (_random.CheckProbability(_probability))
                    yield return orderPosition;
            }
        }
    }
}
