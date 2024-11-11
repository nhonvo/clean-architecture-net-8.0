namespace CleanArchitecture.IntegrationTest.Models;

public class ApiResponse<T, E>
{
    public HttpResponseMessage HttpResponseMessage { get; set; }
    public T Data { get; set; }
    public E ErrorData { get; set; }
}

public class ApiResponse<T>
{
    public HttpResponseMessage HttpResponseMessage { get; set; }
    public T Data { get; set; }
}