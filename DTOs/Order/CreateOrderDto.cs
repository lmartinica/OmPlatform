using OmPlatform.DTOs.OrderItems;
using System.ComponentModel.DataAnnotations;

namespace OmPlatform.DTOs.Order
{
    public class CreateOrderDto
    {
        [Required]
        public ICollection<CreateOrderItemDto>? OrderItems { get; set; }
    }
}
