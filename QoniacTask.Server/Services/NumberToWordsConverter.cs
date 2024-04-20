namespace QoniacTask.Server.Services
{
    internal static class NumberToWordsConverter
    {
        private static readonly NumberToWordsOptions Default = new();

        private static readonly string[] Units =
            ["zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"];

        private static readonly string[] Tens =
            ["zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"];

        //NOTE: add more of these to support higher number
        private static readonly ValueDescription Hundred = new(100, "hundred");
        private static readonly ValueDescription Thousand = new(1_000, "thousand");
        private static readonly ValueDescription Million = new(1_000_000, "million");
        private static readonly ValueDescription Billion = new(1_000_000_000, "billion");

        //todo: support decimals
        public static string Convert(long number, NumberToWordsOptions? options = null)
        {
            options ??= Default;

            if (number == 0)
            {
                return string.IsNullOrWhiteSpace(options.IntegerUnit) ?
                    Units[number] : $"{Units[number]} {options.IntegerUnit}";
            }

            if (number == 1)
            {
                return string.IsNullOrWhiteSpace(options.IntegerUnitForOne) ?
                    Units[number] : $"{Units[number]} {options.IntegerUnitForOne}";
            }

            //NOTE: Not relevant for this task, but negative numbers are also supported :)
            if (number < 0)
            {
                return string.IsNullOrWhiteSpace(options.NegativeText) ?
                    Convert(-number, options) : $"{options.NegativeText} {Convert(-number, options)}";
            }

            var parts = new List<string>(20);

            AppendParts(parts, ref number, Billion.Value, Million.Text, options);
            AppendParts(parts, ref number, Million.Value, Million.Text, options);
            AppendParts(parts, ref number, Thousand.Value, Thousand.Text, options);

            AppendPartsUnderThousand(parts, number, options);

            if (!string.IsNullOrWhiteSpace(options.IntegerUnit))
            {
                parts.Add(options.IntegerUnit);
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

        //todo: consider moving the separating word to a settings object (like system.text.json)
        private static void AppendPartsUnderThousand(
            List<string> parts, long number,
            NumberToWordsOptions options)
        {
            if (number >= 100)
            {
                parts.Add(Units[number / 100]);
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
                        $"{tens}-{Units[units]}" : $"{tens} {Units[units]}";

                    parts.Add(tensText);
                }
            }
            else
            {
                parts.Add(Units[number]);
            }
        }

        private readonly record struct ValueDescription(long Value, string Text);
    }
}