namespace QoniacTask.Server.Services
{
    internal class NumberToWordsOptions
    {
        private string _negativeText = "negative";
        private string _integerUnit = string.Empty;
        private string _fractionUnit = string.Empty;
        private string _integerUnitForOne = string.Empty;
        private string _fractionUnitForOne = string.Empty;

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
        public string FractionUnit
        {
            get
            {
                return _fractionUnit ?? string.Empty;
            }
            set
            {
                _fractionUnit = value;
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
        /// What unit, if any, is used for the fraction part of the number when it's 1 (unit/unity).
        /// </summary>
        /// <remarks>Default: no value is used</remarks>
        public string FractionUnitForOne
        {
            get
            {
                return _fractionUnitForOne ??= string.Empty;
            }
            set
            {
                _fractionUnitForOne = value;
            }
        }

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