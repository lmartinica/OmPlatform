namespace OmPlatform.DTOs.Reports
{
    public class GetTopProductDto
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public int TotalQuantity { get; set; }
        public int TotalRevenue { get; set; }

    }
}
