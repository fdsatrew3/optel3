using System;
using System.Collections.Generic;

namespace GeneticAlgorithm.IndividualsSelectors
{
    public class TournamentIndividualsSelector<I> : IndividualsSelector<I>
        where I : ICalculatedIndividual
    {
        public const int MIN_TEAM_CAPACITY = 2;

        protected IBestSelector<I> BestSelector { get; }

        protected int TeamCapacity { get; }

        public TournamentIndividualsSelector(IBestSelector<I> bestSelector, int teamCapacity)
        {
            if (teamCapacity < MIN_TEAM_CAPACITY)
                throw new ArgumentOutOfRangeException(nameof(teamCapacity), "Team capacity should be greater than or equal to 2");

            BestSelector = bestSelector ?? throw new ArgumentNullException(nameof(bestSelector), "Best selector shouldn't be null");
            TeamCapacity = teamCapacity;
        }

        /// <summary>
        /// Select some individuals from collection
        /// </summary>
        /// <param name="individualsCollection">Collection of individuals to select</param>
        /// <returns>Selected individuals</returns>
        protected override IEnumerable<I> SelectIndividualsInternal(ICollection<I> individualsCollection)
        {
            foreach (var team in SelectTeams(individualsCollection))
            {
                var bestIndividual = BestSelector.SelectBestIndividual(team);

                yield return bestIndividual;
            }
        }

        /// <summary>
        /// Selecting teams from collection of individuals
        /// </summary>
        /// <param name="individualsCollection">Collection of individuals to select</param>
        /// <returns>Teams - collections of individuals</returns>
        protected IEnumerable<IEnumerable<I>> SelectTeams(ICollection<I> individualsCollection)
        {
            var enumerator = individualsCollection.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var team = new List<I>();

                for (int i = 0; i < TeamCapacity; i++)
                {
                    team.Add(enumerator.Current);

                    if (!enumerator.MoveNext())
                        break;
                }

                yield return team;
            }
        }
    }
}
