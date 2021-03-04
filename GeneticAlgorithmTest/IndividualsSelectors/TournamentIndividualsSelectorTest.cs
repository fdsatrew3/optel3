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
    public class TournamentIndividualsSelectorTest : IndividualSelectorTestClass
    {
        protected int DEFAULT_TEAM_CAPACITY = TournamentIndividualsSelector<ICalculatedIndividual>.MIN_TEAM_CAPACITY;

        protected Mock<IBestSelector<ICalculatedIndividual>> BestSelectorMock { get; set; }

        protected IBestSelector<ICalculatedIndividual> BestSelector => BestSelectorMock.Object;

        [SetUp]
        public void Setup()
        {
            BestSelectorMock = new Mock<IBestSelector<ICalculatedIndividual>>();
        }

        [Test]
        [TestCase(0)]
        [TestCase(-6)]
        [TestCase(int.MinValue)]
        public void TeamCapacity_IsOutOfRange_ConstructorExceptionTest(int teamCapacity)
        {
            Assert.IsNotNull(BestSelector);

            Assert.Throws<ArgumentOutOfRangeException>(
                () => new TournamentIndividualsSelector<ICalculatedIndividual>(BestSelector, teamCapacity)
                );
        }

        [Test]
        public void BestSelector_IsNull_ConstructorExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new TournamentIndividualsSelector<ICalculatedIndividual>(null, DEFAULT_TEAM_CAPACITY)
                );
        }

        [Test]
        [TestCase(6)]
        [TestCase(16)]
        [TestCase(64)]
        public void ReturnBestIndividual_FromOneTeam_SelectPopulationTest(int teamCapacity)
        {
            // Arrange
            var individuals = CreateIndividuals(teamCapacity);
            var bestIndividual = individuals.First();

            BestSelectorMock.Setup(x => x.SelectBestIndividual(It.IsAny<IEnumerable<ICalculatedIndividual>>())).Returns(bestIndividual);
            Assert.AreEqual(bestIndividual, BestSelector.SelectBestIndividual(individuals));

            // Act
            var testObject = new TournamentIndividualsSelector<ICalculatedIndividual>(BestSelector, teamCapacity);
            var result = testObject.SelectIndividuals(individuals);

            // Assert
            BestSelectorMock.Verify(x => x.SelectBestIndividual(It.IsAny<IEnumerable<ICalculatedIndividual>>()));

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(bestIndividual, result.First());
        }
    }
}
