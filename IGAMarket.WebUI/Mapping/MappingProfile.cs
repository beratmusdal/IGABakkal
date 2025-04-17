using AutoMapper;
using IGAMarket.DtoLayer.DailyClosurDtos;
using IGAMarket.DtoLayer.FireDtos;
using IGAMarket.DtoLayer.MonthlyDtos;
using IGAMarket.DtoLayer.ProductDtos;
using IGAMarket.DtoLayer.SaleDtos;
using IGAMarket.DtoLayer.UserDtos;
using IGAMarket.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace IGAMarket.WebUI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, AddProductDto>().ReverseMap();
            CreateMap<Product, ResultProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Fire, AddFireDto>().ReverseMap();
            CreateMap<User, CreateUserDto>();
            CreateMap<DailyClosur, CreateDailyDto>().ReverseMap();
            CreateMap<DailyClosur, SelectMonthlyDto>().ReverseMap();
            CreateMap<DailyClosur, MonthlyDto>().ReverseMap();
            CreateMap<Sale, SaleDto>();
        }
    }
}
