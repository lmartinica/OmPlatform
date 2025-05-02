using OmPlatform.DTOs.OrderItems;

namespace OmPlatform.DTOs.Order
{
    public class GetOrderDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
        public ICollection<GetOrderItemDto>? OrderItems { get; set; }
    }
}
