using Microsoft.AspNetCore.Mvc;
using QoniacTask.Api.Models;
using QoniacTask.Server.Services;

namespace QoniacTask.Api.Controllers
{
    [ApiController]
    [Route("api/v1/convert-currency")]
    public class ConvertCurrencyController : ControllerBase
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

        [HttpPost(Name = "ConvertCurrency")]
        public IActionResult ConvertCurrency(
            [FromBody] CurrencyConversionRequest request)
        {
            //NOTE: I don't see the benefit of putting this behind an interface. It's testable as is
            //and putting it behind an interface would be pretty easy if the need arises.
            var description = NumberToWordsConverter.Convert(request.Amount, Options);
            var response = new CurrencyConversionResponse(description);
            return Ok(response);
        }
    }
}