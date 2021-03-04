namespace GeneticAlgorithm.Operators.Crossovers
{
    public struct Parents<I>
        where I : ICalculatedIndividual
    {
        public I FirstParent { get; set; }

        public I SecondParent { get; set; }
    }
}
