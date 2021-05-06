using System;
using System.ComponentModel.DataAnnotations;

namespace OPTEL.Data
{
    public class Order : Core.IDataObject
    {
        public int ID { get; set; }

        [Required]
        public string OrderNumber { get; set; }

        public double Width { get; set; }

        public double QuantityInRunningMeter { get; set; }

        public virtual FilmRecipe FilmRecipe { get; set; }

        public DateTime PlanningEndDate { get; set; }

        public double PriceOverdue { get; set; }

        public double PredefinedTime { get; set; }

        public virtual Customer ParentCustomer { get; set; }
    }
}
