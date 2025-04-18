namespace OmPlatform.DTOs.Order
{
    public class GetOrderItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
