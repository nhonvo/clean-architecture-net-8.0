using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Shared.Models.AuthIdentity.File;
using CleanArchitecture.Shared.Models.AuthIdentity.Media;

namespace CleanArchitecture.Application.Services;

public class MediaService(
    IFileService storageService,
    ILoggerFactory logger,
    IUnitOfWork unitOfWork) : IMediaService
{
    private readonly IFileService _storageService = storageService;
    private readonly ILogger _logger = logger.CreateLogger<MediaService>();
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    // Xóa media
    public async Task RemoveMediaAsync(int mediaId, CancellationToken cancellationToken)
    {
        var media = await _unitOfWork.MediaRepository.FirstOrDefaultAsync(x => x.MediaId == mediaId);
        if (media == null)
        {
            _logger.LogWarning($"Cannot find media with id {mediaId}");
            throw new KeyNotFoundException($"Cannot find media with id {mediaId}");
        }

        if (!string.IsNullOrEmpty(media.PathMedia))
        {
            await _storageService.DeleteFileAsync(new DeleteFileRequest { FileName = media.PathMedia });
            _logger.LogInformation($"File {media.PathMedia} has been deleted.");
        }
        await _unitOfWork.ExecuteTransactionAsync(() =>
            _unitOfWork.MediaRepository.Delete(media), cancellationToken
        );
        _logger.LogInformation($"Media with id {mediaId} has been removed.");
    }

    // Cập nhật media
    public async Task UpdateMediaAsync(int mediaId, MediaCreateRequest request, CancellationToken cancellationToken)
    {
        var media = await _unitOfWork.MediaRepository.FirstOrDefaultAsync(x => x.MediaId == mediaId);
        if (media == null)
        {
            _logger.LogWarning($"Cannot find media with id {mediaId}");
            throw new KeyNotFoundException($"Cannot find media with id {mediaId}");
        }
        var pathMedia = await _storageService.AddFileAsync(request.MediaFile);

        if (request.MediaFile != null)
        {
            media.PathMedia = pathMedia.Path;
            media.FileSize = request.MediaFile.Length;
        }
        await _unitOfWork.ExecuteTransactionAsync(() =>
           _unitOfWork.MediaRepository.Update(media), cancellationToken
       );

        _logger.LogInformation($"Media with id {mediaId} has been updated.");
    }
}
