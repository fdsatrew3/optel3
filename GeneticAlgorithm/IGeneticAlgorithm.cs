namespace GeneticAlgorithm
{
    public interface IGeneticAlgorithm<I> 
        where I : ICalculatedIndividual
    {
        I GetResolve();
    }
}
