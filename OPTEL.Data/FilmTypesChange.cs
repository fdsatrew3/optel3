using System.ComponentModel.DataAnnotations;

namespace OPTEL.Data
{
    public class FilmTypesChange : Core.IDataObject
    {
        public int ID { get; set; }

        [Required]
        public virtual FilmType FilmTypeFrom { get; set; }

        [Required]
        public virtual FilmType FilmTypeTo { get; set; }

        public double ReconfigurationTime { get; set; }

        [Required]
        public virtual ProductionLine ParentProductionLine { get; set; }
    }
}
