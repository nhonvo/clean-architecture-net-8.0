namespace CleanArchitecture.Application.Common.Interfaces;

public interface ICookieService
{
    public void Set(string token);
    public string Get();
    public void Delete();
}
