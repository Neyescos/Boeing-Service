using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using DAL.Entities;

namespace BLL.MapperProfile
{
    public class CustomResolver : IValueResolver<UserDto, User, byte[]>
    {
        public byte[] Resolve(UserDto source, User destination, byte[] destMember, ResolutionContext context)
        {
            var encoding = new PasswordEncodingService();
            byte[] password = null;
            if (source.Password != null)
            {
                 password = encoding.CalculateSHA256(source.Password);
            }
            return password;
        }
    }
    public class DtoProfile : Profile, IMapperProfile
    {
        public DtoProfile()
        {

            CreateMap<User, UserDto>()
                .ForMember(
                dest => dest.PlaneModels,
                opts => opts.MapFrom(src => src.PlaneModels))
                .ForMember(
                dest => dest.Password,
                opts => opts.MapFrom(src => src.Password));
            CreateMap<UserDto, User>()
                .ForMember(
                dest => dest.PlaneModels,
                opts => opts.MapFrom(src => src.PlaneModels))
                .ForMember(
                dest => dest.Password,
                opts => opts.MapFrom(new CustomResolver()));
            CreateMap<PlaneModel, PlaneModelDto>()
                .ForMember(
                dest => dest.PlaneParts,
                opts => opts.MapFrom(src => src.PlaneParts))
                .ForMember(
                dest => dest.Users,
                opts => opts.MapFrom(src => src.Users));
            CreateMap<PlaneModelDto, PlaneModel>()
                .ForMember(
                dest => dest.PlaneParts,
                opts => opts.MapFrom(src => src.PlaneParts))
                .ForMember(
                dest => dest.Users,
                opts => opts.MapFrom(src => src.Users));
            CreateMap<PlanePart, PlanePartDto>()
                .ForMember(
                dest => dest.PlaneModel,
                opts => opts.MapFrom(src => src.PlaneModel));
            CreateMap<PlanePartDto, PlanePart>()
                .ForMember(
                dest => dest.PlaneModel,
                opts => opts.MapFrom(src => src.PlaneModel));

        }

        public Mapper GetMapper()
        {
            var config = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DtoProfile());
            });
            return new Mapper(config);
        }
    }

}
