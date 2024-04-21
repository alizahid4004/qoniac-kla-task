using System.ComponentModel.DataAnnotations;

namespace QoniacTask.Api.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class DecimalRangeValidationAttribute : ValidationAttribute
    {
        public DecimalRangeValidationAttribute(
            long integerMinimum, long integerMaximum,
            ulong decimalMinimum, ulong decimalMaximum,
            bool decimalRequired = false)
                : base($"The {0} field is a required decimal with the integer part in the range {integerMinimum} to {integerMaximum} and the decimal in the range {decimalMinimum} to {decimalMaximum}")
        {
            if (integerMinimum >= integerMaximum)
            {
                throw new ArgumentException("Minimum integer value can't be greater than or equal to the maximum value", nameof(integerMinimum));
            }

            if (decimalMinimum >= decimalMaximum)
            {
                throw new ArgumentException("Minimum integer value can't be greater than or equal to the maximum value", nameof(decimalMinimum));
            }

            IntegerMinimum = integerMinimum;
            IntegerMaximum = integerMaximum;
            DecimalMinimum = decimalMinimum;
            DecimalMaximum = decimalMaximum;
            DecimalRequired = decimalRequired;
        }

        public long IntegerMinimum { get; }
        public long IntegerMaximum { get; }
        public ulong DecimalMinimum { get; }
        public ulong DecimalMaximum { get; }
        public bool DecimalRequired { get; }

        public override bool IsValid(object? value)
        {
            if (value is null)
            {
                //to only return the required error
                return true;
            }

            if (value is not double number)
            {
                return false;
            }

            var numberParts = number.ToString().Split('.');
            var integer = long.Parse(numberParts[0]);

            if (integer < IntegerMinimum || integer > IntegerMaximum)
            {
                return false;
            }

            var hasDecimal = numberParts.Length > 1;

            if (DecimalRequired && !hasDecimal)
            {
                ErrorMessage = "The {0} field is missing the required decimal part.";
                return false;
            }

            var @decimal = ulong.Parse(numberParts[1]);

            if (@decimal < DecimalMinimum || @decimal > DecimalMaximum)
            {
                return false;
            }

            return true;
        }
    }
}