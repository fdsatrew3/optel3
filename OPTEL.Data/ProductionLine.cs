using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using OPTEL.Data.Validation.ValidationEnums;

namespace OPTEL.Data
{
    public class ProductionLine : Core.IDataObject
    {
        public int ID { get; set; }   

        [Required]
        public string Name { get; set; }

        public string Code { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThanOrEqualTo, 0d, ErrorMessage = "Hour cost should be greater than or equal to 0")]
        public double HourCost { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThan, 0d, ErrorMessage = "Max production speed should be greater than 0")]
        public double MaxProductionSpeed { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThanOrEqualTo, 0d, ErrorMessage = "Min width should be greater than or equal to 0")]
        public double WidthMin { get; set; }

        public double WidthMax { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThanOrEqualTo, 0d, ErrorMessage = "Min thickness should be greater than or equal to 0")]
        public double ThicknessMin { get; set; }

        public double ThicknessMax { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThanOrEqualTo, 0d, ErrorMessage = "Min weight should be greater than or equal to 0")]
        public double WeightMin { get; set; }

        public double WeightMax { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThanOrEqualTo, 0d, ErrorMessage = "Min length should be greater than or equal to 0")]
        public double LengthMin { get; set; }

        public double LengthMax { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThanOrEqualTo, 0d, ErrorMessage = "Width change time should be greater than or equal to 0")]
        public double WidthChangeTime { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThanOrEqualTo, 0d, ErrorMessage = "Thickness change time should be greater than or equal to 0")]
        public double ThicknessChangeTime { get; set; }        

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThanOrEqualTo, 0d, ErrorMessage = "Width change consumption should be greater than or equal to 0")]
        public double WidthChangeConsumption { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThanOrEqualTo, 0d, ErrorMessage = "Thickness change consumption should be greater than or equal to 0")]
        public double ThicknessChangeConsumption { get; set; }

        public virtual ICollection<FilmTypesChange> FilmTypesChanges { get; set; }

        public virtual ICollection<NozzleChange> NozzleChanges { get; set; }

        public virtual ICollection<CalibrationChange> CalibrationChanges { get; set; }

        public virtual ICollection<CoolingLipChange> CoolingLipChanges { get; set; }
    }
}
