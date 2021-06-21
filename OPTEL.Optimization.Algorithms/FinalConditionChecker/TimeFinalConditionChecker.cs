using System;
using Optimization.Algorithms;

namespace OPTEL.Optimization.Algorithms.FinalConditionChecker
{
    public class TimeFinalConditionChecker<T> : IFinalConditionChecker<T>
    {
        private const int TICKS_IN_SECOND = 1000;

        private readonly TimeSpan _maxTime;

        private DateTime _startTime;

        public TimeFinalConditionChecker(TimeSpan maxTime)
        {
            _maxTime = maxTime;
        }

        public TimeFinalConditionChecker(int seconds)
        {
            if (seconds <= 0)
                throw new ArgumentOutOfRangeException(nameof(seconds), "Seconds should be greater than 0");

            _maxTime = new TimeSpan(seconds * TICKS_IN_SECOND);
        }

        public void Begin()
        {
            _startTime = DateTime.Now;
        }

        public bool IsStateFinal(T state)
        {
            return DateTime.Now - _startTime >= _maxTime;
        }
    }
}
