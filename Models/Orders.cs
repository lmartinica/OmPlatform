namespace OmPlatform.Models
{
    public class Orders
    {
        public Guid Id { get; set; } 
        public Guid UserId { get; set; }
        public int TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
        public Users User { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
    }
}