using CleanArchitecture.IntegrationTest.Models;

namespace CleanArchitecture.IntegrationTest.Shared.Interface;

public interface IBookClient
{
    Task<ApiResponse<object, string>> Get(string id);
    Task<ApiResponse<object, string>> Get(int pageIndex, int pageSize);
}