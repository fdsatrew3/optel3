using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Moq;

using GeneticAlgorithm;
using GeneticAlgorithm.Operators.Crossovers.Selectors;

namespace GeneticAlgorithmTest.Operators.Crossovers.Selectors
{
    public class InbreedinganCrossoverOperatorSelectorTest
    {
        protected Mock<IIndividualsSelector<ICalculatedIndividual>> IndividualsSelectorMock { get; set; }

        protected IIndividualsSelector<ICalculatedIndividual> IndividualsSelector => IndividualsSelectorMock.Object;

        protected Mock<IPopulation<ICalculatedIndividual>> PopulationMock { get; set; }

        protected IPopulation<ICalculatedIndividual> Population => PopulationMock.Object;

        [SetUp]
        public void Setup()
        {
            IndividualsSelectorMock = new Mock<IIndividualsSelector<ICalculatedIndividual>>();
            PopulationMock = new Mock<IPopulation<ICalculatedIndividual>>();
        }

        protected ICollection<ICalculatedIndividual> CreateIndividuals(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count should be greater than 0");

            ICalculatedIndividual[] individuals = new ICalculatedIndividual[count];

            for (int i = 0; i < individuals.Length; i++)
            {
                individuals[i] = Mock.Of<ICalculatedIndividual>();
            }

            return individuals.ToList();
        }

        [Test]
        public void IndividualsSelector_IsNull_ConstructorExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new InbreedinganCrossoverOperatorSelector<ICalculatedIndividual>(null)
                );
        }

        [Test]
        [TestCase(6)]
        [TestCase(16)]
        [TestCase(64)]
        public void ReturnSettedCountOfIndividuals_SelectIndividualsTest(int individualsCount)
        {
            // Arrange
            Assert.IsNotNull(IndividualsSelector);

            var individuals = CreateIndividuals(individualsCount);

            IndividualsSelectorMock.Setup(x => x.SelectIndividuals(It.IsAny<ICollection<ICalculatedIndividual>>())).Returns(individuals);
            Assert.AreEqual(individuals, IndividualsSelector.SelectIndividuals(individuals));

            PopulationMock.Setup(x => x.Individuals).Returns(individuals);
            Assert.AreEqual(individuals, Population.Individuals);

            // Act
            var testObject = new InbreedinganCrossoverOperatorSelector<ICalculatedIndividual>(IndividualsSelector);
            var result = testObject.SelectParents(Population);

            // Assert
            IndividualsSelectorMock.Verify(x => x.SelectIndividuals(It.IsAny<ICollection<ICalculatedIndividual>>()));
            PopulationMock.Verify(x => x.Individuals);

            Assert.AreEqual(individualsCount, result.Count());
        }
    }
}
