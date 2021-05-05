using System.ComponentModel.DataAnnotations;

namespace OPTEL.Data
{
    public class FilmType : Core.IDataObject
    {
        public int ID { get; set; }

        [Required]
        public string Article { get; set; }
    }
}
