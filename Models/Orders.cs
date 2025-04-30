namespace OmPlatform.Models
{
    public class Orders
    {
        public Guid Id { get; set; } 
        public Guid UserId { get; set; }
        public int TotalPrice { get; set; }
        public int Status { get; set; }
        public DateTime Created { get; set; }
    }
}