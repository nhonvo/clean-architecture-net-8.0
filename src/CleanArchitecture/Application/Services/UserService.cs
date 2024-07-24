using System.Net.Http.Headers;
using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Common.Models.AuthIdentity.UsersIdentity;

namespace CleanArchitecture.Application.Services;

public class UserService(ApplicationDbContext context,
    IFileStorageService storageService,
    UserManager<ApplicationUser> userManager,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IUserService
{
    private readonly IFileStorageService _storageService = storageService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IMapper _mapper = mapper;

    public async Task<List<UserViewModel>> Get(CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.Avatar)
            .ToListAsync();

        var query = users.Select(u => new UserQueryResult
        {
            users = u,
            role = u.UserRoles.Select(ur => ur.Role.Name).ToList(),
            avatar = u.Avatar != null ? u.Avatar.PathMedia : null
        }).ToList();

        var data = _mapper.Map<List<UserViewModel>>(query);

        data.ForEach(x => x.Avatar = !string.IsNullOrEmpty(x.Avatar)
                        ? _storageService.GetFileUrl(x.Avatar)
                        : x.Avatar);
        return data;
    }

    public async Task Update(UserUpdateRequest request, CancellationToken cancellationToken)
    {

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.UserId)
            ?? throw AuthIdentityException.ThrowUserNotExist();

        user.Email = request.Email ?? user.Email;
        user.Name = request.Name ?? user.Name;

        //Lưu Avatar vào Host
        if (request.MediaFile != null)
        {
            var thumb = await _unitOfWork.MediaRepository.FirstOrDefaultAsync(i => i.MediaId == user.AvatarId);
            //Thêm mới Avatar nếu Tài khoản chưa có
            if (thumb == null)
            {
                user.Avatar = new Media()
                {
                    Caption = "Avatar User",
                    DateCreated = DateTime.Now,
                    FileSize = request.MediaFile.Length,
                    PathMedia = SaveFile(request.MediaFile, cancellationToken),
                    Type = MediaType.Image,
                    SortOrder = 1
                };
            }
            else
            {
                //Cập nhật Avatar
                if (thumb.PathMedia != null)
                {
                    _storageService.DeleteFile(thumb.PathMedia);
                }

                thumb.FileSize = request.MediaFile.Length;
                thumb.PathMedia = SaveFile(request.MediaFile, cancellationToken);

                _context.Media.Update(thumb);
            }
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            List<IdentityError> errorList = result.Errors.ToList();
            var errors = string.Join(", ", errorList.Select(e => e.Description));
            throw AuthIdentityException.ThrowUpdateUnsuccessful(errors);
        }
    }

    public async Task Delete(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw AuthIdentityException.ThrowAccountDoesNotExist();

        //Xoá Avatar ra khỏi Source
        var avatar = _context.Media.SingleOrDefault(x => x.MediaId == user.AvatarId);

        if (avatar != null)
        {
            if (avatar.PathMedia != null)
                _storageService.DeleteFile(avatar.PathMedia);
            _context.Media.Remove(avatar);
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            List<IdentityError> errorList = result.Errors.ToList();
            var errors = string.Join(", ", errorList.Select(e => e.Description));
            throw AuthIdentityException.ThrowDeleteUnsuccessful();
        }
    }


    //Lưu ảnh
    private string SaveFile(IFormFile file, CancellationToken cancellationToken)
    {
        var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
        _storageService.SaveFile(file.OpenReadStream(), fileName);
        return fileName;
    }
}
