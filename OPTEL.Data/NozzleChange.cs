using System;
using System.ComponentModel.DataAnnotations;

namespace OPTEL.Data
{
    public class NozzleChange : Core.IDataObject
    {
        public int ID { get; set; }

        public decimal NozzleToChange { get; set; }

        public decimal ReconfigurationTime { get; set; }

        [Required]
        public virtual ProductionLine ParentProductionLine { get; set; }
    }
}
