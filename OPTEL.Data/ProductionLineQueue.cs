using System.Collections.Generic;

namespace OPTEL.Data
{
    public class ProductionLineQueue
    {
        public ProductionLine ProductionLine { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
