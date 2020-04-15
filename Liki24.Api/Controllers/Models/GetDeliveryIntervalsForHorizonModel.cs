using System;
using System.ComponentModel.DataAnnotations;

namespace Liki24.Api.Controllers.Models
{
    public class GetDeliveryIntervalsForHorizonModel
    {
        [Required]
        public DateTime? CurrentDate { get; set; }

        [Required]
        [Range(0, 7)] // no sense to calculate more then 7 days due to repeated values
        public uint? Horizon { get; set; }
    }
}