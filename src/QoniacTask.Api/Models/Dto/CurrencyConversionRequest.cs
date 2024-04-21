using QoniacTask.Api.Validation;
using System.ComponentModel.DataAnnotations;

namespace QoniacTask.Api.Models
{
    public record CurrencyConversionRequest(
        [property: Required]
        [property: DecimalRangeValidation(0, 999999999, 0, 99)]
        [property: Display(Name = "currency amount")] decimal Amount);
}