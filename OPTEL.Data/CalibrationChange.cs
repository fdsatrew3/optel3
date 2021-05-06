﻿using System.ComponentModel.DataAnnotations;

namespace OPTEL.Data
{
    public class CalibrationChange : Core.IDataObject
    {
        public int ID { get; set; }

        public double CalibrationToChange { get; set; }

        public double ReconfigurationTime { get; set; }

        [Required]
        public virtual ProductionLine ParentProductionLine { get; set; }
    }
}
