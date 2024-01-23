using AutoMapper;
using CleanArchitecture.Application.Common.Models.Book;
using CleanArchitecture.Application.Common.Models.User;

namespace CleanArchitecture.Application.Common.Mappings;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Book, BookDTO>().ReverseMap();

        CreateMap<User, LoginRequest>().ReverseMap();
        CreateMap<User, RegisterRequest>().ReverseMap();
        CreateMap<User, UserDTO>().ReverseMap();
    }
}
