using System;
using System.Threading;
using Xunit;

using OPTEL.Optimization.Algorithms.FinalConditionChecker;

namespace OPTEL.Optimization.Algorithms.Test.FinalConditionChecker
{
    public class CancallationTokenFinalConditionCheckerTest
    {
        [Fact]
        public void Constructor_CancellationTokenSourceIsNull_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CancallationTokenFinalConditionChecker<object>(null));
        }

        [Fact]
        public void IsStateFinal_BeforeCancellationTokenSourceCanceled_ReturnFalse()
        {
            // Arrange
            InitializeCancallationTokenFinalConditionChecker(out CancellationTokenSource cancellationTokenSource,
                out CancallationTokenFinalConditionChecker<object> cancallationTokenFinalConditionChecker);

            // Act
            cancallationTokenFinalConditionChecker.Begin();
            var result = cancallationTokenFinalConditionChecker.IsStateFinal(null);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void IsStateFinal_AfterCancellationTokenSourceCanceled_ReturnTrue()
        {
            // Arrange
            InitializeCancallationTokenFinalConditionChecker(out CancellationTokenSource cancellationTokenSource, 
                out CancallationTokenFinalConditionChecker<object> cancallationTokenFinalConditionChecker);

            // Act
            cancallationTokenFinalConditionChecker.Begin();
            cancellationTokenSource.Cancel();
            var result = cancallationTokenFinalConditionChecker.IsStateFinal(null);

            //Assert
            Assert.True(result);
        }

        private static void InitializeCancallationTokenFinalConditionChecker(out CancellationTokenSource cancellationTokenSource, 
            out CancallationTokenFinalConditionChecker<object> cancallationTokenFinalConditionChecker)
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancallationTokenFinalConditionChecker = new CancallationTokenFinalConditionChecker<object>(cancellationTokenSource);
        }
    }
}
