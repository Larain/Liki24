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
        public uint? Horizon { get; set; }
    }
}