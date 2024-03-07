using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace NestApp.Models
{
    public class Category : Base
    {
        public string Icon { get; set; } = null!;
        public string Name { get; set; } = null!;
        [NotMapped]
        public IFormFile File { get; set; }
        public List<Product> Products { get; set; }
    }
}
