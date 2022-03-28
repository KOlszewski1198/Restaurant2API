using AutoMapper;
using Restaurant2API.Entities;
using Restaurant2API.Models;

namespace Restaurant2API
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));

            CreateMap<Dish, DishDto>();

            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(r => r.Address, 
                    c => c.MapFrom(dto => new Address()
                        { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));

            CreateMap<Dish, DishExtendedDto>()
                .ForMember(m => m.RestaurantName, c => c.MapFrom(s => s.Restaurant.Name));

            CreateMap<CreateDishDto, Dish>();
        }

    }
}
