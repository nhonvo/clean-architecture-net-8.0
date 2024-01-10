using Microsoft.EntityFrameworkCore;

namespace Clean.Architecture.Infrastructure.Data
{
    public class ApplicationDbContextInitializer(ApplicationDbContext context, ILoggerFactory logger)
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger _logger = logger.CreateLogger<ApplicationDbContextInitializer>();

        public async Task InitializeAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError("Migration error {exception}", exception);
                throw;
            }
        }
    }
}
