using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Moq;

using GeneticAlgorithm;
using GeneticAlgorithm.Base;

namespace GeneticAlgorithmTest.Base
{
    public class PopulationSelectorTest
    {
        protected Mock<IIndividualsSelector<ICalculatedIndividual>> IndividualsSelectorMock { get; set; }

        protected IIndividualsSelector<ICalculatedIndividual> IndividualsSelector => IndividualsSelectorMock.Object;

        protected Mock<IPopulation<ICalculatedIndividual>> PopulationMock { get; set; }

        protected IPopulation<ICalculatedIndividual> Population => PopulationMock.Object;

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

        [SetUp]
        public void Setup()
        {
            IndividualsSelectorMock = new Mock<IIndividualsSelector<ICalculatedIndividual>>();
            PopulationMock = new Mock<IPopulation<ICalculatedIndividual>>();
        }

        [Test]
        public void IndividualsSelector_IsNull_ConstructorExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new PopulationSelector<ICalculatedIndividual>(null)
                );
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void CurrentPopulationIndividuals_IsOutOfRange_SelectPopulationExceptionTest(int individualsCount)
        {
            // Arrange
            ICollection<ICalculatedIndividual> individuals;

            if (individualsCount > 0)
                individuals = CreateIndividuals(individualsCount);
            else
                individuals = new List<ICalculatedIndividual>();

            PopulationMock.Setup(x => x.Individuals).Returns(individuals);
            Assert.AreEqual(individuals, Population.Individuals);

            Assert.IsNotNull(Population);
            Assert.IsNotNull(IndividualsSelector);

            // Act
            var testObj = new PopulationSelector<ICalculatedIndividual>(IndividualsSelector);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => testObj.SelectPopulation(Population)
                );
        }
    }
}
