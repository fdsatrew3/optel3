using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPTEL.Data
{
    public class CalibrationChange : Core.IDataObject
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public decimal CalibrationToChange { get; set; }

        public decimal ReconfigurationTime { get; set; }

        [Required]
        public virtual Extruder ParentExtruder { get; set; }
    }
}
