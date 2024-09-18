using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopApp.Data.Concrete.EfCore;
using ShopApp.Data.Entity;

namespace ShopApp.Data.Concrete;

public static class SeedData
{
    public static async Task TestVerileriniDoldurAsync(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ShopDbContext>();

            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }

            // Categories
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Küçük Ev Aletleri", Url = "kucuk-ev-aletleri"},
                    new Category { Name = "Telefon", Url = "telefon" },
                    new Category { Name = "Tv & Görüntü", Url = "tv-goruntu" },
                    new Category { Name = "Beyaz Eşya", Url = "beyaz-esya" },
                    new Category { Name = "Bilgisayar & Tablet", Url = "bilgisayar-tablet" },
                    new Category { Name = "Kişisel Bakım Aletleri", Url = "kisisel-bakim-aletleri" },
                    new Category { Name = "Giyilebilir Teknoloji", Url = "giyilebilir-teknoloji" },
                    new Category { Name = "Foto & Kamera", Url = "foto-kamera" }
                };
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // Users
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User { UserName = "admin", FullName= "Macid Tekin", Email="info@mcdtkn.com", Password="123456", Image = "avatar.jpg" },
                    new User { UserName = "dogutkn",FullName= "Doğukan Tekin", Email="info@dgkntkn.com", Password="654321", Image = "avatar.jpg" },
                    new User { UserName = "mcdtkn",FullName= "Tekin Macid", Email="mcdtkn35@gmail.com", Password="123123", Image = "avatar.jpg" },
                };
                context.Users.AddRange(users);
                await context.SaveChangesAsync();
            }

            // Products
            if (!context.Products.Any())
            {
                var categories = context.Categories.ToList(); 
                var products = new List<Product>
                {
                    new Product { 
                        Name = "Robot Süpürge", 
                        Description = "Philips Homerun 3000 serisi",
                        Url = "robot-supurge",
                        Content =  new List<string>{
                            "Tek seferde Süpürür ve Siler",
                            "Otomatik Boşaltma istasyonu",
                            "HomeRun uygulaması",
                            "Yüksek emiş gücü ",
                            "360° lazer navigasyon",
                            "Halıda emiş gücünü artırma",
                            "250-500L Hazne Kapesitesi"
                        },
                        Price = 13.450,
                        Image = "0fd3dd7c-bf51-484e-bac0-32a5f973ab44.jpg", 
                        TotalStock = 50, 
                        Categories = new List<Category> {categories[0]},
                        Comments = new List<Comment>{ 
                            new Comment {
                                CommentText = "Güzel ürün ve emiş gücü yüksek",
                                CommentDate = DateTime.Now.AddDays(-1),
                                UserId = 1
                                },
                            new Comment {
                                CommentText = "Şarj süresi yeterli değil",
                                CommentDate = DateTime.Now.AddHours(-10),
                                UserId = 2
                                }
                            }
                        },
                    new Product { 
                        Name = "Apple Akıllı Telefon", 
                        Description = "IPhone 15 Pro Max 1TB",
                        Url = "apple-akilli-telefon",
                        Content =  new List<string>{
                            "15,5 cm (6.1) 2556 x 1179 Piksel Ceramic Shield",
                            "5G Çift SIM NanoSIM + eSIM",
                            "Üçlü kamera 48 MP 12 MP 12 MP",
                            "Lityum-İyon (Li-Ion) Kablosuz şarj olma",
                            "4320p(Ultra HD) 8K Video Kayıt Çözünürlüğü",
                            "3000-4000(mAh) Pil gücü",
                            "1 TB Dahili Hafıza",
                            "IOS 17"
                        },
                        Price = 103.750, 
                        Image = "2.jpg", 
                        TotalStock = 30, 
                        Categories = new List<Category> {categories[1]}
                        },
                    new Product { 
                        Name = "Samsung Akıllı Telefon", 
                        Description = "Samsung Z Fold 1TB",
                        Url = "samsung-akilli-telefon", 
                        Content =  new List<string>{
                            "Ekran Boyutu: 6.2 İnç",
                            "Dahili Depolama: 1 TB",
                            "Batarya Kapasitesi (Tipik): 4400 mAh",
                            "Üçlü kamera",
                            "4320p(Ultra HD) 8K Video Kayıt Çözünürlüğü",
                            "Android 14",
                        },
                        Price = 90.450, 
                        Image = "3.jpg", 
                        TotalStock = 25, 
                        Categories = new List<Category> {categories[1]}
                        },
                    new Product { 
                        Name = "Samsung Televizyon", 
                        Description = "Samsung 85 inch Neo QLed Smart Tv", 
                        Url = "samsung-televizyon",
                        Content =  new List<string>{
                            "Yapay zeka ile 8K Görüntü Yükseltme Pro",
                            "NQ8 3.Nesil AI İşlemci",
                            "Infinity Air Tasarım",
                            "Samsung Tizen İşletim Sistemi",
                            "Hareket Hızlandırıcı 240 Hz",
                            "100 Hz (4K 240 Hz’e kadar) 8K (7,680 x 4,320) ",
                        },
                        Price = 330.650, 
                        Image = "4.jpg", 
                        TotalStock = 15, 
                        Categories = new List<Category> {categories[2]}
                        },
                    new Product { 
                        Name = "Hoover Kurutma Makinesi", 
                        Description = "Hoover 10kg WiFi & Bluetooth Kurutma Makinesi",
                        Url = "hoover-kurutma-makinesi",
                        Content =  new List<string>{
                            "Uzun Ömürlü Kullanım",
                            "Mükemmel Performans ve Verimlilik",
                            "40 Ek Program Anti-alerji Program",
                            "Etiket Tarama Aquavision Teknolojisi",
                            "hOn App ile Ekstra Fonksiyonlar ve Programla",
                            "Isı Pompalı ",
                            "All in One Teknolojisi"
                        },
                        Price = 13.200, 
                        Image = "5.jpg", 
                        TotalStock = 55, 
                        Categories = new List<Category> {categories[3]}
                        },
                    new Product { 
                        Name = "Ipad Pro", 
                        Description = "Apple Ipad Pro 12.9 inch",
                        Url = "ipad-pro",
                        Content =  new List<string>{
                            "Max Ekran Çözünürlüğü 2800 x 1752",
                            "Kalem Uyumluluğu",
                            "Sekiz Çekirdekli İşlemci",
                            "Liquid Retina XDR Ekran",
                            "8 GB Ram Kapasitesi",
                            "İşletim Sistemi iPadOS 16 ",
                        }, 
                        Price = 62.500, 
                        Image = "6.jpg", 
                        TotalStock = 25, 
                        Categories = new List<Category> {categories[4]}
                        },
                    new Product { 
                        Name = "Philips Tıraş Makinesi", 
                        Description = "Philips 5000 Serisi AuqaTouch Tıraş Makinesi",
                        Url = "philips-tiras-makinesi",
                        Content =  new List<string>{
                            "Hassas çelik bıçaklar",
                            "360° Esnek başlıklar",
                            "Entegre hassas düzeltici",
                            "7 yıl motor ve pil ömrü",
                            "Güç Uyum sensörü",
                            "LED ekran ile sezgisel kullanım ",
                            "1 saatte tam şarj ,5 dakikada hızlı şarj"
                        }, 
                        Price = 18.200, 
                        Image = "7.jpg", 
                        TotalStock = 60, 
                        Categories = new List<Category> {categories[5]}
                        },
                    new Product { 
                        Name = "Samsung Akıllı Saat", 
                        Description = "Samsung Galaxy Watch 6",
                        Url = "samsung-akilli-saat",
                        Content =  new List<string>{
                            "Önceki modellere kıyasla %20 daha büyük ekran ve dayanıklı safir kristal cam",
                            " 40mm model için 300mAh ve 44mm model için 425mAh kapasite",
                            "Kalp atış hızı bölge uyarıları ve otomatik yürüyüş/koşu algılama gibi özellikler",
                            "Paslanmaz çelik kasa ve kolay değiştirilebilir kayışlar",
                            "Exynos W930 işlemci ve 2GB RAM ile %18 daha hızlı performans",
                        },  
                        Price = 6.500, 
                        Image = "8.jpg", 
                        TotalStock = 10, 
                        Categories = new List<Category> {categories[6]}
                        },
                    new Product { 
                        Name = "DJI Aksiyon Kamerası", 
                        Description = "DJI Osmo Action 4",
                        Url = "dji-aksiyon-kamerasi",
                        Content =  new List<string>{
                            "Video çekimlerinde titreşimi azaltan gelişmiş elektronik görüntü sabitleme (EIS)",
                            "4K (4:3): 3840×2880, 24-60fps",
                            "4K: 4 kat yavaş çekim (120fps)",
                            "IP68 sertifikası ile su altında 18 metreye kadar dayanıklılık",
                            "Wi-Fi ve Bluetooth ile kablosuz bağlantı desteği",
                            "MicroSD kart ile depolama genişletme ",
                            "RockSteady 3.0, RockSteady 3.0+, HorizonBalancing, HorizonSteady"
                        },
                        Price = 20.490, 
                        Image = "9.jpg", 
                        TotalStock = 85, 
                        Categories = new List<Category> {categories[7]}
                        }
                };
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }

if (!context.Orders.Any())
{
    var orders = new List<Order>
    {
        new Order 
        { 
            OrderDate = DateTime.UtcNow, 
            AdressLine = "Bahçelievler/Çankaya", 
            City = "Ankara", 
            Email = "mcdtkn35@gmail.com", 
            Name = "Tekin Macid", 
            Phone = "05555555555",
            UserId = context.Users.First().Id // Kullanıcı id'sini alın
        }
    };

    context.Orders.AddRange(orders);
    await context.SaveChangesAsync();
}

// OrderItem Seed Data (Sipariş ve ürünlerin mevcut olduğundan emin olun)
if (!context.OrderItems.Any())
{
    var orderItems = new List<OrderItem>
    {
        new OrderItem 
        { 
            OrderId = context.Orders.First().Id, // İlk eklediğimiz siparişin Id'si
            ProductId = context.Products.First().Id, // İlk eklenen ürünün Id'si
            Price = 330.65, 
            Quantity = 1 
        }
    };

    context.OrderItems.AddRange(orderItems);
    await context.SaveChangesAsync();
}


        }
    }
}

