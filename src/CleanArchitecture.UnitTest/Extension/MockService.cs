using System.Net;
using System.Text;
using CleanArchitecture.Infrastructure.ExternalServices.HealthCheck;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace CleanArchitecture.Unittest.Extension;
public static class MockService
{
    public static ILogger<ExternalServiceHealthCheck> GetMockLogger()
    {
        // Mocking ILogger<ExternalServiceHealthCheck>
        var mockLogger = new Mock<ILogger<ExternalServiceHealthCheck>>();

        // Optionally, you can set up specific behavior for logging if needed
        mockLogger
            .Setup(logger => logger.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()))
            .Verifiable();

        return mockLogger.Object;
    }

    public static IHttpClientFactory GetMockHttpFactory(HttpStatusCode statusCode, string content = "")
    {
        // Mock the HttpMessageHandler to simulate sending HTTP requests and responses
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        // Prepare the response you want to return from the mocked handler
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = statusCode,
            Content = new StringContent(content, Encoding.UTF8, "application/json")
        };

        // Setup the SendAsync method to return the prepared response
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Create the HttpClient using the mocked HttpMessageHandler
        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://mockedurl.com") // Set a base address for your client
        };

        // Mock the IHttpClientFactory to return the mocked HttpClient
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory
            .Setup(factory => factory.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        // Return the mocked IHttpClientFactory
        return mockFactory.Object;
    }
}
