namespace OmPlatform.DTOs.OrderItems
{
    public class GetOrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
