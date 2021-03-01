using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using OPTEL.Data.Validation.ValidationEnums;

namespace OPTEL.Data
{
    public class FilmRecipe : Core.IDataObject
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public virtual FilmType FilmType { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThan, 0, ErrorMessage = "Thickness should be greater than 0")]
        public decimal Thickness { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThan, 0, ErrorMessage = "Production speed should be greater than 0")]
        public decimal ProductionSpeed { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThan, 0, ErrorMessage = "Material cost should be greater than 0")]
        public decimal MaterialCost { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThan, 0, ErrorMessage = "Nozzle should be greater than 0")]
        public decimal Nozzle { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThan, 0, ErrorMessage = "Calibration should be greater than 0")]
        public decimal Calibration { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThan, 0, ErrorMessage = "Cooling lip should be greater than 0")]
        public decimal CoolingLip { get; set; }
    }
}
