using System;
using System.Linq;
using Optimization.Algorithms.Genetic.Data;
using Optimization.Algorithms.Genetic.Services.IndividualsSelectors;
using Xunit;

namespace Optimization.Algorithms.Genetic.Services.Test.IndividualsSelectors
{
    public class UniformRankingIndividualsSelectorTest : IndividualSelectorTestClass
    {
        private const int _defaultSelectionCount = 2;
        private const int _defaultIndividualsCountCoefficient = 2;

        private readonly Random _random;

        public UniformRankingIndividualsSelectorTest()
        {
            _random = new Random();
        }

        [Fact]
        public void Random_IsNull_ConstructorExceptionTest()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new UniformRankingIndividualsSelector<ICalculatedIndividual>(null,
                    _defaultSelectionCount)
                );
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-6)]
        [InlineData(int.MinValue)]
        public void SelectionCount_IsOutOfRange_ConstructorExceptionTest(int selectionCount)
        {
            // Arrange
            Assert.NotNull(_random);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new UniformRankingIndividualsSelector<ICalculatedIndividual>(_random,
                    selectionCount)
                );
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-24)]
        [InlineData(int.MinValue)]
        public void MaxSelectionCount_LesserThanZero_ConstructorExceptionTest(int maxSelectionCount)
        {
            // Arrange
            Assert.NotNull(_random);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new UniformRankingIndividualsSelector<ICalculatedIndividual>(_random,
                    _defaultSelectionCount,
                    maxSelectionCount)
                );
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(8, 20)]
        [InlineData(int.MaxValue - 1, int.MaxValue)]
        public void MaxSelectionCount_LesserThanSelectionCount_ConstructorExceptionTest(int maxSelectionCount, int selectionCount)
        {
            // Arrange
            Assert.NotNull(_random);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new UniformRankingIndividualsSelector<ICalculatedIndividual>(_random,
                    selectionCount,
                    maxSelectionCount)
                );
        }

        [Theory]
        [InlineData(_defaultSelectionCount)]
        [InlineData(16)]
        [InlineData(64)]
        public void ReturnInividualsCountEqualToSelectionCount_SelectPopulationTest(int selectionCount)
        {
            // Arrange
            var individuals = CreateIndividuals(selectionCount * _defaultIndividualsCountCoefficient);
            var testObj = new UniformRankingIndividualsSelector<ICalculatedIndividual>(_random, selectionCount);

            // Act
            var result = testObj.SelectIndividuals(individuals);

            // Assert
            Assert.Equal(selectionCount, result.Count());
        }
    }
}
