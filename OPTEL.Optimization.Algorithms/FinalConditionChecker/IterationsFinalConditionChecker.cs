using System;
using Optimization.Algorithms;

namespace OPTEL.Optimization.Algorithms.FinalConditionChecker
{
    public class IterationsFinalConditionChecker<T> : IFinalCoditionChecker<T>
    {
        private readonly int _iterationsCount;

        private int _currentIteration;

        public IterationsFinalConditionChecker(int iterationsCount)
        {
            if (iterationsCount < 1)
                throw new ArgumentOutOfRangeException(nameof(iterationsCount), "Iterations count should be greater than 1");

            _iterationsCount = iterationsCount;
        }

        public void Begin()
        {
            _currentIteration = 0;
        }

        public bool IsStateFinal(T state)
        {
            _currentIteration++;

            return _currentIteration > _iterationsCount;
        }
    }
}
