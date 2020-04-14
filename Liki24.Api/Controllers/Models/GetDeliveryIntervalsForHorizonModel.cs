using System;
using System.ComponentModel.DataAnnotations;

namespace Liki24.Api.Controllers.Models
{
    public class GetDeliveryIntervalsForHorizonModel
    {
        [Required]
        public DateTime? CurrentDate { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? Horizon { get; set; }
    }
}