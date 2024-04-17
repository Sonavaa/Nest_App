using System.ComponentModel.DataAnnotations.Schema;

namespace NestApp.Models
{
    public class Product : Base
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double? Rating { get; set; }
        public decimal Price { get; set; } = default!;  
        public decimal? DiscountedPrice { get; set; }

        public List<ProductImage> ProductImages { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public bool IsDeleted { get; set; } = default!;
        public int VendorId { get; set; }   
        public Vendor? Vendor { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        [NotMapped]
        public IFormFile? MainFile { get; set; }
        [NotMapped]
        public IFormFile? HoverFile { get; set; }

    }
}
