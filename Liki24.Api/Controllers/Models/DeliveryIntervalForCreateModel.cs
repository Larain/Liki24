using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Liki24.BL.Helpers;
using Liki24.Contracts.Models;

namespace Liki24.Api.Controllers.Models
{
    public class DeliveryIntervalForCreateModel : IValidatableObject
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal? Price { get; set; }

        [Required]
        [RegularExpression(@"^\d{2}:\d{2}")]
        public string AvailableFrom { get; set; }

        [Required]
        [RegularExpression(@"^\d{2}:\d{2}")]
        public string AvailableTo { get; set; }

        [Required]
        public DeliveryIntervalType? Type { get; set; }

        [Required]
        [RegularExpression(@"^\d{2}:\d{2}")]
        public string ExpectedFrom { get; set; }

        [Required]
        [RegularExpression(@"^\d{2}:\d{2}")]
        public string ExpectedTo { get; set; }

        [Required]
        public List<string> AvailableDaysOfWeek { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!AvailableFrom.IsValidDateTime())
            {
                yield return new ValidationResult(
                    $"{AvailableFrom} is not valid",
                    new[] { nameof(AvailableFrom) });
            }
            if (!AvailableTo.IsValidDateTime())
            {
                yield return new ValidationResult(
                    $"{AvailableTo} is not valid",
                    new[] { nameof(AvailableTo) });
            }
            if (!ExpectedFrom.IsValidDateTime())
            {
                yield return new ValidationResult(
                    $"{ExpectedFrom} is not valid",
                    new[] { nameof(ExpectedFrom) });
            }
            if (!ExpectedTo.IsValidDateTime())
            {
                yield return new ValidationResult(
                    $"{ExpectedTo} is not valid",
                    new[] { nameof(ExpectedTo) });
            }

            if (TimeSpan.Parse(AvailableFrom) > TimeSpan.Parse(AvailableTo))
            {
                yield return new ValidationResult(
                    $"AvailableFrom = {AvailableFrom} must be earlier then AvailableTo = {AvailableTo}",
                    new[] { nameof(AvailableFrom), nameof(AvailableTo) });
            }

            if (TimeSpan.Parse(ExpectedFrom) > TimeSpan.Parse(ExpectedTo))
            {
                yield return new ValidationResult(
                    $"ExpectedFrom = {ExpectedFrom} must be earlier then ExpectedTo = {ExpectedTo}",
                    new[] { nameof(ExpectedFrom), nameof(ExpectedTo) });
            }

            if (AvailableDaysOfWeek != null && AvailableDaysOfWeek.Any()) yield break;
        }
    }
}