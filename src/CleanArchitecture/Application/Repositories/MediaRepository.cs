using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interface;

namespace CleanArchitecture.Application.Repositories;

public class MediaRepository(ApplicationDbContext context) : GenericRepository<Media>(context), IMediaRepository { }
