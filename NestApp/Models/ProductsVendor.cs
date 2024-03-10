namespace NestApp.Models
{
    public class ProductsVendor : Base
    {
        public Product Product { get; set; }
        public int Productid { get; set; }
        public Vendor Vendor { get; set; }
        public int VendorId { get; set; }
    }
}
