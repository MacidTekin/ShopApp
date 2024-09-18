using ShopApp.Data.Entity;

namespace ShopApp.Web.Models;

public class Cart 
{
    public List<CartItem> Items { get; set; } = new List<CartItem>();//aşağıda ekelenen ürün bilgisi List<CartItem>() bu listeye eklenecek.

    public virtual void AddItem(Product product, int quantity)
    {   
        var item = Items.FirstOrDefault(i => i.Product.Id == product.Id);

        if(product.TotalStock >= quantity)
        {
            if(item == null) // Eğer ürün daha önce sepete eklenmediyse
            {
                Items.Add(new CartItem {Product = product, Quantity = quantity});
            }
            else
            {
                item.Quantity += quantity; // Ürün zaten sepette varsa miktarını arttır
            }
            product.TotalStock -= quantity;// Stoktan düş
        }
        else
        {
            throw new InvalidOperationException("Yeterli stok bulunmamaktadır.");
        }
    }

    public virtual void RemoveItem(Product product)
    {   
        var item = Items.FirstOrDefault(i => i.Product.Id == product.Id);
        if (item != null)
        {
            product.TotalStock += item.Quantity; // Ürünü sepetten çıkarırken stoğa geri ekleyin
            Items.Remove(item); // Ürünü sepetten kaldır
        }
    }

    public double CalculateTotal()
    {
        return Items.Sum(i => i.Product.Price * i.Quantity);// Toplam tutarı hesapla
    }

    public virtual void Clear()
    {
        Items.Clear();// Sepeti tamamen temizle
    }
}

public class CartItem
{   
    public int CartItemId { get; set; }

    public Product Product { get; set; } = new();

    public int Quantity { get; set; }
}