using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Moq;

using GeneticAlgorithmTest.Core;
using GeneticAlgorithm;
using GeneticAlgorithm.IndividualsSelectors;

namespace GeneticAlgorithmTest.IndividualsSelectors
{
    public class UniformRankingIndividualsSelectorTest : IndividualSelectorTestClass
    {
        protected const int DEFAULT_SELECTION_COUNT = 2;
        protected const int DEFAULT_INDIVIDUALS_COUNT_COEFFICIENT = 2;

        protected Random Random { get; }

        public UniformRankingIndividualsSelectorTest()
        {
            Random = new Random();
        }

        [Test]
        public void Random_IsNull_ConstructorExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new UniformRankingIndividualsSelector<ICalculatedIndividual>(null,
                    DEFAULT_SELECTION_COUNT)
                );
        }

        [Test]
        [TestCase(0)]
        [TestCase(-6)]
        [TestCase(int.MinValue)]
        public void SelectionCount_IsOutOfRange_ConstructorExceptionTest(int selectionCount)
        {
            Assert.IsNotNull(Random);

            Assert.Throws<ArgumentOutOfRangeException>(
                () => new UniformRankingIndividualsSelector<ICalculatedIndividual>(Random,
                    selectionCount)
                );
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-24)]
        [TestCase(int.MinValue)]
        public void MaxSelectionCount_LesserThanZero_ConstructorExceptionTest(int maxSelectionCount)
        {
            Assert.IsNotNull(Random);

            Assert.Throws<ArgumentOutOfRangeException>(
                () => new UniformRankingIndividualsSelector<ICalculatedIndividual>(Random,
                    DEFAULT_SELECTION_COUNT,
                    maxSelectionCount)
                );
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(8, 20)]
        [TestCase(int.MaxValue - 1, int.MaxValue)]
        public void MaxSelectionCount_LesserThanSelectionCount_ConstructorExceptionTest(int maxSelectionCount, int selectionCount)
        {
            Assert.IsNotNull(Random);

            Assert.Throws<ArgumentOutOfRangeException>(
                () => new UniformRankingIndividualsSelector<ICalculatedIndividual>(Random,
                    selectionCount,
                    maxSelectionCount)
                );
        }

        [Test]
        [TestCase(DEFAULT_SELECTION_COUNT)]
        [TestCase(16)]
        [TestCase(64)]
        public void ReturnInividualsCountEqualToSelectionCount_SelectPopulationTest(int selectionCount)
        {
            // Arrange
            var individuals = CreateIndividuals(selectionCount * DEFAULT_INDIVIDUALS_COUNT_COEFFICIENT);

            Assert.IsNotNull(Random);

            // Act
            var testObj = new UniformRankingIndividualsSelector<ICalculatedIndividual>(Random, selectionCount);

            var result = testObj.SelectIndividuals(individuals);

            // Assert
            Assert.AreEqual(selectionCount, result.Count());
        }
    }
}
