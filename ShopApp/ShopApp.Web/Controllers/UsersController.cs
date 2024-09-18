using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApp.Data.Abstract;
using ShopApp.Data.Entity;
using ShopApp.Web.Models;

namespace ShopApp.Web.Controllers;

public class UsersController : Controller
{   
    private readonly IUserRepository _userRepository;

    private readonly  IWebHostEnvironment _webHostEnvironment;


    private readonly  IOrderRepository _orderRepository;

    public UsersController(IUserRepository userRepository, IWebHostEnvironment webHostEnvironment,IOrderRepository orderRepository)
    {
        _userRepository = userRepository;
        _webHostEnvironment = webHostEnvironment;
        _orderRepository = orderRepository;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if(User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Index","Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {   
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Register()
    {   
        if(User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Index","Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model, IFormFile Image)
    {
        if (ModelState.IsValid)
        {
            if (Image != null && Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{Path.GetExtension(Image.FileName)}");
                var filePath = Path.Combine(uploadsFolder, randomFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                var user = await _userRepository.Users.FirstOrDefaultAsync(x => x.UserName == model.UserName && x.Email == model.Email);
                if (user == null)
                {
                    _userRepository.CreateUser(new User
                    {
                        UserName = model.UserName,
                        FullName = model.FullName,
                        Email = model.Email,
                        Password = model.Password,
                        Image = randomFileName
                    });
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya Eposta zaten kullanılıyor.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Profil resmi yüklenmedi.");
            }
        }

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {       
        if(ModelState.IsValid)
        {
            var isUser = _userRepository.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);

            if(isUser != null)
            {
                var userClaims = new List<Claim>();
                userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.Id.ToString()));
                userClaims.Add(new Claim(ClaimTypes.Name, isUser.UserName ?? ""));
                userClaims.Add(new Claim(ClaimTypes.GivenName, isUser.FullName ?? ""));
                userClaims.Add(new Claim(ClaimTypes.UserData, isUser.Image ?? ""));

                if(isUser.Email == "info@mcdtkn.com")
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                }
                var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties{
                    IsPersistent = true
                };

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );
                return RedirectToAction("Index","Home");
            }

            else
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış");
            }
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userRepository.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);

        if (user == null)
        {
            return NotFound();
        }
         
        var userOrders = await _orderRepository.Orders
            .Where(o => o.UserId == user.Id)  
            .ToListAsync();

     
    user.Orders = userOrders;


        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Profile(User model, IFormFile? Image)
    {
        if (!User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Login");
        }

        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userRepository.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);

            if (user == null)
            {
                return NotFound();
            }

            // Eğer yeni bir resim yüklendiyse
            if (Image != null && Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                var randomFileName = string.Format($"{Guid.NewGuid()}{Path.GetExtension(Image.FileName)}");
                var filePath = Path.Combine(uploadsFolder, randomFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                // Eski resmi sil (varsa)
                if (!string.IsNullOrEmpty(user.Image))
                {
                    var oldImagePath = Path.Combine(uploadsFolder, user.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                user.Image = randomFileName;
            }

             
            user.UserName = model.UserName;
            user.FullName = model.FullName;
            user.Email = model.Email;

             
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                user.Password = model.Password;
            }

            _userRepository.UpdateUser(user);  // Kullanıcı güncellemesi
            return RedirectToAction("Profile");
        }

        return View(model);
    }

    


}