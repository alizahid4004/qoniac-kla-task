using QoniacTask.Api.Services;

namespace QoniacTask.Api.Tests
{
    public class CurrencyParserTests
    {
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
        public void Parse_ShouldThrowInvalidOperationException_WhenInvalidOptionsArePassed()
        {
            Action result = () => CurrencyParser.Parse(
                "123",
                new CurrencyParserOptions { DecimalSeparator = ' ', GroupSeparator = ' ' });

            Assert.Throws<InvalidOperationException>(result);
        }
    }
}