namespace QoniacTask.Api.Services
{
    internal class CurrencyParserOptions
    {
        //NOTE: all of this can also be deduced from a CultureInfo property
        public char DecimalSeparator { get; set; } = ',';

        public char GroupSeparator { get; set; } = ' ';

        public long? WholeMinimum { get; set; } = 0;
        
        public long? WholeMaximum { get; set; } = 999_999_999;
    }
}