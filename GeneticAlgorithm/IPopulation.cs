using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public interface IPopulation<I>
        where I : IIndividual
    {
        ICollection<I> Individuals { get; }

        void AddIndividual(I individual);

        void AddIndividuals(IEnumerable<I> individuals);

        void RemoveIndividual(I individual);

        void RemoveIndividuals(IEnumerable<I> individuals);
    }
}
