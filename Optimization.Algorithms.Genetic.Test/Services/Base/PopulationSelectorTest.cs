using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Optimization.Algorithms.Genetic.Core;
using Optimization.Algorithms.Genetic.Data;
using Xunit;

namespace Optimization.Algorithms.Genetic.Services.Base.Test
{
    public class PopulationSelectorTest
    {
        protected Mock<IIndividualsSelector<ICalculatedIndividual>> IndividualsSelectorMock { get; set; }

        protected IIndividualsSelector<ICalculatedIndividual> IndividualsSelector => IndividualsSelectorMock.Object;

        protected Mock<IPopulation<ICalculatedIndividual>> PopulationMock { get; set; }

        protected IPopulation<ICalculatedIndividual> Population => PopulationMock.Object;

        private ICollection<ICalculatedIndividual> CreateIndividuals(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count should be greater than 0");

            var individuals = new ICalculatedIndividual[count];

            for (int i = 0; i < individuals.Length; i++)
            {
                individuals[i] = Mock.Of<ICalculatedIndividual>();
            }

            return individuals.ToList();
        }

        public PopulationSelectorTest()
        {
            IndividualsSelectorMock = new Mock<IIndividualsSelector<ICalculatedIndividual>>();
            PopulationMock = new Mock<IPopulation<ICalculatedIndividual>>();
        }

        [Fact]
        public void Constructor_InputIndividualsSelectorIsNull_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new PopulationSelector<ICalculatedIndividual>(null)
                );
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void CurrentPopulationIndividuals_IsOutOfRange_SelectPopulationExceptionTest(int individualsCount)
        {
            // Arrange
            var individuals = CreateIndividuals(individualsCount);
            PopulationMock.Setup(x => x.Individuals).Returns(individuals);

            // Act
            var testObj = new PopulationSelector<ICalculatedIndividual>(IndividualsSelector);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => testObj.SelectPopulation(Population)
                );
        }
    }
}
