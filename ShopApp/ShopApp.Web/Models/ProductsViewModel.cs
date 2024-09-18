using System.ComponentModel.DataAnnotations;
using ShopApp.Data.Entity;

namespace ShopApp.Web.Models;

public class ProductsViewModel
{
    public List<Product> Products { get; set; } = new();

    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public string SelectedCategoryUrl { get; set; } = null!;

    public class ProductDetailsViewModel
    {
        public Product Product { get; set; } = null!;
        
        public List<Comment>? PagedComments { get; set; } 

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalComments { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalComments / PageSize);
    }
    public class ProductManagmentViewModel
    {   
        public int ProductId { get; set; } 

        [Required]
        [Display(Name ="Ürün Adı")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name ="Ürün Açıklaması")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name ="Ürün Route Url")]
        public string Url { get; set; } = string.Empty;


        [Required]
        [Display(Name ="Ürün Fiyatı")]
        public double Price { get; set; } 


        [Required]
        [Display(Name ="Stok Miktarı")]
        public int TotalStock { get; set; }

        
        [Display(Name ="Ürün Resmi")]
        public string Image { get; set; } = string.Empty;

        [Required]
        [Display(Name ="Kategori")]
        public List<int> Categories { get; set; } = new();

        [Required]
        [Display(Name ="Ürün İçeriği")]
        public string Content { get; set; } = string.Empty;
    }

}