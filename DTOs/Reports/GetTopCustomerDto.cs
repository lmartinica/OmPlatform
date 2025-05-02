namespace OmPlatform.DTOs.Reports
{
    public class GetTopCustomerDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public int TotalSpent { get; set; }
    }
}
