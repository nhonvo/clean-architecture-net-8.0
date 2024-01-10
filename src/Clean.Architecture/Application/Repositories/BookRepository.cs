using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Infrastructure.Interface;

namespace Clean.Architecture.Application.Repositories
{
    public class BookRepository(ApplicationDbContext context) : GenericRepository<Book>(context), IBookRepository
    {
    }
}
