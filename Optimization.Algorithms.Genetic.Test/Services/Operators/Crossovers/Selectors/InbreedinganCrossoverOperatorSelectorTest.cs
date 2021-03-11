﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Optimization.Algorithms.Genetic.Core;
using Optimization.Algorithms.Genetic.Data;
using Optimization.Algorithms.Genetic.Services.Operators.Crossovers.Selectors;
using Xunit;

namespace Optimization.Algorithms.Genetic.Services.Test.Operators.Crossovers.Selectors
{
    public class InbreedinganCrossoverOperatorSelectorTest
    {
        private readonly Mock<IIndividualsSelector<ICalculatedIndividual>> _individualsSelectorMock;
        private readonly Mock<IPopulation<ICalculatedIndividual>> _populationMock;

        private IIndividualsSelector<ICalculatedIndividual> IndividualsSelector => _individualsSelectorMock.Object;

        private IPopulation<ICalculatedIndividual> Population => _populationMock.Object;

        public InbreedinganCrossoverOperatorSelectorTest()
        {
            _individualsSelectorMock = new Mock<IIndividualsSelector<ICalculatedIndividual>>();
            _populationMock = new Mock<IPopulation<ICalculatedIndividual>>();
        }

        [Fact]
        public void IndividualsSelector_IsNull_ConstructorExceptionTest()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new InbreedinganCrossoverOperatorSelector<ICalculatedIndividual>(null)
                );
        }

        [Theory]
        [InlineData(6)]
        [InlineData(16)]
        [InlineData(64)]
        public void ReturnSettedCountOfIndividuals_SelectIndividualsTest(int individualsCount)
        {
            // Arrange
            var individuals = CreateIndividuals(individualsCount);

            _individualsSelectorMock.Setup(x => x.SelectIndividuals(It.IsAny<ICollection<ICalculatedIndividual>>())).Returns(individuals);
            _populationMock.Setup(x => x.Individuals).Returns(individuals);

            // Act
            var testObject = new InbreedinganCrossoverOperatorSelector<ICalculatedIndividual>(IndividualsSelector);
            var result = testObject.SelectParents(Population);

            // Assert
            Assert.Equal(individualsCount, result.Count());

            _individualsSelectorMock.Verify(x => x.SelectIndividuals(It.IsAny<ICollection<ICalculatedIndividual>>()));
            _populationMock.Verify(x => x.Individuals);
        }

        private ICollection<ICalculatedIndividual> CreateIndividuals(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count should be greater than 0");

            var individuals = new ICalculatedIndividual[count];

            for (int i = 0; i < individuals.Length; i++)
            {
                individuals[i] = Mock.Of<ICalculatedIndividual>();
            }

            return individuals.ToList();
        }
    }
}
