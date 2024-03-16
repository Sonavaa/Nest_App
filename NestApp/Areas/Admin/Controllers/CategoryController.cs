using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NestApp.Data;
using NestApp.FileExtension;
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
            var result = await _context.Categories.Where(x => x.IsDeleted == false).Include(x=>x.Products).ToListAsync();
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
                //ModelState.AddModelError("", "The blank must be filled!");
                    return View(category);
            }

            if (!category.File.CheckFileType("image"))
            {
                ModelState.AddModelError("", "Invalid File");
                return View(category);
            }

            //var mb = category.File.Length * 1024 * 2;
            if(!category.File.CheckFileSize(2))
            {
                ModelState.AddModelError("", "File's length must be less than 2 mb!");
                return View(category);
            }

            string uniqueFileName = await category.File.SaveFilesAsync(_env.WebRootPath, "client", "categoryIcons");

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

        //public async Task<IActionResult> Delete(int id)
        //{
        //    Category? category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }
        //    //_context.Categories.Remove(category); Hard Delete
        //    category.IsDeleted = true;
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        public async Task<IActionResult> Edit(int id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        public async Task<IActionResult> Update (int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }
       Category? existCategory = await _context.Categories.FirstOrDefaultAsync(x=>x.Id == id);
            if (existCategory == null)
            {
                return NotFound();
            }
            if(category.File != null)
            {
                if (!category.File.CheckFileSize(2))
                {
                    ModelState.AddModelError("File", "File's length must be less than 2mb!");
                    return View(category);
                }
                if(!category.File.CheckFileType("image"))
                {
                    ModelState.AddModelError("File", "File's type must be image!");
                    return View(category);
                }

                category.File.DeleteFile(_env.WebRootPath, "client", "categoryIcons", existCategory.Icon);

                var uniqueFileName = await category.File.
                    SaveFilesAsync(_env.WebRootPath, "client", "categoryIcons");
                existCategory.Icon = uniqueFileName;
                existCategory.Name = category.Name;
                _context.Update(existCategory);
            }
            else
            {
                category.Icon = existCategory.Icon;
                _context.Categories.Update(category);
            }

            await _context.SaveChangesAsync();
            if (category.Name == null)
            {
                return RedirectToAction("Edit", new { id = id });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Category? category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category is null)
            {
                return NotFound();
            }
            //category.File.DeleteFile(_env.WebRootPath, "client", "categoryIcons", existsCategory.Icon);

            category.IsDeleted = true; 
            await _context.SaveChangesAsync();

            var categories = await _context.Categories.Include(x => x.Products).Where(x => !x.IsDeleted).ToListAsync();

            return PartialView("_CategoryPartial", categories);
        }
    }

}
