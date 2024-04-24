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

        [HttpPost("convert", Name = nameof(ConvertCurrency))]
        public IActionResult ConvertCurrency(
            [FromBody] CurrencyConversionRequest request)
        {
            //NOTE: I don't see the benefit of putting this behind an interface. It's testable as is
            //and putting it behind an interface would be pretty easy if the need arises.
            var description = NumberToWordsConverter.Convert(request.Amount, Options);
            var response = new CurrencyConversionResponse(description);
            return Ok(response);
        }

        [HttpPost("parse-and-convert", Name = nameof(ParseAndConvertCurrency))]
        public IActionResult ParseAndConvertCurrency(
            [FromBody] CurrencyParseAndConvertRequest request)
        {
            var currency = CurrencyParser.Parse(request.Amount);
            var description = NumberToWordsConverter.Convert(currency, Options);
            var response = new CurrencyConversionResponse(description);
            return Ok(response);
        }
    }
}