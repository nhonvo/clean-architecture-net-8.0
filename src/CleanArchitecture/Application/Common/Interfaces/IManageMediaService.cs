using CleanArchitecture.Application.Common.Models.AuthIdentity.Media;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IManageMediaService
{
    Task<int> RemoveMedia(int mediaId);
    Task<int> UpdateMedia(int mediaId, NewsMediaCreateRequest request);
}
