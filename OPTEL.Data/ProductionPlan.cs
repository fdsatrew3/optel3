using System.Collections.Generic;

namespace OPTEL.Data
{
    public class ProductionPlan
    {
        public ICollection<ProductionLineQueue> ProductionLineQueues { get; set; }
    }
}
