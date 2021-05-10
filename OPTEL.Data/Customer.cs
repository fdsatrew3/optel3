using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OPTEL.Data
{
    public class Customer : Core.IDataObject
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
