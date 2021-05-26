using System.Linq;
using OPTEL.Data;
using OPTEL.Data.Users;
using OPTEL.Entity.Core;
using OPTEL.Entity.Helpers.Ensurers;
using OPTEL.Entity.Helpers.Exceptions;
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
        public void SaveDatabase_BaseEntitiesAdded_RunSuccessfully()
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

        [Fact]
        public void AddEntity_DifferentTypesOfUsersThroughChildrenRepository_CanBeGetFromParentRepository()
        {
            // Arrange
            var administrator = new Administrator { Login = "Biba", Password = "pswrdA" };
            var productionDirector = new ProductionDirector { Login = "Pupa", Password = "pswrdP" };
            var knowledgeEngineer = new KnowledgeEngineer { Login = "Lupa", Password = "pswrdK" };
                        
            // Act
            _unitOfWork.AdministratorRepository.Add(administrator);
            _unitOfWork.ProductionDirectorRepository.Add(productionDirector);
            _unitOfWork.KnowledgeEngineerRepository.Add(knowledgeEngineer);

            _unitOfWork.Save();

            // Assert
            Assert.Single(_unitOfWork.AdministratorRepository.GetAll());
            Assert.Single(_unitOfWork.ProductionDirectorRepository.GetAll());
            Assert.Single(_unitOfWork.KnowledgeEngineerRepository.GetAll());

            Assert.Equal(3, _unitOfWork.UserRepository.GetAll().Count());
        }

        [Fact]
        public void AddEntity_DifferentTypesOfUsersThroughParentRepository_CanBeGetFromChildreRepository()
        {
            // Arrange
            var administrator = new Administrator { Login = "Biba", Password = "pswrdA" };
            var productionDirector = new ProductionDirector { Login = "Pupa", Password = "pswrdP" };
            var knowledgeEngineer = new KnowledgeEngineer { Login = "Lupa", Password = "pswrdK" };
                        
            // Act
            _unitOfWork.UserRepository.Add(administrator);
            _unitOfWork.UserRepository.Add(productionDirector);
            _unitOfWork.UserRepository.Add(knowledgeEngineer);

            _unitOfWork.Save();

            // Assert
            Assert.Equal(3, _unitOfWork.UserRepository.GetAll().Count());

            Assert.Single(_unitOfWork.AdministratorRepository.GetAll());
            Assert.Single(_unitOfWork.ProductionDirectorRepository.GetAll());
            Assert.Single(_unitOfWork.KnowledgeEngineerRepository.GetAll());
        }

        [Theory]
        [InlineData("Biba","ps")]
        [InlineData("Bib","pswrd")]
        [InlineData("BibaBobaLupaPupaAndCompanyAndFamily","pswrd")]
        [InlineData("Biba","TheLongOnePasswordThatWasCreatedByGods")]
        public void AddEntity_LoginOrPasswordNotInLengthRange_ThrowsDbEntityValidationException(string login, string password)
        {
            // Arrange
            var administrator = new Administrator { Login = login, Password = password };
                        
            // Act & Assert
            _unitOfWork.UserRepository.Add(administrator);

            Assert.Throws<DbEntityValidationException>(() => _unitOfWork.Save());
        }
    }
}
