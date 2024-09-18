using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShopApp.Data.Abstract;
using ShopApp.Data.Entity;
using ShopApp.Web.Models;


namespace ShopApp.Web.Controllers;

public class ProductController : Controller
{
    private  IProductRepository _productRepository;

    private ICategoryRepository _categoryRepository;

    private readonly IWebHostEnvironment _webHostEnvironment;


    public ProductController(IProductRepository productRepository,IWebHostEnvironment webHostEnvironment,ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;  

        _webHostEnvironment = webHostEnvironment;

        _categoryRepository = categoryRepository;
    }
    public IActionResult Index()
    {   
        var model = new ProductsViewModel{
            Products = _productRepository.Products.ToList()
        };

        return View(model);
    }

    [HttpGet]
    public  IActionResult Create()
    {   
        if (!User.IsInRole("admin"))
        {
            return RedirectToAction("Index", "Home");
        }
        
        ViewBag.Categories = _categoryRepository.Categories;     
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

         
        var product = await _productRepository.Products
                            .Include(p => p.Categories)
                            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

         
        ViewBag.Categories = await _categoryRepository.Categories.ToListAsync();

        
        var viewModel = new ProductsViewModel.ProductManagmentViewModel
        {
            ProductId = product.Id,
            Name = product.Name,
            Url = product.Url,
            Description = product.Description,
            Price = product.Price,
            TotalStock = product.TotalStock,
            Image = product.Image,
            Content = string.Join(",", product.Content),
            Categories = product.Categories.Select(c => c.Id).ToList() // Seçili kategoriler
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, ProductsViewModel.ProductManagmentViewModel model, IFormFile Image)
    {
        if (!User.IsInRole("admin"))
        {
            return RedirectToAction("Index", "Home");
        }

        
        var updateProduct = await _productRepository.Products
                            .Include(p => p.Categories)
                            .FirstOrDefaultAsync(prd => prd.Id == id);

        if (updateProduct == null)
        {
            return NotFound();
        }

        ViewBag.Categories = await _categoryRepository.Categories.ToListAsync();

        if (ModelState.IsValid)
        {
            
            if (Image != null && Image.Length > 0)
            {
                if (!string.IsNullOrEmpty(updateProduct.Image))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "img", updateProduct.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        try
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Eski resim silinirken hata: {ex.Message}");
                        }
                    }
                }

                var uploadsEditProductFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                var randomEditProductName = $"{Guid.NewGuid()}{Path.GetExtension(Image.FileName)}";
                var editProductFilePath = Path.Combine(uploadsEditProductFolder, randomEditProductName);

                using (var editPrdStream = new FileStream(editProductFilePath, FileMode.Create))
                {
                    await Image.CopyToAsync(editPrdStream);
                }

                updateProduct.Image = randomEditProductName;
            }

            // Ürün bilgilerini güncelle
            updateProduct.Name = model.Name;
            updateProduct.Description = model.Description;
            updateProduct.Url = model.Url;
            updateProduct.Price = model.Price;
            updateProduct.TotalStock = model.TotalStock;
            updateProduct.Content = model.Content.Split(",").ToList();

            // Kategorileri repository katmanında güncelle
            await _productRepository.EditProductAsync(updateProduct, model.Categories);

            return RedirectToAction("Index", "Home");
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        if (!User.IsInRole("admin"))
        {
            return RedirectToAction("Index", "Home");
        }
           var product = await _productRepository.Products.FirstOrDefaultAsync(p => p.Id == id);              

         if (product == null)
        {
            return NotFound();
        }

        return View(product);

    }

    [HttpPost]
    [ActionName("Delete")]// Aynı aksiyon adını kullandığımızı belirtir
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!User.IsInRole("admin"))
        {
            return RedirectToAction("Index", "Home");
        }
        var product = await _productRepository.Products.FirstOrDefaultAsync(p => p.Id == id);
        if(product == null)
        {
            return NotFound();
        }
        await _productRepository.DeleteProductAsync(id);
        return RedirectToAction("Index","Home");
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductsViewModel.ProductManagmentViewModel model, IFormFile Image)
    {   
        if(ModelState.IsValid)
        {
            if(Image != null && Image.Length > 0)
            {
                var uploadsProductFolder = Path.Combine(_webHostEnvironment.WebRootPath,"img");
                var randomProductName = string.Format($"{Guid.NewGuid().ToString()}{Path.GetExtension(Image.FileName)}");
                var productFilePath= Path.Combine(uploadsProductFolder, randomProductName);

                using (var prdStream = new FileStream(productFilePath, FileMode.Create))
                {
                    await Image.CopyToAsync(prdStream);
                }
            
                 
                var selectedCategories = await _categoryRepository.Categories.Where(c => model.Categories.Contains(c.Id)).ToListAsync();

                
                    var newProduct = new Product {
                    Name = model.Name,
                    Description = model.Description,
                    Url = model.Url,
                    Price = model.Price,
                    TotalStock = model.TotalStock,
                    Image = randomProductName,
                    Categories = selectedCategories,
                    Content = model.Content.Split(",").ToList()
                    };

                _productRepository.CreateProduct(newProduct);
                return RedirectToAction("Index","Home");

            }
        }
        ViewBag.Categories = _categoryRepository.Categories;

        return View(model); 
    }

    }