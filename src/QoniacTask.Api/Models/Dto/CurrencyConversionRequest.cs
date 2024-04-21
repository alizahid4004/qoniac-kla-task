﻿using QoniacTask.Api.Validation;
using System.ComponentModel.DataAnnotations;

namespace QoniacTask.Api.Models
{
    public record CurrencyConversionRequest(
        [Required]
        [DecimalRangeValidation(0, 999999999, 0, 99)]
        [Display(Name = "currency amount")] decimal Amount);
}