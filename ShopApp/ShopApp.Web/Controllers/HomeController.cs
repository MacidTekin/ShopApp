using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApp.Data.Abstract;
using ShopApp.Web.Models;
using ShopApp.Data.Entity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using AutoMapper;

namespace ShopApp.Web.Controllers;

public class HomeController : Controller
{   
    private  IProductRepository _productRepository;

    private ICommentRepository _commentRepository;

    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly IMapper _mapper;

    
    public HomeController(IProductRepository productRepository, ICommentRepository commentRepository,IWebHostEnvironment webHostEnvironment, IMapper mapper)
    {
        _productRepository = productRepository;  

        _commentRepository = commentRepository;

        _webHostEnvironment = webHostEnvironment;

        _mapper = mapper;
    }

    public async Task<IActionResult> Index(string url, int page = 1, int pageSize = 4)
    {
         
        var productsQuery = _productRepository.Products.AsQueryable();

         
        if (!string.IsNullOrEmpty(url))
        {
            productsQuery = productsQuery.Where(x => x.Categories.Any(t => t.Url == url));
        }

         
        var totalProducts = await productsQuery.CountAsync();

         
        if (totalProducts == 0)
        {
            return NotFound();
        }

        
        var pagedProducts = await productsQuery
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

         
        var model = new ProductsViewModel
        {
            Products = pagedProducts,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize),
            SelectedCategoryUrl = url  
        };

        return View(model);
    }
    
    
    public async Task<IActionResult> Details(string url, int page = 1, int pageSize = 5)
    {    
        var product = await _productRepository.Products
                        .Include(x => x.Comments)
                        .ThenInclude(x => x.User)
                        .FirstOrDefaultAsync(p => p.Url == url);

        if(product == null)
        {
            return NotFound();
        }
        var productDetails = _mapper.Map<ProductsViewModel.ProductDetailsViewModel>(product);
            // Yorumları sayfalıyoruz
        productDetails.PagedComments = productDetails.Product.Comments
                                        .OrderByDescending(c => c.CommentDate)
                                        .Skip((page - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToList();

        productDetails.CurrentPage = page;
        productDetails.PageSize = pageSize;
        productDetails.TotalComments = productDetails.Product.Comments.Count;

        return View(productDetails);
    }

    [HttpPost]
    public JsonResult AddComment(int ProductId, string Text)
    {   
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  
        var username = User.FindFirstValue(ClaimTypes.GivenName);
        var avatar = User.FindFirstValue(ClaimTypes.UserData);
        var entity = new Comment {

            ProductId = ProductId,
            CommentText = Text,
            CommentDate = DateTime.Now,
            UserId = int.Parse(userId ?? "")  

        };
        _commentRepository.CreateComment(entity);
        
        return Json(new{  
        username,  
        Text,
        entity.CommentDate,
        avatar          //UserController.cs dosyasında UserDatayı ekledık ve resimi artık oradan alıyoruz yyukarıda tanımladıgığımız avatarı burada kullandık.
        });
    }

    
}