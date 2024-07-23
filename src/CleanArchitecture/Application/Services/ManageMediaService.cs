using System.Net.Http.Headers;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models.AuthIdentity.Media;
using CleanArchitecture.Infrastructure.Data;

namespace CleanArchitecture.Application.Services;

public class ManageMediaService(ApplicationDbContext context, IFileStorageService storageService) : IManageMediaService
{
    private readonly ApplicationDbContext _context = context;

    private readonly IFileStorageService _storageService = storageService;

    //Xóa media
    public async Task<int> RemoveMedia(int mediaId)
    {
        var media = await _context.Media.FindAsync(mediaId);
        if (media == null)
            throw new Exception($"CannotFindMediaWithId {mediaId}");

        if (media.PathMedia != null)
            _storageService.DeleteFile(media.PathMedia);

        _context.Media.Remove(media);

        return await _context.SaveChangesAsync();
    }
    //Cập nhật media
    public async Task<int> UpdateMedia(int mediaId, NewsMediaCreateRequest request)
    {
        var media = await _context.Media.FindAsync(mediaId);
        if (media == null)
            throw new Exception($"CannotFindMediaWithId {mediaId}");

        if (request.MediaFile != null)
        {
            media.PathMedia = this.SaveFile(request.MediaFile);
            media.FileSize = request.MediaFile.Length;
        }

        _context.Media.Update(media);

        return await _context.SaveChangesAsync();
    }

    private string SaveFile(IFormFile file)
    {
        var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
        _storageService.SaveFile(file.OpenReadStream(), fileName);
        return fileName;
    }
}
