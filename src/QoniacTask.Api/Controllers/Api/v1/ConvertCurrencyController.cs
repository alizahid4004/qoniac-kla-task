using Microsoft.AspNetCore.Mvc;
using QoniacTask.Api.Models;

namespace QoniacTask.Api.Controllers
{
    [ApiController]
    [Route("api/v1/convert-currency")]
    public class ConvertCurrencyController : ControllerBase
    {
        [HttpPost(Name = "ConvertCurrency")]
        public IActionResult ConvertCurrency(
            [FromBody] CurrencyConversionRequest request)
        {
            return Ok(request);
        }
    }
}