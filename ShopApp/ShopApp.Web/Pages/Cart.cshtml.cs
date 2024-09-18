using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShopApp.Data.Abstract;
using ShopApp.Web.Helpers;
using ShopApp.Web.Models;

namespace ShopApp.Web.Pages;

public class CartModel : PageModel
{
    private IProductRepository _productRepository;

    public CartModel(IProductRepository productRepository, Cart cartService)
    {
        _productRepository = productRepository;
        Cart = cartService;
    }
    
    public Cart? Cart { get; set; }  

    public void OnGet() //sayfa ilk yüklendiğinde çalışacak metod 
    {   
        //Cart obejesi oluşturulduğu için sessionları buradan sildik. Aynısını OnPost ve OnPostRemove için yaptık.
        //Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();//sepet boşsa null döner null değilse Cart nesnesi oluşturur

    }

    public IActionResult OnPost(int id) 
    {
        var product = _productRepository.Products.FirstOrDefault(i => i.Id == id);

        if(product != null)
        {
            Cart?.AddItem(product,1); //seçilen üründen 1 tane alıyor
        }
            

        return RedirectToPage("/Cart");  
    }


    public IActionResult OnPostRemove(int id)
    {
        Cart?.RemoveItem((Cart.Items.First(p => p.Product.Id == id).Product));
        return RedirectToPage("/Cart");
    }
}