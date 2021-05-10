using OPTEL.Data;
using OPTEL.UI.Desktop.Models;
using OPTEL.UI.Desktop.Services.ModelsConverter.Base;

namespace OPTEL.UI.Desktop.Services.ModelsConverter
{
    public class PlanningConfigOrderToOrderConverterService : IModelConverterService<PlanningConfigOrder, Order>
    {
        public int ID { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public PlanningConfigOrder Convert(Order source)
        {
            return new PlanningConfigOrder(source);
        }

        public Order ConvertBack(PlanningConfigOrder source)
        {
            Order order = new Order
            {
                FilmRecipe = source.FilmRecipe,
                FilmRecipeID = source.FilmRecipeID,
                ID = source.ID,
                OrderNumber = source.OrderNumber,
                ParentCustomer = source.ParentCustomer,
                ParentCustomerID = source.ParentCustomerID,
                PlanningEndDate = source.PlanningEndDate,
                PredefinedTime = source.PredefinedTime,
                PriceOverdue = source.PriceOverdue,
                QuantityInRunningMeter = source.QuantityInRunningMeter,
                Width = source.Width
                
            };
            return order;
        }
    }
}
