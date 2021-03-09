namespace Optimization.Algorithms.Genetic.Data
{
    public interface ICalculatedIndividual : IIndividual
    {
        double TargetFunctionValue { get; }

        double FitnessFunctionValue { get; }
    }
}
