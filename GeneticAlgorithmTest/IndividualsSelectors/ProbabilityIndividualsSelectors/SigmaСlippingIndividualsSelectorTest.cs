using System;
using System.Linq;
using NUnit.Framework;

using Optimization.Algorithms.Utilities.Probabilities;
using GeneticAlgorithm;
using GeneticAlgorithm.IndividualsSelectors.ProbabilityIndividualsSelectors;
using GeneticAlgorithmTest.Core;

namespace GeneticAlgorithmTest.IndividualsSelectors.ProbabilityIndividualsSelectors
{
    public class SigmaСlippingIndividualsSelectorTest : IndividualSelectorTestClass
    {
        protected const int DEFAULT_SELECTION_COUNT = 2;

        protected Random Random { get; }

        protected RandomProbabilityChecker RandomProbabilityChecker { get; }

        public SigmaСlippingIndividualsSelectorTest()
        {
            Random = new Random();
            RandomProbabilityChecker = new RandomProbabilityChecker(Random);
        }

        [Test]
        public void RandomProbabilityChecker_IsNull_ConstructorExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new SigmaСlippingIndividualsSelector<ICalculatedIndividual>(null,
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
                () => new SigmaСlippingIndividualsSelector<ICalculatedIndividual>(RandomProbabilityChecker,
                    selectionCount)
                );
        }

        [Test]
        [TestCase(DEFAULT_SELECTION_COUNT)]
        [TestCase(16)]
        [TestCase(32)]
        [TestCase(64)]
        public void ReturnInividualsCountEqualToSelectionCount_SelectIndividualsTest(int selectionCount)
        {
            // Arrange
            var individuals = CreateIndividuals(selectionCount);

            // Act
            var testObj = new SigmaСlippingIndividualsSelector<ICalculatedIndividual>(RandomProbabilityChecker,
                selectionCount);

            var result = testObj.SelectIndividuals(individuals);

            // Assert
            Assert.AreEqual(selectionCount, result.Count());
        }
    }
}
