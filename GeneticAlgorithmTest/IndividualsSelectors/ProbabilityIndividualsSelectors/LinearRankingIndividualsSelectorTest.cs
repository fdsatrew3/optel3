using System;
using System.Linq;
using NUnit.Framework;

using Optimization.Algorithms.Utilities.Probabilities;
using GeneticAlgorithm;
using GeneticAlgorithm.IndividualsSelectors.ProbabilityIndividualsSelectors;
using GeneticAlgorithmTest.Core;

namespace GeneticAlgorithmTest.IndividualsSelectors.ProbabilityIndividualsSelectors
{
    public class LinearRankingIndividualsSelectorTest : IndividualSelectorTestClass
    {
        protected const double DEFAULT_COEFFICIENT = 1.5;
        protected const int DEFAULT_SELECTION_COUNT = 2;
        protected const int DEFAULT_INDIVIDUALS_COUNT_COEFFICIENT = 2;

        protected Random Random { get; }

        protected RandomProbabilityChecker RandomProbabilityChecker { get; }

        public LinearRankingIndividualsSelectorTest()
        {
            Random = new Random();
            RandomProbabilityChecker = new RandomProbabilityChecker(Random);
        }

        [Test]
        public void RandomProbabilityChecker_IsNull_ConstructorExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new LinearRankingIndividualsSelector<ICalculatedIndividual>(DEFAULT_COEFFICIENT,
                    null,
                    DEFAULT_SELECTION_COUNT)
                );
        }

        [Test]
        [TestCase(0)]
        [TestCase(-6)]
        [TestCase(int.MinValue)]
        public void SelectionCount_IsOutOfRange_ConstructorExceptionTest(int selectionCount)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new LinearRankingIndividualsSelector<ICalculatedIndividual>(DEFAULT_COEFFICIENT,
                    RandomProbabilityChecker,
                    selectionCount)
                );
        }

        [Test]
        [TestCase(0)]
        [TestCase(0.99)]
        [TestCase(-1.87)]
        [TestCase(double.MinValue)]
        [TestCase(2.0001)]
        [TestCase(5.99)]
        [TestCase(double.MaxValue)]
        public void Coefficient_IsOutOfRange_ConstructorExceptionTest(double coefficient)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new LinearRankingIndividualsSelector<ICalculatedIndividual>(coefficient,
                    RandomProbabilityChecker,
                    DEFAULT_SELECTION_COUNT)
                );
        }

        [Test]
        [TestCase(DEFAULT_SELECTION_COUNT)]
        [TestCase(16)]
        [TestCase(32)]
        [TestCase(64)]
        public void ReturnInividualsCountEqualToSelectionCount_SelectPopulationTest(int selectionCount)
        {
            // Arrange
            var individuals = CreateIndividuals(selectionCount * DEFAULT_INDIVIDUALS_COUNT_COEFFICIENT);

            // Act
            var testObj = new LinearRankingIndividualsSelector<ICalculatedIndividual>(DEFAULT_COEFFICIENT,
                RandomProbabilityChecker,
                selectionCount);

            var result = testObj.SelectIndividuals(individuals);

            // Assert
            Assert.AreEqual(selectionCount, result.Count());
        }
    }
}
