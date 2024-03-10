using NestApp.Models;

namespace NestApp.ViewModels
{
    public class ProductVM
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}
