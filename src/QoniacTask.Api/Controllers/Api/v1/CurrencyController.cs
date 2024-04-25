using Microsoft.AspNetCore.Mvc;
using QoniacTask.Api.Models;
using QoniacTask.Api.Services;
using QoniacTask.Server.Services;

namespace QoniacTask.Api.Controllers
{
    [ApiController]
    [Route("api/v1/currency")]
    public class CurrencyController : ControllerBase
    {
        private static readonly NumberToWordsOptions Options = new()
        {
            IntegerUnit = "dollars",
            IntegerUnitForOne = "dollar",
            DecimalUnit = "cents",
            DecimalUnitForOne = "cent",
            AddAnd = false,
            IntegerDecimalSeparationText = "and"
        };

        /// <summary>
        /// Takes in a currency value as a decimal and describes it in text.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("convert", Name = nameof(ConvertCurrency))]
        [ProducesResponseType(typeof(CurrencyConversionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public IActionResult ConvertCurrency(
            [FromQuery] CurrencyConversionRequest request)
        {
            //NOTE: I don't see the benefit of putting this behind an interface. It's testable as is
            //and putting it behind an interface would be pretty easy if the need arises.
            //putting it behind an interface later also means i will have more info to design a better abstraction
            var description = NumberToWordsConverter.Convert(request.Amount, Options);
            var response = new CurrencyConversionResponse(description);
            return Ok(response);
        }

        /// <summary>
        /// Takes in a currency value as a pre-formatted sting with space for group separators and comma for
        /// decimal separator and describes it in text.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("parse-and-convert", Name = nameof(ParseAndConvertCurrency))]
        [ProducesResponseType(typeof(CurrencyConversionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        public IActionResult ParseAndConvertCurrency(
            [FromQuery] CurrencyParseAndConvertRequest request)
        {
            //NOTE: just like the other controller method, this can be easily placed behind an interface
            var currency = CurrencyParser.Parse(request.Amount);
            var description = NumberToWordsConverter.Convert(currency, Options);
            var response = new CurrencyConversionResponse(description);

            return Ok(response);
        }
    }
}