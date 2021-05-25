using System;
using Xunit;
using OPTEL.Optimization.Algorithms.FinalConditionChecker;

namespace OPTEL.Optimization.Algorithms.Test.FinalConditionChecker
{
    public class IterationsFinalConditionCheckerTest
    {        
        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Constructor_IterrationsCountIsLesserThan1_ThrowsArgumentOutOfRangeException(int iterationsCount)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new IterationsFinalConditionChecker<object>(iterationsCount));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        public void IsStateFinal_ForNonLastIteration_ReturnFalse(int iterationsCount)
        {
            // Arrange
            var iterationsFinalConditionChecker = new IterationsFinalConditionChecker<object>(iterationsCount);

            // Act & Assert
            iterationsFinalConditionChecker.Begin();

            for(int i = 0; i < iterationsCount - 1; i++)
            {
                Assert.False(iterationsFinalConditionChecker.IsStateFinal(null));
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        public void IsStateFinal_ForLastIteration_ReturnTrue(int iterationsCount)
        {
            // Arrange
            var iterationsFinalConditionChecker = new IterationsFinalConditionChecker<object>(iterationsCount);

            // Act & Assert
            iterationsFinalConditionChecker.Begin();

            for(int i = 0; i < iterationsCount - 1; i++)
            {
                iterationsFinalConditionChecker.IsStateFinal(null);
            }

            Assert.True(iterationsFinalConditionChecker.IsStateFinal(null));
        }
    }
}
