using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPTEL.Data
{
    public class Order : Core.IDataObject
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required]
        public string OrderNumber { get; set; }

        public decimal Width { get; set; }

        public decimal QuantityInRunningMeter { get; set; }

        public virtual FilmRecipe FilmRecipe { get; set; }

        public DateTime PlanningEndDate { get; set; }

        public decimal PriceOverdue { get; set; }

        public decimal PredefinedTime { get; set; }

        public virtual Customer ParentCustomer { get; set; }
    }
}
