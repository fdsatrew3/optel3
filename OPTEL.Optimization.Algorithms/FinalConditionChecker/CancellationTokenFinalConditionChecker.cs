using System;
using System.Threading;
using Optimization.Algorithms;

namespace OPTEL.Optimization.Algorithms.FinalConditionChecker
{
    public class CancellationTokenFinalConditionChecker<T> : IFinalConditionChecker<T>
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        
        private CancellationToken CancellationToken => _cancellationTokenSource.Token;

        public CancellationTokenFinalConditionChecker(CancellationTokenSource cancellationTokenSource)
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
