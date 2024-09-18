using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ShopApp.Data.Abstract;
using ShopApp.Data.Concrete;
using ShopApp.Data.Concrete.EfCore;
using ShopApp.Web.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddDbContext<ShopDbContext>(options => {
    options.UseSqlite(builder.Configuration["ConnectionStrings:ShopDbContext"], b => b.MigrationsAssembly("ShopApp.Web"));
});

builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EfCategoryRepository>();
builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<IOrderRepository, EfOrderRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

builder.Services.AddDistributedMemoryCache();//sessions ı mermory üzerinde tutmak için
builder.Services.AddSession();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();//IHttpContextAccessor nesnesi bize contexti verecek onun ınterface i

builder.Services.AddScoped<Cart>(sc => SessionCart.GetCart(sc));

var app = builder.Build();

app.UseStaticFiles();

app.UseSession();//sessions ı aktıf ettik 

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Test verilerini doldur
        await SeedData.TestVerileriniDoldurAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanında bir hata oluştu.");
    }
}

app.MapControllerRoute(
    name : "product_details",
    pattern : "products/details/{url}",
    defaults : new{controller = "Home", action = "Details"}
);

app.MapControllerRoute(
    name : "product_by_category",
    pattern : "products/category/{url}",
    defaults : new{controller = "Home", action = "Index"}
);

app.MapControllerRoute(
    name: "pagination_with_category",
    pattern: "products/category/{url}",
    defaults : new{controller = "Home", action = "Index"}
);

app.MapControllerRoute(
    name: "pagination_with_comments",
    pattern: "Product/Details/{url}",
    defaults: new { controller = "Home", action = "Details" }
);

app.MapControllerRoute(
    name : "default",
    pattern : "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();
app.Run();
