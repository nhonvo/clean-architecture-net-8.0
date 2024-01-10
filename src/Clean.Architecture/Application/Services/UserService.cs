using System.Text.Json;
using System.Transactions;
using AutoMapper;
using Clean.Architecture.Application.Common;
using Clean.Architecture.Application.Common.Interfaces;
using Clean.Architecture.Application.Common.Models.User;
using Clean.Architecture.Application.Common.Utilities;

namespace Clean.Architecture.Application.Services
{
    public class UserService(IUnitOfWork unitOfWork,
                             IMapper mapper,
                             ICurrentTime currentTime,
                             AppSettings configuration,
                             ILoggerFactory logger) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentTime _currentTime = currentTime;
        private readonly AppSettings _configuration = configuration;
        private readonly ILogger _logger = logger.CreateLogger<UserService>();

        public async Task<UserDTO> Authenticate(LoginRequest request)
        {
            _logger.LogInformation("Request: " + JsonSerializer.Serialize(request));

            var isUserExist = await _unitOfWork.UserRepository.AnyAsync(x => x.UserName == request.UserName);
            if (!isUserExist)
            {
                throw new Exception("User does not exist!");
            }

            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.UserName == request.UserName);

            if (!StringHelper.Verify(request.Password, user.Password))
            {
                throw new Exception("Password Incorrect!");
            }

            var token = user.Authenticate(
                _configuration.Jwt.Issuer,
                _configuration.Jwt.Audience,
                _configuration.Jwt.Key,
                _currentTime);
            var response = _mapper.Map<UserDTO>(user);
            response.Token = token;

            _logger.LogInformation("Response: " + JsonSerializer.Serialize(response));

            return response;
        }

        public async Task<UserDTO> Register(RegisterRequest request)
        {
            _logger.LogInformation("Request: " + JsonSerializer.Serialize(request));

            var isUserExist = await _unitOfWork.UserRepository.AnyAsync(x => x.UserName == request.UserName);
            if (isUserExist)
                throw new Exception("This Username Already Used!");

            var isEmailExist = await _unitOfWork.UserRepository.AnyAsync(x => x.UserName == request.Email);
            if (isEmailExist)
                throw new Exception("This Email Already Used");


            var user = _mapper.Map<User>(request);
            user.Password = user.Password.Hash();
            await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.UserRepository.AddAsync(user));

            var response = _mapper.Map<UserDTO>(user);
            _logger.LogInformation("Response: " + JsonSerializer.Serialize(response));

            return response;
        }
    }
}
