using OPTEL.Data;
using System.Collections.Generic;

namespace OPTEL.Optimization.Algorithms.Bruteforce.Data
{
    public class ProductionLineQueue
    {
        public ProductionLine ProductionLine { get; set; }

        public List<Order> Orders { get; set; }

        public decimal SummaryTime { get; }
    }
}
