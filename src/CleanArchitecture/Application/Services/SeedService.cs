using System.Text.Json;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using Newtonsoft.Json;

namespace CleanArchitecture.Application.Services
{
    public class SeedService(IUnitOfWork unitOfWork, ILoggerFactory logger) : ISeedService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger _logger = logger.CreateLogger<SeedService>();
        public async Task Seed(CancellationToken token)
        {
            try
            {

                if (!await _unitOfWork.BookRepository.AnyAsync())
                {
                    string json = File.ReadAllText(Constant.Url.BookData);
                    List<Book> books = JsonConvert.DeserializeObject<List<Book>>(json)!;
                    await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.BookRepository.AddRangeAsync(books), token);
                };

                if (!await _unitOfWork.UserRepository.AnyAsync())
                {
                    string json = File.ReadAllText(Constant.Url.UserData);
                    List<User> users = JsonConvert.DeserializeObject<List<User>>(json)!;
                    await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.UserRepository.AddRangeAsync(users), token);
                };
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Fail seeding data: {exception}", exception);
                throw new UserFriendlyException(ErrorCode.BadRequest, $"Fail seeding data: {exception}");
            }
        }
    }
}
