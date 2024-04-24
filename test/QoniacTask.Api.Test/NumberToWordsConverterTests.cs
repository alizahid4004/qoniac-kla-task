using QoniacTask.Server.Services;

namespace QoniacTask.Api.Test
{
    public class NumberToWordsConverterTests
    {
        //NOTE: using TheoryData because InlineData doesn't work with decimals
        public static TheoryData<decimal, string> DefaultConversionData =>
        new()
        {
            { -876_543.21m, "negative eight hundred and seventy-six thousand five hundred and forty-three and twenty-one" },
            { -333m, "negative three hundred and thirty-three" },
            { -0m, "zero" },
            { 0m, "zero" },
            { 1m, "one" },
            { 25.10m, "twenty-five and ten" },
            { 999_999_999.99m, "nine hundred and ninety-nine million nine hundred and ninety-nine thousand nine hundred and ninety-nine and ninety-nine" },
        };

        [Theory]
        [MemberData(nameof(DefaultConversionData))]
        public void Convert_ShouldConvertToText(
            decimal number, string expectedResult)
        {
            var description = NumberToWordsConverter.Convert(number);

            Assert.Equal(expectedResult, description);
        }

        private static readonly NumberToWordsOptions UnitOptions = new()
        {
            IntegerUnit = "units",
            IntegerUnitForOne = "unit",
            DecimalUnit = "decimals",
            DecimalUnitForOne = "decimal",
        };

        public static TheoryData<decimal, string> UnitConversionData =>
        new()
        {
            { -876_543.21m, "negative eight hundred and seventy-six thousand five hundred and forty-three units and twenty-one decimals" },
            { 0m, "zero units" },
            { 1m, "one unit" },
            { 1.1m, "one unit and one decimal" },
            { 1.10m, "one unit and ten decimals" },
            { 25.10m, "twenty-five units and ten decimals" },
            { 999_999_999.99m, "nine hundred and ninety-nine million nine hundred and ninety-nine thousand nine hundred and ninety-nine units and ninety-nine decimals" },
        };

        [Theory]
        [MemberData(nameof(UnitConversionData))]
        public void Convert_ShouldConvertIncludingUnits_WhenUnitSettingsAreProvided(
            decimal number, string expectedResult)
        {
            var description = NumberToWordsConverter.Convert(number, UnitOptions);

            Assert.Equal(expectedResult, description);
        }

        public static TheoryData<decimal, string, string> NegativeConversionData =>
        new()
        {
            { -876_543.21m, "minus" ,"minus eight hundred and seventy-six thousand five hundred and forty-three and twenty-one" },
            { -0m, "minus", "zero" },
            { -1m, "negative" ,"negative one" },
        };

        [Theory]
        [MemberData(nameof(NegativeConversionData))]
        public void Convert_ShouldConvertWithNegativeText_WhenNegativeTextSettingsAreProvided(
            decimal number, string negativeTextToUse, string expectedResult)
        {
            var description = NumberToWordsConverter.Convert(
                number, new NumberToWordsOptions() { NegativeText = negativeTextToUse });

            Assert.Equal(expectedResult, description);
        }

        public static TheoryData<decimal, bool, string> HyphenConversionData =>
        new()
        {
            { 999_999_999.99m, true, "nine hundred and ninety-nine million nine hundred and ninety-nine thousand nine hundred and ninety-nine and ninety-nine" },
            { 999_999_999.99m, false, "nine hundred and ninety nine million nine hundred and ninety nine thousand nine hundred and ninety nine and ninety nine" },
        };

        [Theory]
        [MemberData(nameof(HyphenConversionData))]
        public void Convert_ShouldRespectHyphenSettings_WhenHyphenSettingsAreProvided(
            decimal number, bool useHyphens, string expectedResult)
        {
            var description = NumberToWordsConverter.Convert(
                number, new NumberToWordsOptions() { HyphenateTens = useHyphens });

            Assert.Equal(expectedResult, description);
        }

        private static readonly NumberToWordsOptions CurrencyOptions = new()
        {
            IntegerUnit = "dollars",
            IntegerUnitForOne = "dollar",
            DecimalUnit = "cents",
            DecimalUnitForOne = "cent",
            AddAnd = false,
            IntegerDecimalSeparationText = "and"
        };

        public static TheoryData<decimal, string> CurrencyConversionData =>
        new()
        {
            { 0m, "zero dollars" },
            { 1m, "one dollar" },
            { 25.10m, "twenty-five dollars and ten cents" },
            { 999_999_999.99m, "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents" },
        };

        [Theory]
        [MemberData(nameof(CurrencyConversionData))]
        public void Convert_ShouldConvertToCurrencyDescription_WhenCurrencySettingsAreProvided(
            decimal number, string expectedResult)
        {
            var description = NumberToWordsConverter.Convert(number, CurrencyOptions);

            Assert.Equal(expectedResult, description);
        }
    }
}