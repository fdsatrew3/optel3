using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPTEL.Data
{
    public class FilmTypesChange : Core.IDataObject
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required]
        public virtual FilmType FilmTypeFrom { get; set; }

        [Required]
        public virtual FilmType FilmTypeTo { get; set; }

        public decimal ReconfigurationTime { get; set; }

        [Required]
        public virtual Extruder ParentExtruder { get; set; }
    }
}
