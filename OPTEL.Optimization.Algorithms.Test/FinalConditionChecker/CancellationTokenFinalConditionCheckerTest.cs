using System;
using System.Threading;
using Xunit;

using OPTEL.Optimization.Algorithms.FinalConditionChecker;

namespace OPTEL.Optimization.Algorithms.Test.FinalConditionChecker
{
    public class CancellationTokenFinalConditionCheckerTest
    {
        [Fact]
        public void Constructor_CancellationTokenSourceIsNull_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CancellationTokenFinalConditionChecker<object>(null));
        }

        [Fact]
        public void IsStateFinal_BeforeCancellationTokenSourceCanceled_ReturnFalse()
        {
            // Arrange
            InitializeCancellationTokenFinalConditionChecker(out CancellationTokenSource cancellationTokenSource,
                out CancellationTokenFinalConditionChecker<object> cancallationTokenFinalConditionChecker);

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
            InitializeCancellationTokenFinalConditionChecker(out CancellationTokenSource cancellationTokenSource, 
                out CancellationTokenFinalConditionChecker<object> cancellationTokenFinalConditionChecker);

            // Act
            cancellationTokenFinalConditionChecker.Begin();
            cancellationTokenSource.Cancel();
            var result = cancellationTokenFinalConditionChecker.IsStateFinal(null);

            //Assert
            Assert.True(result);
        }

        private static void InitializeCancellationTokenFinalConditionChecker(out CancellationTokenSource cancellationTokenSource, 
            out CancellationTokenFinalConditionChecker<object> cancellationTokenFinalConditionChecker)
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenFinalConditionChecker = new CancellationTokenFinalConditionChecker<object>(cancellationTokenSource);
        }
    }
}
