using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NestApp.Data;
using NestApp.Models;
using NuGet.Common;
using System.Data;

namespace NestApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CategoryController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env= env;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _context.Categories.Include(x=>x.Products).ToListAsync();
            return View(result);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "The blank must be filled!");
                    return View(category);
            }

            if (!category.File.ContentType.Contains("image"))
            {
                ModelState.AddModelError("", "Invalid File");
                return View(category);
            }
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + category.File.FileName;

            Category newcategory = new Category() { 
            Name = category.Name,
            Icon = uniqueFileName,
            };

            string path = Path.Combine(_env.WebRootPath, "Client", "categoryIcons", uniqueFileName);

            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);

            await category.File.CopyToAsync(fs);

            await _context.Categories.AddAsync(newcategory);
            await _context.SaveChangesAsync();
            return View();
        }
    }
}
