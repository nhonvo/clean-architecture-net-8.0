using AutoMapper;
using CleanArchitecture.Application.Common.Models.AuthIdentity.UsersIdentity;
using CleanArchitecture.Application.Common.Models.Book;
using CleanArchitecture.Application.Common.Models.User;

namespace CleanArchitecture.Application.Common.Mappings;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Book, BookDTO>().ReverseMap();

        CreateMap<User, UserSignInRequest>().ReverseMap();
        CreateMap<User, UserSignInResponse>().ReverseMap();
        CreateMap<User, UserSignUpRequest>().ReverseMap();
        CreateMap<User, UserSignUpResponse>().ReverseMap();
        CreateMap<User, UserProfileResponse>().ReverseMap();


        CreateMap<ApplicationUser, UserViewModel>()
            .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.Name));

        CreateMap<UserQueryResult, UserViewModel>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.users.Id))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.users.Name))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.users.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.users.Email))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.users.Status))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.avatar))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.role));
    }
}
