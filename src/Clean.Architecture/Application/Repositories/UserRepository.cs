using Clean.Architecture.Infrastructure.Data;
using Clean.Architecture.Infrastructure.Interface;

namespace Clean.Architecture.Application.Repositories
{
    public class UserRepository(ApplicationDbContext context) : GenericRepository<User>(context), IUserRepository
    {
    }
}
