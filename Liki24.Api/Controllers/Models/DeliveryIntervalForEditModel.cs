using System.ComponentModel.DataAnnotations;

namespace Liki24.Api.Controllers.Models
{
    public class DeliveryIntervalForEditModel : DeliveryIntervalForCreateModel
    {
        [Required]
        public int? Id { get; set; }
    }
}