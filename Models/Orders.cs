namespace OmPlatform.Models
{
    public class Orders
    {
        public int Id { get; set; } 
        public int UserId { get; set; }
        public int TotalPrice { get; set; }
        public int Status { get; set; }
        public DateTime Created { get; set; }
    }
}
