using AutoMapper;
using NetCore7API.Domain.DTOs.User;
using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Repositories;
using NetCore7API.Domain.Extensions;
using NetCore7API.Domain.DTOs;
using NetCore7API.Domain.Services;
using NetCore7API.Domain.Providers;

namespace NetCore7API.Services
{
    public class UserService : Domain.Services.IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserProvider _userProvider;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IUserProvider userProvider,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _userRepository = userRepository;
            _userProvider = userProvider;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);

            if (user is null)
                throw new UserException("User not found.");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> RegisterAsync(RegisterUserDto dto)
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

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto dto)
        {
            var isUserNameExists = await _userProvider.IsUserNameInUse(dto.UserName, id);
            if (isUserNameExists)
                throw new UserException("Username already in use! Please type another username.");

            var user = await _userRepository.FindAsync(id);

            if (user is null)
                throw new UserException("User not found.");

            user.Update(dto);

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> ChangePasswordAsync(Guid id, ChangePasswordDto dto)
        {
            var user = await _userRepository.FindAsync(id);

            if (user is null)
                throw new UserException("User not found.");

            user.ChangePassword(dto);

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> DeleteAsync(Guid id)
        {
            var user = await _userRepository.FindAsync(id);

            if (user is null)
                throw new Exception("User not found.");

            _userRepository.SoftDelete(user);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }
    }
}