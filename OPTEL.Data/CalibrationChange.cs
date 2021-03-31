using System.ComponentModel.DataAnnotations;

namespace OPTEL.Data
{
    public class CalibrationChange : Core.IDataObject
    {
        public int ID { get; set; }

        public decimal CalibrationToChange { get; set; }

        public decimal ReconfigurationTime { get; set; }

        [Required]
        public virtual ProductionLine ParentProductionLine { get; set; }
    }
}
