using AutoMapper;
using Clean.Architecture.Application.Common.Models.Book;
using Clean.Architecture.Application.Common.Models.User;

namespace Clean.Architecture.Application.Common.Mappings;

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
