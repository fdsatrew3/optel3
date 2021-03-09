using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Optimization.Algorithms.Genetic.Core;
using Optimization.Algorithms.Genetic.Data;
using Optimization.Algorithms.Genetic.Services.IndividualsSelectors;
using Xunit;

namespace Optimization.Algorithms.Genetic.Services.Test.IndividualsSelectors
{
    public class TournamentIndividualsSelectorTest : IndividualSelectorTestClass
    {
        private const int _defaultTeamCapacity = TournamentIndividualsSelector<ICalculatedIndividual>.MIN_TEAM_CAPACITY;

        private readonly Mock<IBestSelector<ICalculatedIndividual>> _bestSelectorMock;

        private IBestSelector<ICalculatedIndividual> BestSelector => _bestSelectorMock.Object;

        public TournamentIndividualsSelectorTest()
        {
            _bestSelectorMock = new Mock<IBestSelector<ICalculatedIndividual>>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-6)]
        [InlineData(int.MinValue)]
        public void TeamCapacity_IsOutOfRange_ConstructorExceptionTest(int teamCapacity)
        {
            // Arrange
            Assert.NotNull(BestSelector);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new TournamentIndividualsSelector<ICalculatedIndividual>(BestSelector, teamCapacity)
                );
        }

        [Fact]
        public void BestSelector_IsNull_ConstructorExceptionTest()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new TournamentIndividualsSelector<ICalculatedIndividual>(null, _defaultTeamCapacity)
                );
        }

        [Theory]
        [InlineData(6)]
        [InlineData(16)]
        [InlineData(64)]
        public void ReturnBestIndividual_FromOneTeam_SelectPopulationTest(int teamCapacity)
        {
            // Arrange
            var individuals = CreateIndividuals(teamCapacity);
            var bestIndividual = individuals.First();

            _bestSelectorMock.Setup(x => x.SelectBestIndividual(It.IsAny<IEnumerable<ICalculatedIndividual>>())).Returns(bestIndividual);
            Assert.Equal(bestIndividual, BestSelector.SelectBestIndividual(individuals));

            // Act
            var testObject = new TournamentIndividualsSelector<ICalculatedIndividual>(BestSelector, teamCapacity);
            var result = testObject.SelectIndividuals(individuals);

            // Assert
            _bestSelectorMock.Verify(x => x.SelectBestIndividual(It.IsAny<IEnumerable<ICalculatedIndividual>>()));

            Assert.Single(result);
            Assert.Equal(bestIndividual, result.First());
        }
    }
}
