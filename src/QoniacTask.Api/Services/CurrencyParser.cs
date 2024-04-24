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

            options ??= Default;

            if (options.DecimalSeparator == options.GroupSeparator)
            {
                throw new InvalidOperationException("Decimal separator and group separator's can't be the same");
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
    }
}