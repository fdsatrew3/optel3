using System;
using System.Linq;
using Optimization.Algorithms.Genetic.Data;
using Optimization.Algorithms.Genetic.Services.IndividualsSelectors.ProbabilityIndividualsSelectors;
using Optimization.Algorithms.Utilities.Probabilities;
using Xunit;

namespace Optimization.Algorithms.Genetic.Services.Test.IndividualsSelectors.ProbabilityIndividualsSelectors
{
    public class RouletteIndividualsSelectorTest : IndividualSelectorTestClass
    {
        private const int _defaultSelectionCount = 2;

        private readonly Random _random;
        private readonly RandomProbabilityChecker _randomProbabilityChecker;

        public RouletteIndividualsSelectorTest()
        {
            _random = new Random();
            _randomProbabilityChecker = new RandomProbabilityChecker(_random);
        }

        [Fact]
        public void RandomProbabilityChecker_IsNull_ConstructorExceptionTest()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new RouletteIndividualsSelector<ICalculatedIndividual>(null,
                    _defaultSelectionCount)
                );
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-6)]
        [InlineData(int.MinValue)]
        public void SelectionCount_IsOutOfRange_ConstructorExceptionTest(int selectionCount)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new RouletteIndividualsSelector<ICalculatedIndividual>(_randomProbabilityChecker,
                    selectionCount)
                );
        }

        [Theory]
        [InlineData(_defaultSelectionCount)]
        [InlineData(16)]
        [InlineData(32)]
        [InlineData(64)]
        public void ReturnInividualsCountEqualToSelectionCount_SelectIndividualsTest(int selectionCount)
        {
            // Arrange
            var individuals = CreateIndividuals(selectionCount);

            // Act
            var testObj = new RouletteIndividualsSelector<ICalculatedIndividual>(_randomProbabilityChecker,
                selectionCount);

            var result = testObj.SelectIndividuals(individuals).ToList();

            // Assert
            Assert.Equal(selectionCount, result.Count());
        }
    }
}
