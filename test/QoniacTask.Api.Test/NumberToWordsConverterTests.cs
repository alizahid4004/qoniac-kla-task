using QoniacTask.Server.Services;

namespace QoniacTask.Api.Test
{
    public class NumberToWordsConverterTests
    {
        public static TheoryData<decimal, string> DefaultConversionData =>
        new()
        {
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

        private static readonly NumberToWordsOptions Options = new()
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
            var description = NumberToWordsConverter.Convert(number, Options);

            Assert.Equal(expectedResult, description);
        }
    }
}