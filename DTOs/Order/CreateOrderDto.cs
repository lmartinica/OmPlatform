using OmPlatform.DTOs.OrderItems;

namespace OmPlatform.DTOs.Order
{
    public class CreateOrderDto
    {
        public ICollection<CreateOrderItemDto> OrderItems { get; set; }
    }
}
