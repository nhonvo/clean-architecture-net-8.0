using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interface;

namespace CleanArchitecture.Application.Repositories;

public class ForgotPasswordRepository(ApplicationDbContext context) : GenericRepository<ForgotPassword>(context), IForgotPasswordRepository { }
