namespace GeneticAlgorithm
{
    public interface ICalculatedIndividual : IIndividual
    {
        double TargetFunctionValue { get; }

        double FitnessFunctionValue { get; }
    }
}
