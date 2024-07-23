using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.Services;

public class FileStorageService(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : IFileStorageService
{
    public static string USER_CONTENT_FOLDER_NAME { get; set; } = "images";

    private string _userContentFolder = "";

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    public void DeleteFile(string fileName)
    {
        _userContentFolder = Path.Combine(_webHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME);

        var filePath = Path.Combine(_userContentFolder, fileName);
        if (File.Exists(filePath))
        {
            Task.Run(() => File.Delete(filePath));
        }
    }

    public void SaveFile(Stream mediaBinaryStream, string fileName)
    {
        _userContentFolder = Path.Combine(_webHostEnvironment.WebRootPath, USER_CONTENT_FOLDER_NAME);

        var filePath = Path.Combine(_userContentFolder, fileName);
        using var output = new FileStream(filePath, FileMode.Create);
        mediaBinaryStream.CopyTo(output);
    }

    //Hàm lấy ra URL hình ảnh (trường hợp ảnh được tạo bởi hệ thống)
    public string GetFileUrl(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return null;

        return _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value + $"/{USER_CONTENT_FOLDER_NAME}/{fileName}";
    }

    //Hàm lấy ra URL của tin tức (trong trường hợp tin được tạo bởi hệ thống)
    public string GetNewsUrl(string alias)
    {
        // return _httpContextAccessor.HttpContext.Request.Scheme + "://" + "localhost:5003" + $"/news/{alias}";
        return $"https://localhost:5003/news/{alias}";
    }
}
