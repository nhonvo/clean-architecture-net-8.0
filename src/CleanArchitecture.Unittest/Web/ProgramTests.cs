using Microsoft.AspNetCore.Mvc.Testing;

namespace CleanArchitecture.Unittest.Web;

public class ProgramTests
{
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _application;
    public ProgramTests()
    {
        _application = new WebApplicationFactory<Program>();
        _httpClient = _application.CreateClient();
    }

    //[Fact]
    //public async Task Program_HealthCheck_ShouldReturnHealthResult()
    //{

    //    var response = await _httpClient.GetStringAsync("/health");
    //    Assert.Equal("Healthy", response);
    //}
}
