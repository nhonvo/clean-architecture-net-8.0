using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models.AuthIdentity.File;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Controller;

public class FileStorageController(IFileStorageService fileStorageService) : BaseController
{
    private readonly IFileStorageService _fileStorageService = fileStorageService;

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        return Ok(await _fileStorageService.AddFileAsync(file));
    }

    [HttpDelete]
    public IActionResult DeleteFile(DeleteFileRequest request)
    {
        _fileStorageService.DeleteFileAsync(request);
        return NoContent();
    }

    [HttpPost]
    public IActionResult GetFileUrl(AddFileRequest request)
        => Ok(_fileStorageService.GetFileUrl(request));
}
