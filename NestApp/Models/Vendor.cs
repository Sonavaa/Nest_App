namespace NestApp.Models
{
    public class Vendor : Base
    {
        public string Name { get; set; }
        public ICollection<ProductsVendor> productsVendors { get; set; }
    }
}
