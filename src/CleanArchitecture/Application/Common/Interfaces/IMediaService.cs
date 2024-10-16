using CleanArchitecture.Application.Common.Models.AuthIdentity.Media;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IMediaService
{
    Task RemoveMediaAsync(int mediaId, CancellationToken cancellationToken);
    Task UpdateMediaAsync(int mediaId, MediaCreateRequest request, CancellationToken cancellationToken);
}
