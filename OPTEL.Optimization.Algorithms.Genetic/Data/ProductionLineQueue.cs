using System;
using System.Collections.Generic;
using System.Linq;
using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Data
{
    public class ProductionLineQueue : ICloneable
    {
        public Extruder Extruder { get; set; }

        public List<Order> Orders { get; set; }

        public object Clone()
        {
            return new ProductionLineQueue { Extruder = Extruder, Orders = Orders.ToList() };
        }
    }
}
