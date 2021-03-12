using System.Collections.Generic;
using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Data
{
    public class ProductionLineQueue
    {
        public Extruder Extruder { get; set; }

        public List<Order> Orders { get; set; }
    }
}
