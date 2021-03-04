using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace GeneticAlgorithm.Base
{
    public class Population<I> : IPopulation<I>
        where I : ICalculatedIndividual
    {
        public ICollection<I> Individuals { get; }

        public Population()
        {
            Individuals = new List<I>();
        }

        public Population(IEnumerable<I> individuals)
        {
            Individuals = individuals.ToList();
        }

        public virtual void AddIndividual(I individual)
        {
            Individuals.Add(individual);
        }

        public virtual void AddIndividuals(IEnumerable<I> individuals)
        {
            var list = individuals.ToList();
            (Individuals as List<I>).AddRange(list);
        }

        public virtual void RemoveIndividual(I individual)
        {
            Individuals.Remove(individual);
        }

        public virtual void RemoveIndividuals(IEnumerable<I> individuals)
        {
            (Individuals as List<I>).RemoveAll(x => individuals.Contains(x));
        }
    }
}
