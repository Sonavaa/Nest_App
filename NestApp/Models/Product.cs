namespace NestApp.Models
{
    public class Product : Base
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double? Rating { get; set; }
        public decimal Price { get; set; } = default!;
        public decimal? DiscountedPrice { get; set; }
    }
}
