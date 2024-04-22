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
    }
}