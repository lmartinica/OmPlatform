namespace OmPlatform.DTOs.Product
{
    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public int? Stock { get; set; }
        public string? Category { get; set; }
    }
}
