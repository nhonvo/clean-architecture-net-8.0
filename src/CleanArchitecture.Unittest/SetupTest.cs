using AutoFixture;
using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interface;
using Microsoft.EntityFrameworkCore;
using Moq;
using CleanArchitecture.Application.Common.Mappings;

namespace CleanArchitecture.Unittest;

public class SetupTest : IDisposable
{

    protected readonly IMapper _mapperConfig;
    protected readonly Fixture _fixture;
    protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
    protected readonly ApplicationDbContext _dbContext;
    protected readonly Mock<IBookService> _bookServiceMock;
    protected readonly Mock<ICurrentTime> _currentTimeMock;
    protected readonly Mock<IBookRepository> _bookRepositoryMock;
    protected readonly Mock<IUserRepository> _userRepository;

    public SetupTest()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MapProfile());
        });
        _mapperConfig = mappingConfig.CreateMapper();
        _fixture = new Fixture();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _bookServiceMock = new Mock<IBookService>();
        _currentTimeMock = new Mock<ICurrentTime>();
        _bookRepositoryMock = new Mock<IBookRepository>();
        _userRepository = new Mock<IUserRepository>();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new ApplicationDbContext(options);

        _currentTimeMock.Setup(x => x.GetCurrentTime()).Returns(DateTime.UtcNow);
    }
    public void Dispose() => _dbContext.Dispose();
}
