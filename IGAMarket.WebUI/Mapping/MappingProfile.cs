using AutoMapper;
using IGAMarket.DtoLayer.FireDtos;
using IGAMarket.DtoLayer.ProductDtos;
using IGAMarket.EntityLayer.Concrete;

namespace IGAMarket.WebUI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, AddProductDto>().ReverseMap();
            CreateMap<Fire, AddFireDto>().ReverseMap();
        }
    }
}
