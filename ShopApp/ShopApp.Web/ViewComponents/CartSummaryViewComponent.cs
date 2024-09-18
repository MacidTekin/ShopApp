using Microsoft.AspNetCore.Mvc;
using ShopApp.Web.Models;

namespace ShopApp.Web.ViewComponents;

public class CartSummaryViewComponent : ViewComponent
{
    private Cart cart;
    public CartSummaryViewComponent(Cart cartService)
    {
        cart = cartService;
    }

    public IViewComponentResult Invoke()
    {
        return View(cart);
    }
}