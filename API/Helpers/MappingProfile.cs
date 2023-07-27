using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product,ProductToRerurnDto>()
            .ForMember(d=>d.ProductBrand,o=>o.MapFrom(s=>s.ProductBrand.Name))
            .ForMember(d=>d.ProductType,o=>o.MapFrom(s=>s.ProductType.Name));
        }
    }
}