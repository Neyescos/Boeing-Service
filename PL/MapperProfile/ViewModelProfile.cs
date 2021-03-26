using AutoMapper;
using BLL.Models;
using PL.Interfaces;
using PL.Models;

namespace PL.MapperProfile
{
    public class ViewModelProfile : Profile, IViewProfile
    {
        public ViewModelProfile()
        {
            CreateMap<PlaneModelDto, PlaneModelViewModel>()
                .ForMember(
                dest => dest.PlaneParts,
                opts => opts.MapFrom(src => src.PlaneParts))
                .ForMember(
                dest => dest.Users,
                opts => opts.MapFrom(src => src.Users))
                .ForMember(
                dest => dest.ProductionYear,
                opts => opts.MapFrom(src => src.YearOfProd))
                .ReverseMap();

            CreateMap<PlanePartViewModel, PlanePartDto>()
                .ForMember(
                dest => dest.PlaneModel,
                opts => opts.MapFrom(src => src.PlaneModel)).ReverseMap();

            CreateMap<UserDto, UserViewModel>()
                .ForMember(
                dest => dest.PlaneModels,
                opts => opts.MapFrom(src => src.PlaneModels))
                .ReverseMap();
           
        }

        public Mapper GetMapper()
        {
            var config = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ViewModelProfile());
            });
            return new Mapper(config);
        }
    }
}
