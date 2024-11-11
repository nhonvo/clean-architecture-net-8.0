using CleanArchitecture.IntegrationTest.Models;
using CleanArchitecture.IntegrationTest.Shared.Interface;

namespace CleanArchitecture.IntegrationTest.Shared.Client;
public class BookClient : IBookClient
{
    private readonly HttpClient _client;

    public BookClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<ApiResponse<object, string>> Get(string id)
    {
        var response = await _client.GetAsync($"/{id}");

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            return new ApiResponse<object, string>
            {
                HttpResponseMessage = response,
                Data = data
            };
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return new ApiResponse<object, string>
            {
                HttpResponseMessage = response,
                ErrorData = errorContent
            };
        }
    }

    public async Task<ApiResponse<object, string>> Get(int pageIndex, int pageSize)
    {
        var response = await _client.GetAsync($"/Book?pageIndex={pageIndex}&pageSize={pageSize}");

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            return new ApiResponse<object, string>
            {
                HttpResponseMessage = response,
                Data = data
            };
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            return new ApiResponse<object, string>
            {
                HttpResponseMessage = response,
                ErrorData = errorContent
            };
        }
    }
}
