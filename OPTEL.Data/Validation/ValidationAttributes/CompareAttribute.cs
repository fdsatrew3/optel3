using System;
using System.ComponentModel.DataAnnotations;

using OPTEL.Data.Validation.ValidationEnums;

namespace OPTEL.Data.Validation.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class CompareAttribute : ValidationAttribute
    {
        private readonly ComparisonType _comparisonType;
        public readonly object OtherValue;

        public CompareAttribute(ComparisonType comparison, object otherValue)
        {
            _comparisonType = comparison;
            OtherValue = otherValue;
        }

        #region Overrides of ValidationAttribute

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (OtherValue.GetType() != value.GetType())
                throw new ArgumentException("Members should be of one type");
            if (!(OtherValue is IComparable otherValueComparable) || !(value is IComparable valueComparable))
                throw new ArgumentException("One or both of the members don't implement IComparable");

            switch (_comparisonType)
            {
                case ComparisonType.IsEqualTo when valueComparable.CompareTo(otherValueComparable) == 0:
                    return ValidationResult.Success;
                case ComparisonType.IsGreaterThan when valueComparable.CompareTo(otherValueComparable) > 0:
                    return ValidationResult.Success;
                case ComparisonType.IsGreaterThanOrEqualTo when valueComparable.CompareTo(otherValueComparable) >= 0:
                    return ValidationResult.Success;
                case ComparisonType.IsLessThan when valueComparable.CompareTo(otherValueComparable) < 0:
                    return ValidationResult.Success;
                case ComparisonType.IsLessThanOrEqualTo when valueComparable.CompareTo(otherValueComparable) <= 0:
                    return ValidationResult.Success;
                default:
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
        }

        //Нужно для того, чтобы несколько атрибутов одного типа работали на одном свойстве
        private readonly object _typeId = new object();
        public override object TypeId => _typeId;

        #endregion
    }
}
