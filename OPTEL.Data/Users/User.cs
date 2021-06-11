using System.ComponentModel.DataAnnotations;
using OPTEL.Data.Core;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace OPTEL.Data.Users
{
    public class User : IDataObject
    {
        public int ID { get; set; }

        [Required]
        [IndexColumn(IsUnique = true)]
        [StringLength(32, MinimumLength = 4)]
        public string Login { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 3)]
        public string Password { get; set; }
    }
}
