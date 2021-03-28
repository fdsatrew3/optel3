using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.Algorithms.Genetic.Data;
using Optimization.Algorithms.Genetic.Operators.Crossovers;
using Optimization.Algorithms.Genetic.Services.Operators.Crossovers;

using OPTEL.Optimization.Algorithms.Genetic.Data;
using OPTEL.Optimization.Algorithms.Genetic.Services.Util.Base;
using OPTEL.Optimization.Algorithms.Genetic.Services.Util.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Operators.Crossovers
{
    public class MultipointedCrossoverOperator : CrossoverOperatorBase<ProductionPlan>
    {
        private readonly int _pointCount;
        private readonly Random _random;
        private readonly IProductionPlanUnwarper _productionPlanUnwarper;

        public MultipointedCrossoverOperator(int pointCount, Random random, IProductionPlanUnwarper productionPlanUnwarper, ICrossoverOperatorSelector<ProductionPlan> crossoverOperatorSelector) 
            : base(crossoverOperatorSelector)
        {
            if (pointCount < 1)
                throw new ArgumentOutOfRangeException(nameof(pointCount), "Point count should be greater than or equal to 1");

            _pointCount = pointCount;
            _random = random ?? throw new ArgumentNullException(nameof(random));
            _productionPlanUnwarper = productionPlanUnwarper ?? throw new ArgumentNullException(nameof(productionPlanUnwarper));
        }

        protected override IEnumerable<ProductionPlan> CreateChildren(Parents<ProductionPlan> parents)
        {
            var ordersCount = parents.FirstParent.ProductionLineQueues.SelectMany(x => x.Orders).Count();
            var points = GetPointCollection(ordersCount);

            yield return CreateChild(parents.FirstParent, parents.SecondParent, points, true);
            yield return CreateChild(parents.SecondParent, parents.FirstParent, points, false);
        }

        private int[] GetPointCollection(int elementsCount)
        {
            var result = new int[_pointCount];

            for (int i = 0; i < result.Length; i++)
            {
                int startPoint = i == 0 ? 0 : result[i - 1];
                result[i] = _random.Next(startPoint, elementsCount - _pointCount + i);
            }

            return result;
        }

        private ProductionPlan CreateChild(ProductionPlan firstParent, ProductionPlan secondParent, int[] points, bool useOddIntervals)
        {
            var convertedProductionPlan = _productionPlanUnwarper.Unwarp(firstParent);
            var intervals = GetOrdersIntervals(convertedProductionPlan, points);

            return CreateChild(secondParent, intervals, useOddIntervals);
        }

        private ICollection<ICollection<OrderPosition>> GetOrdersIntervals(ICollection<OrderPosition> orderPositions, int[] points)
        {
            var result = new List<ICollection<OrderPosition>>();

            for(int i = 0; i < points.Length; i++)
            {
                var startPosition = i == 0 ? 0 : points[i - 1];

                result.Add(orderPositions.Skip(startPosition).Take(points[i] - startPosition).ToList());
            }

            return result;
        }

        private ProductionPlan CreateChild(ProductionPlan parent, ICollection<ICollection<OrderPosition>> intervals, bool useOddIntervals)
        {
            var result = parent.Clone() as ProductionPlan;

            foreach(var interval in intervals.Where((x, index) => (index % 2 == 0) != useOddIntervals))
            {
                foreach (var productionLine in interval.Select(x => x.ProductionLine).GroupBy(x => x))
                {
                    var productionLineInterval = interval.Where(x => x.ProductionLine == productionLine).ToArray();
                    var queue = result.ProductionLineQueues.First(x => x.Extruder == productionLine);
                    var orders = productionLineInterval.Select(x => x.Order).ToArray();

                    queue.Orders.RemoveAll(x => productionLineInterval.Any(y => y.Order == x));
                    queue.Orders.InsertRange(productionLineInterval.First().Position, orders);
                }
            }

            return result;
        }
    }
}
