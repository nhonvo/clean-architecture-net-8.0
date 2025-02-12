using CleanArchitecture.Shared.Models.AuthIdentity.File;
using CleanArchitecture.Shared.Models.AuthIdentity.Media;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IFileService
{
    Task DeleteFileAsync(DeleteFileRequest request);
    Task<FileUploadResult> AddFileAsync(IFormFile file);
    string GetFileUrl(string fileName);
}
