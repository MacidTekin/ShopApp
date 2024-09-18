using AutoMapper;
using ShopApp.Data.Entity;

namespace ShopApp.Web.Models;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Product,ProductsViewModel.ProductDetailsViewModel>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src)); // 'Product' alanını doğrudan mapliyoruz
    }
}