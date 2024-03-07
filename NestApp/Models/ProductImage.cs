using System.ComponentModel.DataAnnotations.Schema;

namespace NestApp.Models
{
    public class ProductImage : Base
    {
        public string Url { get; set; } = null!;
        [NotMapped]
        public IFormFile File { get; set; } = null!;
        public bool IsMain { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
