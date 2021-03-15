﻿using System;
using System.Collections.Generic;
using System.Linq;
using OPTEL.Data;
using Optimization.Algorithms.Genetic.Core;
using Optimization.Algorithms.Genetic.Data;
using Optimization.Algorithms.Genetic.Services.Base;
using Optimization.Algorithms.Utilities.Extensions;

using OPTEL.Optimization.Algorithms.Genetic.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.StartPopulationGenerator
{
    public class RandomStartPopulationGenerator : IStartPopulationGenerator<ProductionPlan>
    {
        private readonly Random _random;
        private readonly ICollection<Extruder> _extruders;
        private readonly ICollection<Order> _orders;

        public RandomStartPopulationGenerator(Random random, ICollection<Extruder> extruders, ICollection<Order> orders)
        {
            _random = random ?? throw new ArgumentNullException(nameof(random));
            _extruders = extruders ?? throw new ArgumentNullException(nameof(extruders));
            _orders = orders ?? throw new ArgumentNullException(nameof(orders));
        }

        public IPopulation<ProductionPlan> CreateStartPopulation(int count)
        {
            var result = new Population<ProductionPlan>();

            for (int i = 0; i < count; i++)
            {
                result.AddIndividual(GenerateRandomProductionPlan());
            }

            return result;
        }

        private ProductionPlan GenerateRandomProductionPlan()
        {
            var result = new ProductionPlan { ProductionLineQueues = new List<ProductionLineQueue>() };

            foreach(var extruder in _extruders)
            {
                result.ProductionLineQueues.Add(new ProductionLineQueue { Extruder = extruder, Orders = new List<Order>() });
            }

            foreach(var order in _random.NextElements(_orders))
            {
                var queue = GetShortestProductionLineQueue(result.ProductionLineQueues);
                queue.Orders.Add(order);
            }

            return result;
        }

        private ProductionLineQueue GetShortestProductionLineQueue(IEnumerable<ProductionLineQueue> productionLineQueues)
        {
            var minValue = productionLineQueues.Min(x => x.SummaryTime);
            
            return productionLineQueues.FirstOrDefault(x => x.SummaryTime == minValue);
        }
    }
}
