
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NestApp.Data;
using NestApp.FileExtension;
using NestApp.Models;

namespace P237_Nest.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]

public class ProductController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public ProductController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        List<Product> products = await _context.Products
                                               .Include(x => x.ProductImages)
                                               .Include(x => x.Category)
                                               .ToListAsync();
        return View(products);


    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _context.Categories.ToListAsync();

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        if (_context.Products.Any(p => p.Name == product.Name))
        {
            ModelState.AddModelError("", "Product already exists");
            return View(product);
        }
        product.ProductImages = new List<ProductImage>();
        if (product.File != null)
        {
            foreach (var file in product.File)
            {

                if (!file.CheckFileize(10))
                {
                    ModelState.AddModelError("File", "File cannot be more than 10mb");
                    return View(product);
                }


                if (!file.CheckFileType("image"))
                {
                    ModelState.AddModelError("File", "File must be image type!");
                    return View(product);
                }

                var filename = await file.SaveFileAsync(_env.WebRootPath, "client", "assets", "imgs/products");
                var additionalProductImages = CreateProduct(filename, false, false, product);

                product.ProductImages.Add(additionalProductImages);

            }
        }
        if (!product.MainFile.CheckFileSize(10))
        {
            ModelState.AddModelError("MainFile", "File cannot be more than 10mb");
            return View(product);
        }


        if (!product.MainFile.CheckFileType("image"))
        {
            ModelState.AddModelError("MainFile", "File must be image type!");
            return View(product);
        }

        var mainFileName = await product.MainFile.SaveFilesAsync(_env.WebRootPath, "Client", "imgs", "products");
        var mainProductImageCreate = CreateProduct(mainFileName, false, true, product);

        product.ProductImages.Add(mainProductImageCreate);

        if (!product.HoverFile.CheckFileSize(10))
        {
            ModelState.AddModelError("HoverFile", "File cannot be more than 10mb");
            return View(product);
        }


        if (!product.HoverFile.CheckFileType("image"))
        {
            ModelState.AddModelError("HoverFile", "File must be image type!");
            return View(product);
        }

        var hoverFileName = await product.HoverFile.SaveFilesAsync(_env.WebRootPath, "Client", "imgs", "products");
        var hoverProductImageCreate = CreateProduct(hoverFileName, true, false, product);
        product.ProductImages.Add(hoverProductImageCreate);



        await _context.Products.AddAsync(product);

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public ProductImage CreateProduct(string url, bool isHover, bool isMain, Product product)
    {
        return new ProductImage
        {
            Url = url,
            IsHover = isHover,
            IsMain = isMain,
            Product = product
        };
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || id < 1) return View("404");
        ViewBag.Categories = await _context.Categories.ToListAsync();
        var product = await _context.Products.Include(x => x.ProductImages)
                                             .Include(x => x.Category)
                                             .FirstOrDefaultAsync(x => x.Id == id);
        if (product == null) return View("404");


        return View(product);
    }
    [HttpPost]
    public async Task<IActionResult> Update(int id, Product product)
    {
        if (id != product.Id || id == null || id < 1) return BadRequest();

        var existProduct = await _context.Products.FindAsync(id);


        if (product.File != null)
        {
            foreach (var file in product.File)
            {

                if (!file.CheckFileize(10))
                {
                    ModelState.AddModelError("File", "File cannot be more than 10mb");
                    return View(product);
                }


                if (!file.CheckFileType("image"))
                {
                    ModelState.AddModelError("File", "File must be image type!");
                    return View(product);
                }
                var filename = await file.SaveFileAsync(_env.WebRootPath, "client", "assets", "imgs/products");
                var additionalProductImages = CreateProduct(filename, false, false, product);
                existProduct.ProductImages.Add(additionalProductImages);
            }
        }
        if (product.MainFile != null)
        {
            if (!product.MainFile.CheckFileSize(10))
            {
                ModelState.AddModelError("MainFile", "File cannot be more than 10mb");
                return View(product);
            }


            if (!product.MainFile.CheckFileType("image"))
            {
                ModelState.AddModelError("MainFile", "File must be image type!");
                return View(product);
            }

            product.MainFile.DeleteFile(_env.WebRootPath, "client", "assets", "imgs/products", existProduct.ProductImages.FirstOrDefault(x => x.IsMain).Url);
            var mainFileName = await product.MainFile.SaveFilesAsync(_env.WebRootPath, "Client", "imgs", "products");
            var mainProductImage = CreateProduct(mainFileName, false, false, product);
            existProduct.ProductImages.Add(mainProductImage);

        }
        if (product.HoverFile != null)
        {
            if (!product.HoverFile.CheckFileSize(10))
            {
                ModelState.AddModelError("HoverFile", "File cannot be more than 10mb");
                return View(product);
            }


            if (!product.HoverFile.CheckFileType("image"))
            {
                ModelState.AddModelError("HoverFile", "File must be image type!");
                return View(product);
            }

            product.HoverFile.DeleteFile(_env.WebRootPath, "Client", "imgs", "products", existProduct.ProductImages.FirstOrDefault(x => x.IsHover).Url);
            var hoverFileName = await product.HoverFile.SaveFilesAsync(_env.WebRootPath, "Client", "imgs", "products");
            var hoverProductImageCreate = CreateProduct(hoverFileName, true, false, product);
            existProduct.ProductImages.Add(hoverProductImageCreate);
        }

        existProduct.Name = product.Name;
        existProduct.Description = product.Description;
        existProduct.Price = product.Price;
        existProduct.Rating = product.Rating;
        existProduct.DiscountedPrice = product.DiscountedPrice;
        existProduct.CategoryId = product.CategoryId;


        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


}
