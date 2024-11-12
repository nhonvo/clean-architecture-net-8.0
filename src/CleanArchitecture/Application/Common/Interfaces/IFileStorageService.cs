using CleanArchitecture.Shared.Models.AuthIdentity.File;
using CleanArchitecture.Shared.Models.AuthIdentity.Media;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IFileStorageService
{
    Task DeleteFileAsync(DeleteFileRequest request);
    Task<FileUploadResult> AddFileAsync(IFormFile file);
    string GetFileUrl(AddFileRequest request);
}
