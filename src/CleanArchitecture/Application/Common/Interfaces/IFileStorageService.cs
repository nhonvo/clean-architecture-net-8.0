using CleanArchitecture.Application.Common.Models.AuthIdentity.File;
using CleanArchitecture.Application.Common.Models.AuthIdentity.Media;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IFileStorageService
{
    Task DeleteFileAsync(DeleteFileRequest request);
    Task<FileUploadResult> AddFileAsync(IFormFile file);
    string GetFileUrl(AddFileRequest request);
}
