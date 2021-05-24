using System;
using System.Threading;
using Optimization.Algorithms;

namespace OPTEL.Optimization.Algorithms.FinalConditionChecker
{
    public class CancallationTokenFinalConditionChecker<T> : IFinalCoditionChecker<T>
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        
        private CancellationToken CancellationToken => _cancellationTokenSource.Token;

        public CancallationTokenFinalConditionChecker(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource ?? throw new ArgumentNullException(nameof(cancellationTokenSource));
        }

        public void Begin()
        {

        }

        public bool IsStateFinal(T state)
        {
            return CancellationToken.IsCancellationRequested;
        }
    }
}
