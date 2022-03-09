using AutoMapper;
using ParkyAPI.Models.DTOs;
using ParkyAPI.Models;

namespace ParkyAPI.ParkyMapping
{
    public class ParkyMappings : Profile
    {
        public ParkyMappings()
        {
            CreateMap<NationalPark,NationalParkDTO>().ReverseMap();
            CreateMap<Trail, TrailDTO>().ReverseMap();
            CreateMap<Trail, TrailCreateDTO>().ReverseMap();
            CreateMap<Trail, TrailUpdateDTO>().ReverseMap();
        }
    }
}
