namespace QoniacTask.Server.Services
{
    internal class NumberToWordsOptions
    {
        private string _negativeText = "negative";
        private string _integerUnit = string.Empty;
        private string _decimalUnit = string.Empty;
        private string _integerUnitForOne = string.Empty;
        private string _decimalUnitForOne = string.Empty;
        private string _integerDecimalSeparationText = "and";

        /// <summary>
        /// What word should be used to describe negative number.
        /// </summary>
        /// <remarks>Default: "negative"</remarks>
        public string NegativeText
        {
            get
            {
                return _negativeText ??= string.Empty;
            }
            set
            {
                _negativeText = value;
            }
        }

        /// <summary>
        /// What unit, if any, is used for the integer (characteristic) part of the number
        /// </summary>
        /// <remarks>Default: no value is used</remarks>
        public string IntegerUnit
        {
            get
            {
                return _integerUnit ??= string.Empty;
            }
            set
            {
                _integerUnit = value;
            }
        }

        /// <summary>
        /// What unit, if any, is used for the decimal (mantissa) part of the number
        /// </summary>
        /// <remarks>Default: no value is used</remarks>
        public string DecimalUnit
        {
            get
            {
                return _decimalUnit ?? string.Empty;
            }
            set
            {
                _decimalUnit = value;
            }
        }

        /// <summary>
        /// What unit, if any, is used for 1 (unit/unity).
        /// </summary>
        /// <remarks>Default: no value is used</remarks>
        public string IntegerUnitForOne
        {
            get
            {
                return _integerUnitForOne ??= string.Empty;
            }
            set
            {
                _integerUnitForOne = value;
            }
        }

        /// <summary>
        /// What unit, if any, is used for the decimal (mantissa) part of the number when it's 1 (unit/unity).
        /// </summary>
        /// <remarks>Default: no value is used</remarks>
        public string DecimalUnitForOne
        {
            get
            {
                return _decimalUnitForOne ??= string.Empty;
            }
            set
            {
                _decimalUnitForOne = value;
            }
        }

        /// <summary>
        /// Text that separates the integration description and the decimal description
        /// </summary>
        /// <remarks>Default: "and"</remarks>
        public string IntegerDecimalSeparationText
        {
            get
            {
                return _integerDecimalSeparationText ??= string.Empty;
            }
            set
            {
                _integerDecimalSeparationText = value;
            }
        }

        /// <summary>
        /// Symbol that separates the integer and decimal part of a number such as a dot or a comma.
        /// </summary>
        /// <remarks>Default: ,</remarks>
        public char DecimalSymbol { get; set; } = ',';

        /// <summary>
        /// Whether numbers from 21 to 99 should have a hyphen between them.
        /// </summary>
        /// <example>When true, 21 will be converted to "twenty-two".</example>
        /// <remarks>Default: true</remarks>
        public bool HyphenateTens { get; set; } = true;

        /// <summary>
        /// Whether to add the word "and" before the last number.
        /// </summary>
        /// <example>When true, 4305 becomes "four thousand three hundred and five" otherwise it's "four thousand three hundred five"</example>
        /// <remarks>Default: true</remarks>
        public bool AddAnd { get; set; } = true;

        //NOTE: another property that can be added here is CultureInfo to support other languages and cultures
    }
}