using System;
using Xunit;
using OPTEL.Optimization.Algorithms.FinalConditionChecker;

namespace OPTEL.Optimization.Algorithms.Test.FinalConditionChecker
{
    public class TimeFinalConditionCheckerTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Constructor_SecondsIsLesserThan1_THrowsArgumentOutOfRangeException(int seconds)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new TimeFinalConditionChecker<object>(seconds));
        }
    }
}
