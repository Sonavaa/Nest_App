using System.ComponentModel.DataAnnotations.Schema;

namespace NestApp.Models
{
    public class Vendor : Base
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? ContactInfo { get; set; }
        public double? Rating { get; set; }
        

        [NotMapped]
        public List<Product>? Products { get; set; }
    }
}
