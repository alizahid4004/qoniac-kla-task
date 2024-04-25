using QoniacTask.Api.Models;

namespace QoniacTask.Api.Services
{
    internal static class CurrencyParser
    {
        private static readonly CurrencyParserOptions Default = new();

        public static Currency Parse(
            string formattedAmount,
            CurrencyParserOptions options = null)
        {
            if (string.IsNullOrWhiteSpace(formattedAmount))
            {
                throw new ArgumentException($"'{nameof(formattedAmount)}' cannot be null or whitespace.", nameof(formattedAmount));
            }

            if (options is not null)
            {
                ValidateOptions(options);
            }
            else
            {
                options = Default;
            }

            var parts = formattedAmount.Split(options.DecimalSeparator);

            if (parts.Length > 2)
            {
                throw new FormatException("Decimal separator can't appear more than once");
            }

            //NOTE: decimal.TryParse can be used for this if CurrencyParserOptions includes a CultureInfo property
            //decimal.TryParse(formattedAmount, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, options.CultureInfo, out var amount)
            var wholePartValid = InputContainsNumbers(parts[0], options.GroupSeparator);

            if (!wholePartValid)
            {
                throw new FormatException("Number string contains invalid characters");
            }

            var wholeString = parts[0].Replace(options.GroupSeparator.ToString(), string.Empty);
            var whole = long.Parse(wholeString);

            if (options.WholeMinimum.HasValue && whole < options.WholeMinimum)
            {
                throw new FormatException($"Provided number is below the allowed range of {options.WholeMinimum}");
            }

            if (options.WholeMaximum.HasValue && whole > options.WholeMaximum)
            {
                throw new FormatException($"Provided number is above the allowed range of {options.WholeMaximum}");
            }

            var hasChange = parts.Length == 2;

            if (hasChange)
            {
                var changeString = parts[1];

                if (changeString.Length == 1)
                {
                    changeString += '0';
                }

                var changeValid = uint.TryParse(changeString, out var change);

                if (!changeValid)
                {
                    throw new FormatException("Number string contains invalid characters");
                }

                return new Currency(whole, change);
            }

            return new Currency(whole, null);
        }

        private static bool InputContainsNumbers(string input, char separator)
        {
            return input.Split(separator).All(x => int.TryParse(x, out _));
        }

        private static void ValidateOptions(CurrencyParserOptions options)
        {
            if (options.DecimalSeparator == options.GroupSeparator)
            {
                throw new InvalidOperationException("Decimal separator and group separator's can't be the same");
            }

            if (options.WholeMinimum.HasValue &&
                options.WholeMaximum.HasValue &&
                options.WholeMinimum >= options.WholeMaximum)
            {
                throw new InvalidOperationException("Minimum whole value can't be greater than or equal to the maximum whole");
            }
        }
    }
}