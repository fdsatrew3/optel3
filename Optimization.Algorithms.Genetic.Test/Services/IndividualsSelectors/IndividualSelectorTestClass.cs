using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Optimization.Algorithms.Genetic.Data;
using Xunit;

namespace Optimization.Algorithms.Genetic.Services.Test.IndividualsSelectors
{
    public class IndividualSelectorTestClass
    {
        /// <summary>
        /// Create collection of individuals
        /// </summary>
        /// <param name="count">Count of individuals</param>
        /// <returns>Collection of individuals</returns>
        protected ICollection<ICalculatedIndividual> CreateIndividuals(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count should be greater than 0");

            var individuals = new ICalculatedIndividual[count];

            for (int i = 0; i < individuals.Length; i++)
            {
                var mock = new Mock<ICalculatedIndividual>();
                mock.Setup(x => x.TargetFunctionValue).Returns(i * 10);
                mock.Setup(x => x.FitnessFunctionValue).Returns(i);

                individuals[i] = mock.Object;
            }

            for (int i = 0; i < individuals.Length; i++)
            {
                Assert.Equal(i * 10, individuals[i].TargetFunctionValue);
                Assert.Equal(i, individuals[i].FitnessFunctionValue);
            }

            return individuals.ToList();
        }
    }
}
