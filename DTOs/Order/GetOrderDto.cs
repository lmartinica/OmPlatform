namespace OmPlatform.DTOs.Order
{
    public class GetOrderDto
    {
        public Guid Id { get; set; }
        public int TotalPrice { get; set; }
        public int Status { get; set; }
        public DateTime Created { get; set; }
        public List<GetOrderItemDto>? Items { get; set; }
    }
}
