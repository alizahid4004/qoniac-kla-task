using Microsoft.AspNetCore.Mvc.ModelBinding;
using QoniacTask.Api.Validation;
using System.ComponentModel.DataAnnotations;

namespace QoniacTask.Api.Models
{
    public record CurrencyConversionRequest(
        [Required]
        [BindRequired]
        [DecimalRangeValidation(0, 999999999, 0, 99)]
        [Display(Name = "currency amount")] decimal Amount);

    public record CurrencyParseAndConvertRequest(
        [Required]
        [BindRequired]
        [MinLength(1)]
        [Display(Name = "formatted currency amount")]
        string Amount);
}