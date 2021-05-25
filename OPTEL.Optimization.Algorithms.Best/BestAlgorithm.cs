using System;
using System.Collections.Generic;
using System.Linq;
using OPTEL.Data;
using Optimization.Algorithms.Core;

namespace OPTEL.Optimization.Algorithms.Best
{
    public class BestAlgorithm : IOptimizationAlgorithm<ProductionPlan>
    {
        private readonly IEnumerable<ProductionLine> _productionLines;
        private readonly IEnumerable<Order> _orders;
        private readonly IEnumerable<FilmTypesChange> _filmTypesChanges;

        public BestAlgorithm(IEnumerable<ProductionLine> productionLines, IEnumerable<Order> orders, IEnumerable<FilmTypesChange> filmTypesChanges)
        {
            _productionLines = productionLines ?? throw new ArgumentNullException(nameof(productionLines));
            _orders = orders ?? throw new ArgumentNullException(nameof(orders));
            _filmTypesChanges = filmTypesChanges ?? throw new ArgumentNullException(nameof(filmTypesChanges));
        }

        public ProductionPlan GetResolve()
        {
            var result = new ProductionPlan { ProductionLineQueues = new List<ProductionLineQueue>() };
            var ordersByTime = _orders.OrderByDescending(order => order.PredefinedTime).ToArray();

            for (int i = 0; i < _productionLines.Count(); i++)
            {
                var orders = new List<Order>();

                for (int j = i; j < ordersByTime.Length; j += _productionLines.Count())
                {
                    orders.Add(ordersByTime[j]);
                }

                if (ordersByTime.Length % _productionLines.Count() != 0 && ordersByTime.Length % _productionLines.Count() <= i)
                {
                    int index = ordersByTime.Length / _productionLines.Count();

                    orders.Add(ordersByTime[index + i]);
                }

                result.ProductionLineQueues.Add(BestProductionPlan(_productionLines.ElementAt(i), orders));
            }

            return result;
        }
                
        private ProductionLineQueue BestProductionPlan(ProductionLine productionLine, List<Order> orders)
        {
            //берем все перенастройки
            var result = new ProductionLineQueue() { ProductionLine = productionLine, Orders = new List<Order>() };

            var extruderRecipeChange = _filmTypesChanges.OrderByDescending(x => x.ReconfigurationTime).ToArray();

            if (extruderRecipeChange.Length == 0)
                throw new Exception("Нет перенастроек");

            // для начала смотрим все перенастройки в поисках заказов, которые могли бы выполняться
            foreach (var change in extruderRecipeChange)
            {
                // если в списке заказов есть хотя бы один заказ по такому рецепту
                if (orders.Where(order => order.FilmRecipe.FilmType.Equals(change.FilmTypeTo)).Count() > 0)
                {
                    // выбираем все заказы по такому рецепту
                    foreach (var order in orders.Where(or => or.FilmRecipe.FilmType.Equals(change.FilmTypeTo))
                        .OrderBy(order => order.FilmRecipe.Nozzle)
                        .ThenBy(order => order.FilmRecipe.Calibration)
                        .ThenBy(order => order.FilmRecipe.CoolingLip))
                    {
                        result.Orders.Add(order); // и вставляем их в план
                    }

                    break; // заказы выбраны - сваливаем
                }
            }

            if (result.Orders.Count == 0)
                throw new Exception("Нет заказов");

            int l = 0;

            // Теперь будем вставлять остальные заказы в план в зависимости от рецепта предыдущего заказа (пока все заказы не забьем в план)
            while (result.Orders.Count < orders.Count && l < 2000)
            {
                // берем последний тип пленки в плане
                var lastFilmRecipe = result.Orders.Last().FilmRecipe;

                // находим все перенастройки с этого рецепта (отсортируем их по возрастанию сразу)
                var recipeChanges = extruderRecipeChange.Where(change => change.FilmTypeFrom.Equals(lastFilmRecipe.FilmType)).OrderBy(change => change.ReconfigurationTime);

                // идем от наименьшего времени перенастройки к большему, ищем заказы
                foreach(var change in recipeChanges)
                {
                    // если есть заказы с таким типом пленки
                    if (orders.Where(order => order.FilmRecipe.FilmType.Equals(change.FilmTypeTo)).Any())
                    {
                        // и такого типа пленки еще не было в плане
                        if (!result.Orders.Where(x => x.FilmRecipe.FilmType.Equals(change.FilmTypeTo)).Any())
                        {
                            // выбираем все заказы с таким типом пленки
                            foreach (var order in orders.Where(or => or.FilmRecipe.FilmType.Equals(change.FilmTypeTo))
                                .OrderBy(order => order.FilmRecipe.Nozzle)
                                .ThenBy(order => order.FilmRecipe.Calibration)
                                .ThenBy(order => order.FilmRecipe.CoolingLip))
                            {
                                result.Orders.Add(order); // кидаем их в план
                            }

                            break; // заказы нашлись - здесь больше делать нечего, смотрим следующий тип пленки (возвращаемся в while)
                        }
                    }
                }

                l++;
            }

            return result;
        }
    }
}
