using System;
using System.Linq;
using Optimization.Algorithms.Genetic.Data;
using Optimization.Algorithms.Genetic.Services.IndividualsSelectors.ProbabilityIndividualsSelectors;
using Optimization.Algorithms.Utilities.Probabilities;
using Xunit;

namespace Optimization.Algorithms.Genetic.Services.Test.IndividualsSelectors.ProbabilityIndividualsSelectors
{
    public class LinearRankingIndividualsSelectorTest : IndividualSelectorTestClass
    {
        private const double _defaultCoefficient = 1.5;
        private const int _defaultSelectionCount = 2;
        private const int _defaultIndividualsCountCoefficient = 2;

        private readonly Random _random;
        private readonly RandomProbabilityChecker _randomProbabilityChecker;

        public LinearRankingIndividualsSelectorTest()
        {
            _random = new Random();
            _randomProbabilityChecker = new RandomProbabilityChecker(_random);
        }

        [Fact]
        public void RandomProbabilityChecker_IsNull_ConstructorExceptionTest()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new LinearRankingIndividualsSelector<ICalculatedIndividual>(_defaultCoefficient,
                    null,
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
                () => new LinearRankingIndividualsSelector<ICalculatedIndividual>(_defaultCoefficient,
                    _randomProbabilityChecker,
                    selectionCount)
                );
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.99)]
        [InlineData(-1.87)]
        [InlineData(double.MinValue)]
        [InlineData(2.0001)]
        [InlineData(5.99)]
        [InlineData(double.MaxValue)]
        public void Coefficient_IsOutOfRange_ConstructorExceptionTest(double coefficient)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new LinearRankingIndividualsSelector<ICalculatedIndividual>(coefficient,
                    _randomProbabilityChecker,
                    _defaultSelectionCount)
                );
        }

        [Theory]
        [InlineData(_defaultSelectionCount)]
        [InlineData(16)]
        [InlineData(32)]
        [InlineData(64)]
        public void ReturnInividualsCountEqualToSelectionCount_SelectPopulationTest(int selectionCount)
        {
            // Arrange
            var individuals = CreateIndividuals(selectionCount * _defaultIndividualsCountCoefficient);
            var testObj = new LinearRankingIndividualsSelector<ICalculatedIndividual>(_defaultCoefficient,
                _randomProbabilityChecker,
                selectionCount);

            // Act
            var result = testObj.SelectIndividuals(individuals);

            // Assert
            Assert.Equal(selectionCount, result.Count());
        }
    }
}
