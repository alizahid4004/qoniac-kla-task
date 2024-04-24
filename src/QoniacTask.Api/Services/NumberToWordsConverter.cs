using QoniacTask.Api.Models;
using System;
using System.Globalization;

namespace QoniacTask.Server.Services
{
    internal static class NumberToWordsConverter
    {
        private static readonly NumberToWordsOptions Default = new();

        private static readonly string[] UnitsAndTeens =
            ["zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"];

        private static readonly string[] Tens =
            ["zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"];

        //NOTE: add more of these to support higher number
        private static readonly ValueDescription Hundred = new(100, "hundred");

        private static readonly ValueDescription Thousand = new(1_000, "thousand");
        private static readonly ValueDescription Million = new(1_000_000, "million");
        private static readonly ValueDescription Billion = new(1_000_000_000, "billion");

        //This API design is inspired by System.Text.Json.JsonSerializer.Serialize :)
        public static string Convert(decimal number, NumberToWordsOptions? options = null)
        {
            options ??= Default;

            //NOTE: I just want to break the number into integer and decimal part, so I want the
            //ToString to always format the string in the same way regardless of the culture.
            var numberParts = number.ToString(CultureInfo.InvariantCulture).Split('.');
            var integer = long.Parse(numberParts[0]);

            var integerDescription = ConvertInternal(integer, false, options);

            var hasDecimal = numberParts.Length > 1;

            if (hasDecimal)
            {
                var @decimal = long.Parse(numberParts[1]);
                var decimalDescription = ConvertInternal(@decimal, true, options);

                var integerDecimalSeparator = string.IsNullOrWhiteSpace(options.IntegerDecimalSeparationText) ?
                    " " : $" {options.IntegerDecimalSeparationText.Trim()} ";

                return $"{integerDescription}{integerDecimalSeparator}{decimalDescription}";
            }

            return integerDescription;
        }

        public static string Convert(Currency currency, NumberToWordsOptions? options = null)
        {
            options ??= Default;
            var integerDescription = ConvertInternal(currency.Whole, false, options);

            if (currency.Change.HasValue)
            {
                var decimalDescription = ConvertInternal(currency.Change.Value, true, options);

                var integerDecimalSeparator = string.IsNullOrWhiteSpace(options.IntegerDecimalSeparationText) ?
                        " " : $" {options.IntegerDecimalSeparationText.Trim()} ";

                return $"{integerDescription}{integerDecimalSeparator}{decimalDescription}";
            }

            return integerDescription;
        }

        private static string ConvertInternal(
            long number, bool isDecimal, NumberToWordsOptions options)
        {
            var unit = isDecimal ? options.DecimalUnit : options.IntegerUnit;
            var unitForOne = isDecimal ? options.DecimalUnitForOne : options.IntegerUnitForOne;

            if (number == 0)
            {
                return string.IsNullOrWhiteSpace(unit) ?
                    UnitsAndTeens[number] : $"{UnitsAndTeens[number]} {unit}";
            }

            if (number == 1)
            {
                return string.IsNullOrWhiteSpace(unitForOne) ?
                    UnitsAndTeens[number] : $"{UnitsAndTeens[number]} {unitForOne}";
            }

            //NOTE: Not relevant for this task, but negative numbers are also supported :)
            if (number < 0)
            {
                return string.IsNullOrWhiteSpace(options.NegativeText) ?
                    ConvertInternal(-number, isDecimal, options) : $"{options.NegativeText} {ConvertInternal(-number, isDecimal, options)}";
            }

            var parts = new List<string>(20);

            AppendParts(parts, ref number, Billion.Value, Billion.Text, options);
            AppendParts(parts, ref number, Million.Value, Million.Text, options);
            AppendParts(parts, ref number, Thousand.Value, Thousand.Text, options);

            AppendPartsUnderThousand(parts, number, options);

            if (!string.IsNullOrWhiteSpace(unit))
            {
                parts.Add(unit);
            }

            return string.Join(' ', parts);
        }

        private static void AppendParts(
            List<string> parts, ref long number, long divisor, string word,
            NumberToWordsOptions options)
        {
            var result = number / divisor;
            if (result == 0)
            {
                return;
            }

            AppendPartsUnderThousand(parts, result, options);

            number %= divisor;
            parts.Add(word);
        }

        private static void AppendPartsUnderThousand(
            List<string> parts, long number,
            NumberToWordsOptions options)
        {
            if (number >= 100)
            {
                parts.Add(UnitsAndTeens[number / 100]);
                number %= 100;
                parts.Add(Hundred.Text);
            }

            if (number == 0)
            {
                return;
            }

            if (parts.Count > 0 && options.AddAnd)
            {
                parts.Add("and");
            }

            if (number >= 20)
            {
                var tens = Tens[number / 10];
                var units = number % 10;
                if (units == 0)
                {
                    parts.Add(tens);
                }
                else
                {
                    var tensText = options.HyphenateTens ?
                        $"{tens}-{UnitsAndTeens[units]}" : $"{tens} {UnitsAndTeens[units]}";

                    parts.Add(tensText);
                }
            }
            else
            {
                parts.Add(UnitsAndTeens[number]);
            }
        }

        private readonly record struct ValueDescription(long Value, string Text);
    }
}