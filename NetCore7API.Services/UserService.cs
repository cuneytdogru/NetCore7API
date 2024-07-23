using AutoMapper;
using NetCore7API.Domain.DTOs.User;
using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Repositories;
using NetCore7API.Domain.Services;
using NetCore7API.Domain.Providers;

namespace NetCore7API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserProvider _userProvider;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUserRepository userRepository,
            IUserProvider userProvider,
            ITokenService tokenService,
            IUnitOfWork unitOfWork
            )
        {
            _userRepository = userRepository;
            _userProvider = userProvider;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> RegisterAsync(RegisterUserRequestDto dto)
        {
            var isUserNameExists = await _userProvider.IsUserNameInUse(dto.UserName);
            if (isUserNameExists)
                throw new UserException("Username already in use! Please type another username.");

            var isEmailExists = await _userProvider.IsEmailInUse(dto.Email);
            if (isEmailExists)
                throw new UserException("Email address already in use! Please type another email.");

            var user = new User(dto.UserName, dto.Email, dto.FullName, dto.Password);

            _userRepository.Add(user);

            await _unitOfWork.SaveChangesAsync();

            return user.Id;
        }

        public async Task UpdateAsync(Guid id, UpdateUserRequestDto dto)
        {
            if (_tokenService.UserId != id)
                throw new UserException("User not found.");

            var user = await _userRepository.FindAsync(id);

            if (user is null)
                throw new UserException("User not found.");

            var isUserNameExists = await _userProvider.IsUserNameInUse(dto.UserName, id);
            if (isUserNameExists)
                throw new UserException("Username already in use! Please type another username.");

            user.Update(dto);

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(Guid id, ChangePasswordRequestDto dto)
        {
            if (_tokenService.UserId != id)
                throw new UserException("User not found.");

            var user = await _userRepository.FindAsync(id);

            if (user is null)
                throw new UserException("User not found.");

            user.ChangePassword(dto);

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            if (_tokenService.UserId != id)
                throw new UserException("User not found.");

            var user = await _userRepository.FindAsync(id);

            if (user is null)
                throw new Exception("User not found.");

            _userRepository.SoftDelete(user);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}