using OPTEL.Data;
using OPTEL.UI.Desktop.Models.Base;

namespace OPTEL.UI.Desktop.Models
{
    public class PlanningConfigOrder : Order, IDataModel
    {
        public bool IsSelected { get; set; }

        public PlanningConfigOrder(Order order)
        {
            FilmRecipe = order.FilmRecipe;
            FilmRecipeID = order.FilmRecipeID;
            ID = order.ID;
            OrderNumber = order.OrderNumber;
            ParentCustomer = order.ParentCustomer;
            ParentCustomerID = order.ParentCustomerID;
            PlanningEndDate = order.PlanningEndDate;
            PredefinedTime = order.PredefinedTime;
            PriceOverdue = order.PriceOverdue;
            QuantityInRunningMeter = order.QuantityInRunningMeter;
            Width = order.Width;
            IsSelected = false;
        }
    }
}
