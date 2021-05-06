using System.ComponentModel.DataAnnotations;

using OPTEL.Data.Validation.ValidationEnums;

namespace OPTEL.Data
{
    public class FilmRecipe : Core.IDataObject
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public virtual FilmType FilmType { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThan, 0d, ErrorMessage = "Thickness should be greater than 0")]
        public double Thickness { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThan, 0d, ErrorMessage = "Production speed should be greater than 0")]
        public double ProductionSpeed { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThan, 0d, ErrorMessage = "Material cost should be greater than 0")]
        public double MaterialCost { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThan, 0d, ErrorMessage = "Nozzle should be greater than 0")]
        public double Nozzle { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThan, 0d, ErrorMessage = "Calibration should be greater than 0")]
        public double Calibration { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThan, 0d, ErrorMessage = "Cooling lip should be greater than 0")]
        public double CoolingLip { get; set; }
    }
}
