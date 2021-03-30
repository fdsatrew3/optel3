using System;
using System.Collections.Generic;
using System.Linq;
using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Data
{
    public class ProductionLineQueue : ICloneable
    {
        public ProductionLine ProductionLine { get; set; }

        public List<Order> Orders { get; set; }

        public decimal SummaryTime { get; }

        public object Clone()
        {
            return new ProductionLineQueue { ProductionLine = ProductionLine, Orders = Orders.ToList() };
        }
    }
}
