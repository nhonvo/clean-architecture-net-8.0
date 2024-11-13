using CleanArchitecture.IntegrationTest.Shared.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.IntegrationTest.TestCase;

public class BookTest : IClassFixture<InjectionFixture>
{
    private readonly IBookClient _bookClient;

    public BookTest(InjectionFixture fixture)
        => _bookClient = fixture.ServiceProvider.GetRequiredService<IBookClient>();

    [Fact(DisplayName = "[Book] Verify get book by id")]
    public async Task Book_ShouldSuccess_WhenGetBookById()
    {
        // Act
        //var result = await _bookClient.Get("1");

        // Assert
        Assert.True(true);
    }
}
// todo: check set up client, refer resharp
// return a object to assert
// set up docker, port, api gateway
// gen code
// move model to shared
// write auth local use dummy token
// update docker config

// push up awn uong

