using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestApp.Data;
using NestApp.Models;
using NestApp.ViewModels;

namespace NestApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(x=>x.Category)
                .Include(x => x.ProductImages)
                .Include(x => x.Vendor)
                .OrderByDescending(x => x.Id).Take(20).ToListAsync();

            var categories = await _context.Categories
                .Include(x => x.Products).ToListAsync();
            ProductVM productVM = new ProductVM()
            {
                Products = products,
                Categories = categories
            };

            return View(productVM);
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}

