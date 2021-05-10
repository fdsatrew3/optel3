using OPTEL.Data;
using OPTEL.Entity.Core;
using OPTEL.Entity.Helpers.Ensurers;
using OPTEL.Entity.Persistance;
using Xunit;

namespace OPTEL.Entity.Test
{
    public class DatabaseIntegrationTest
    {
        private readonly IUnitOfWork _unitOfWork;

        public DatabaseIntegrationTest()
        {
            _unitOfWork = new UnitOfWork(new TestingDataBaseEnsurer());
        }

        [Fact]
        public void CreateDungeon_SavingSmallDungeonToDatabase_HasNoException()
        {
            // Arrange
            var customer = new Customer { Name = "I am customer" };
            var filmType = new FilmType { Article = "I am film type" };
            var filmRecipe = new FilmRecipe { Name = "I am film recipe", FilmType = filmType, Thickness = 1, Calibration = 1, CoolingLip = 1, MaterialCost = 1, Nozzle = 1, ProductionSpeed = 1 };
            var order = new Order { OrderNumber = "Some number", FilmRecipe = filmRecipe, ParentCustomer = customer };
            
            _unitOfWork.OrderRepository.Add(order);

            // Act
            var exception = Record.Exception(() => 
            { 
                _unitOfWork.Save(); 
            });

            // Assert
            Assert.Null(exception);
        }
    }
}
