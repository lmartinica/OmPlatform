namespace OmPlatform.Queries
{
    public class ProductQuery
    {
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public bool? Stock { get; set; }
        public string? Category { get; set; }
        public string? Search { get; set; }
    }
}
