using OPTEL.Data;

namespace OPTEL.Optimization.Algorithms.Genetic.Services.Util.Data
{
    public struct OrderPosition
    {
        public Extruder ProductionLine { get; set; }

        public Order Order { get; set; }

        public int Position { get; set; }
    }
}
