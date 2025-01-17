using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Shared.Models.AuthIdentity.File;
using CleanArchitecture.Shared.Models.AuthIdentity.Media;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.Web.Controller;

public class FileStorageController(IFileService fileStorageService) : BaseController
{
    private readonly IFileService _fileStorageService = fileStorageService;

    /// <summary>
    /// upload a static file
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    [HttpPost("upload")]
    [SwaggerResponse(200, "File uploaded successfully.", typeof(FileUploadResult))]
    [SwaggerResponse(400, "Invalid file format.")]
    public async Task<IActionResult> UploadFile(IFormFile file) 
        => Ok(await _fileStorageService.AddFileAsync(file));

    /// <summary>
    /// delete a file
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpDelete]
    [SwaggerResponse(204, "File deleted successfully.")]
    [SwaggerResponse(404, "File not found.")]
    public IActionResult DeleteFile(DeleteFileRequest request)
    {
        _fileStorageService.DeleteFileAsync(request);
        return NoContent();
    }

    /// <summary>
    /// get a file by file name
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("{fileName}")]
    [SwaggerResponse(200, "File URL retrieved successfully.", typeof(string))]
    [SwaggerResponse(404, "File not found.")]
    public IActionResult GetFileUrl(string fileName)
        => Ok(_fileStorageService.GetFileUrl(fileName));
}
