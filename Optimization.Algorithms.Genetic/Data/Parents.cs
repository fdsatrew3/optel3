namespace Optimization.Algorithms.Genetic.Data
{
    public struct Parents<I>
        where I : ICalculatedIndividual
    {
        public I FirstParent { get; set; }

        public I SecondParent { get; set; }
    }
}
