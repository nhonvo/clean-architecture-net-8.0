using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interface;

namespace CleanArchitecture.Application.Repositories;

public class BookRepository(ApplicationDbContext context) : GenericRepository<Book>(context), IBookRepository
{
}
