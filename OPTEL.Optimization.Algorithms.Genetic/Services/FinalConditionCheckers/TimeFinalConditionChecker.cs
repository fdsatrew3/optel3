using System;
using Optimization.Algorithms.Genetic.Core;
using Optimization.Algorithms.Genetic.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.FinalConditionCheckers
{
    public class TimeFinalConditionChecker<I> : IFinalCoditionChecker<I>
        where I : ICalculatedIndividual
    {
        private const int TICKS_IN_SECOND = 1000;

        protected TimeSpan MaxTime { get; }

        private DateTime _startTime;

        public TimeFinalConditionChecker(TimeSpan maxTime)
        {
            MaxTime = maxTime;
        }

        public TimeFinalConditionChecker(int seconds)
        {
            if (seconds <= 0)
                throw new ArgumentOutOfRangeException(nameof(seconds), "Seconds should be greater than 0");

            MaxTime = new TimeSpan(seconds * TICKS_IN_SECOND);
        }

        /// <summary>
        /// Set start settings to checker
        /// </summary>
        public void Begin()
        {
            _startTime = DateTime.Now;
        }

        /// <summary>
        /// Check if thet population is final
        /// </summary>
        /// <param name="currentPopulation">Current population</param>
        /// <returns>If that is final operation</returns>
        public bool IsPopulationIsFinal(IPopulation<I> currentPopulation)
        {
            return DateTime.Now - _startTime >= MaxTime;
        }
    }
}
