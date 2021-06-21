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

        public int FilmRecipeID { get; set; }

        public virtual FilmRecipe FilmRecipe { get; set; }

        public DateTime PlanningEndDate { get; set; }

        public double PriceOverdue { get; set; }

        public double FinishedGoods { get; set; }

        public double Waste { get; set; }

        public int RollsCount { get; set; }

        public double? PredefinedTime { get; set; }

        public int ParentCustomerID { get; set; }

        public virtual Customer ParentCustomer { get; set; }

        public double RollWeight => FinishedGoods + Waste;

        public double Weight => RollWeight * RollsCount;
    }
}
