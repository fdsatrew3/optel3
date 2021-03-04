namespace GeneticAlgorithm
{
    public interface IGeneticAlgorythmSettings
    {
        int OperationsCount { get; }

        int IndividualsCount { get; }

        int MutableIndividualsCount { get; }

        double CrossoverProbability { get; }

        double MutationProbability { get; }
    }
}
