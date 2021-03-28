using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using OPTEL.Data.Validation.ValidationEnums;

namespace OPTEL.Data
{
    public class ProductionLine : Core.IDataObject
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }   

        [Required]
        public string Name { get; set; }

        public string Code { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThanOrEqualTo, 0, ErrorMessage = "Hour cost should be greater than or equal to 0")]
        public decimal HourCost { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThan, 0, ErrorMessage = "Max production speed should be greater than 0")]
        public decimal MaxProductionSpeed { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThanOrEqualTo, 0, ErrorMessage = "Min width should be greater than or equal to 0")]
        public decimal WidthMin { get; set; }

        public decimal WidthMax { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThanOrEqualTo, 0, ErrorMessage = "Min thickness should be greater than or equal to 0")]
        public decimal ThicknessMin { get; set; }

        public decimal ThicknessMax { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThanOrEqualTo, 0, ErrorMessage = "Min weight should be greater than or equal to 0")]
        public decimal WeightMin { get; set; }

        public decimal WeightMax { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThanOrEqualTo, 0, ErrorMessage = "Min length should be greater than or equal to 0")]
        public decimal LengthMin { get; set; }

        public decimal LengthMax { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThanOrEqualTo, 0, ErrorMessage = "Width change time should be greater than or equal to 0")]
        public decimal WidthChangeTime { get; set; }

        [Validation.ValidationAttributes.Compare(ComparisonType.IsGreaterThanOrEqualTo, 0, ErrorMessage = "Thickness change time should be greater than or equal to 0")]
        public decimal ThicknessChangeTime { get; set; }

        public virtual ICollection<FilmTypesChange> FilmTypesChanges { get; set; }

        public virtual ICollection<NozzleChange> NozzleChanges { get; set; }

        public virtual ICollection<CalibrationChange> CalibrationChanges { get; set; }

        public virtual ICollection<CoolingLipChange> CoolingLipChanges { get; set; }
    }
}
