using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Data.Abstract;
using ShopApp.Data.Entity;
using ShopApp.Web.Models;


namespace ShopApp.Web.Controllers;

public class OrderController : Controller
{
    private Cart cart;
    
    private IOrderRepository _orderRepository;

    private IUserRepository _userRepository;

    private IProductRepository _productRepository;

    public OrderController(Cart cartService,IOrderRepository orderRepository,IUserRepository userRepository,IProductRepository productRepository)
    {
        cart = cartService;
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
    }

    public IActionResult Checkout()
    {
        return View(new OrderModel() {Cart = cart});
    }

  [HttpPost]
public IActionResult Checkout(OrderModel model)
{
     
    if (cart.Items.Count == 0)
    {
        ModelState.AddModelError("", "Sepetinizde ürün yok");
        model.Cart = cart;
        return View(model);  
    }

     
    var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrEmpty(userIdStr))
    {
        ModelState.AddModelError("", "Giriş yapmanız gerekiyor.");
        model.Cart = cart;
        return View(model);  
    }

     
    var userId = int.Parse(userIdStr);

     
    var user = _userRepository.Users.FirstOrDefault(u => u.Id == userId);
    if (user == null)
    {
        ModelState.AddModelError("", "Kullanıcı bulunamadı.");
        model.Cart = cart;
        return View(model);  
    }

    if (ModelState.IsValid)
    {
        
        var order = new Order
        {
            Name = model.Name,
            Email = model.Email,
            City = model.City,
            AdressLine = model.AdressLine,
            OrderDate = DateTime.UtcNow,
            UserId = userId,  
            OrderItems = cart.Items.Select(i => new ShopApp.Data.Entity.OrderItem
            {
                ProductId = i.Product.Id,  
                Price = (double)i.Product.Price,  
                Quantity = i.Quantity  
            }).ToList()
        };

        
        foreach (var item in order.OrderItems)
        {
            var product = _productRepository.Products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product == null)
            {
                ModelState.AddModelError("", $"Ürün ID'si {item.ProductId} bulunamadı.");
                model.Cart = cart; 
                return View(model);  
            }
        }

        // Siparişi kaydet
        try
        {
            _orderRepository.SaveOrder(order); // Siparişi kaydet
            cart.Clear(); // Sepeti temizle

            return RedirectToPage("/Completed", new { OrderId = order.Id });  
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Sipariş kaydedilirken bir hata oluştu: " + ex.Message);
            model.Cart = cart;  
            return View(model);  
        }
    }
    else
    {
        model.Cart = cart; // Form geçersizse sepeti geri döndür
        return View(model);
    }
}

    private Payment ProcessPayment(OrderModel model)    
    {


        Options options = new Options();
        options.ApiKey = "";
        options.SecretKey = "";
        options.BaseUrl = "https://sandbox-api.iyzipay.com";


        // Payment request oluşturma
    CreatePaymentRequest request = new CreatePaymentRequest();
    request.Locale = Locale.TR.ToString();
    request.ConversationId = new Random().Next(111111111, 999999999).ToString();  
    request.Price = model?.Cart?.CalculateTotal().ToString();  
    request.PaidPrice = model?.Cart?.CalculateTotal().ToString();  
    request.Currency = Currency.TRY.ToString();  
    request.Installment = 1;  
    request.BasketId = "B67832";  
    request.PaymentChannel = PaymentChannel.WEB.ToString();
    request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

     
    PaymentCard paymentCard = new PaymentCard();
    paymentCard.CardHolderName = model?.CartName;  
    paymentCard.CardNumber = model?.CartNumber;  
    paymentCard.ExpireMonth = model?.ExpirationMonth;  
    paymentCard.ExpireYear = model?.ExpirationYear;  
    paymentCard.Cvc = model?.Cvc;  
    paymentCard.RegisterCard = 0;  
    request.PaymentCard = paymentCard;

     
    Buyer buyer = new Buyer();
    buyer.Id = model?.User.Id.ToString(); 
    buyer.Name = model?.User.FullName.Split(' ')[0];  
    buyer.Surname = model?.User.FullName.Split(' ').Last();  
    buyer.GsmNumber = model?.Phone;  
    buyer.Email = model?.Email;  
    buyer.IdentityNumber = "74300864791"; 
    buyer.LastLoginDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");  
    buyer.RegistrationDate = DateTime.UtcNow.AddYears(-1).ToString("yyyy-MM-dd HH:mm:ss");  
    buyer.RegistrationAddress = model?.AdressLine;  
    buyer.Ip = HttpContext.Connection.RemoteIpAddress?.ToString();  
    buyer.City = model?.City;  
    buyer.Country = "Turkey";  
    buyer.ZipCode = "34732";  
    request.Buyer = buyer;

     
    Address shippingAddress = new Address();
    shippingAddress.ContactName = model?.Name;  
    shippingAddress.City = model?.City;  
    shippingAddress.Country = "Turkey";  
    shippingAddress.Description = model?.AdressLine;  
    shippingAddress.ZipCode = "34742";  
    request.ShippingAddress = shippingAddress;

    Address billingAddress = new Address();
    billingAddress.ContactName = model?.Name;  
    billingAddress.City = model?.City;  
    billingAddress.Country = "Turkey";  
    billingAddress.Description = model?.AdressLine;  
    billingAddress.ZipCode = "34742";  
    request.BillingAddress = billingAddress;

     
    List<BasketItem> basketItems = new List<BasketItem>();
    foreach (var item in model?.Cart?.Items ?? Enumerable.Empty<CartItem>())
    {
        BasketItem basketItem = new BasketItem();
        basketItem.Id = item.Product.Id.ToString();  
        basketItem.Name = item.Product.Name;  
        basketItem.Category1 = "Telefon";  
        basketItem.ItemType = BasketItemType.PHYSICAL.ToString();  
        basketItem.Price = item.Product.Price.ToString();  
        basketItems.Add(basketItem);  
    }
    request.BasketItems = basketItems;

    // Ödeme işlemini başlatma
    Payment payment = Payment.Create(request, options);
    return payment;
    }

}