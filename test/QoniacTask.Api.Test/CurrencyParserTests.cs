using QoniacTask.Api.Services;

namespace QoniacTask.Api.Tests
{
    public class CurrencyParserTests
    {
        [Theory]
        [InlineData("-0", 0L, null)]
        [InlineData("0", 0L, null)]
        [InlineData("0,01", 0L, 1U)]
        [InlineData("10,5", 10L, 50U)]
        [InlineData("999 999 999", 999_999_999L, null)]
        [InlineData("999 999 999,99", 999_999_999L, 99U)]
        public void Parse_ShouldParseFormattedAmount(
            string formattedAmount, long whole, uint? change)
        {
            var currency = CurrencyParser.Parse(formattedAmount);

            Assert.Equal(whole, currency.Whole);
            Assert.Equal(change, currency.Change);
        }

        [Fact]
        public void Parse_ShouldThrowArgumentException_WhenEmptyStringIsPassed()
        {
            Action result = () => CurrencyParser.Parse(string.Empty);

            Assert.Throws<ArgumentException>(result);
        }

        [Fact]
        public void Parse_ShouldThrowFormatException_WhenStringContainsAlphabets()
        {
            Action result = () => CurrencyParser.Parse("123 456 X");

            Assert.Throws<FormatException>(result);
        }

        [Fact]
        public void Parse_ShouldThrowFormatException_WhenDecimalSeparatorRepeats()
        {
            Action result = () => CurrencyParser.Parse("123 456,12,34");

            Assert.Throws<FormatException>(result);
        }

        [Fact]
        public void Parse_ShouldThrowInvalidOperationException_WhenGroupAndDecimalSeparatorsAreSame()
        {
            Action result = () => CurrencyParser.Parse(
                "123",
                new CurrencyParserOptions { DecimalSeparator = ' ', GroupSeparator = ' ' });

            Assert.Throws<InvalidOperationException>(result);
        }

        [Theory]
        [InlineData("-26")]
        [InlineData("101")]
        public void Parse_ShouldParseFormattedAmount_WhenXSettingsAreProvided(
            string formattedAmount)
        {
            Action result = () => CurrencyParser.Parse(
                formattedAmount, new CurrencyParserOptions { WholeMinimum = -25, WholeMaximum = 100 });

            Assert.Throws<FormatException>(result);
        }

        [Fact]
        public void Parse_ShouldThrowInvalidOperationException_WhenMinimumIsLargerThanMaximum()
        {
            Action result = () => CurrencyParser.Parse(
                "123",
                new CurrencyParserOptions { WholeMinimum = 2, WholeMaximum = 1 });

            Assert.Throws<InvalidOperationException>(result);
        }
    }
}