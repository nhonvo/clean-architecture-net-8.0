using System.Text.Json;
using Clean.Architecture.Application.Common.Interfaces;
using Newtonsoft.Json;

namespace Clean.Architecture.Application.Services
{
    public class SeedService(IUnitOfWork unitOfWork, ILoggerFactory logger) : ISeedService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger _logger = logger.CreateLogger<SeedService>();
        public async Task Seed()
        {
            try
            {

                if (!await _unitOfWork.BookRepository.AnyAsync())
                {
                    string json = File.ReadAllText(Constant.Url.BookData);
                    List<Book> books = JsonConvert.DeserializeObject<List<Book>>(json)!;
                    await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.BookRepository.AddRangeAsync(books));
                };

                if (!await _unitOfWork.UserRepository.AnyAsync())
                {
                    string json = File.ReadAllText(Constant.Url.UserData);
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(json)!;
                    await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.UserRepository.AddRangeAsync(users));
                };
            }
            catch (Exception exception)
            {
                // Log or handle the exception
                _logger.LogError(exception, "Error deserializing JSON: {exception}", exception);
            }
        }
    }
}
