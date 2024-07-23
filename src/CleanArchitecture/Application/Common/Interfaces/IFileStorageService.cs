namespace CleanArchitecture.Application.Common.Interfaces;

public interface IFileStorageService
{
    void DeleteFile(string fileName);
    string GetFileUrl(string fileName);
    string GetNewsUrl(string alias);
    void SaveFile(Stream mediaBinaryStream, string fileName);
}
