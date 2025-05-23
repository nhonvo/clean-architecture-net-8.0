using AutoMapper;
using CleanArchitecture.Shared.Models.Book;
using CleanArchitecture.Shared.Models.User;

namespace CleanArchitecture.Application.Common.Mappings;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Book, BookDTO>().ReverseMap();
        CreateMap<Book, AddBookRequest>().ReverseMap();
        CreateMap<Book, UpdateBookRequest>().ReverseMap();


        CreateMap<User, UserSignInRequest>().ReverseMap();
        CreateMap<User, UserSignInResponse>().ReverseMap();
        CreateMap<User, UserSignUpRequest>().ReverseMap();
        CreateMap<User, UserSignUpResponse>().ReverseMap();
        CreateMap<User, UserProfileResponse>().ReverseMap();
    }
}
