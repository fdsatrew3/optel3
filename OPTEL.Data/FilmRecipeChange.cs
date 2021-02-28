using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OPTEL.Data
{
    public class FilmRecipeChange : Core.IDataObject
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required]
        public virtual FilmRecipe FilmRecipeFrom { get; set; }

        [Required]
        public virtual FilmRecipe FilmRecipeTo { get; set; }

        public decimal ReconfigurationTime { get; set; }

        [Required]
        public virtual Extruder ParentExtruder { get; set; }
    }
}
