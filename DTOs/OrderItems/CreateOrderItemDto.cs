namespace OmPlatform.DTOs.OrderItems
{
    public class CreateOrderItemDto
    {
        // TODO add [Required] - framework verifica in controller
        // Data annotations API
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
