using AutoMapper;
using NetCore7API.Domain.DTOs.User;
using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Filters;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Repositories;
using NetCore7API.Domain.Extensions;
using NetCore7API.Domain.DTOs;

namespace NetCore7API.Services
{
    public class UserService : Domain.Services.IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            var entity = await _userRepository.GetAsync(id);

            if (entity == null)
                throw new UserException("User not found.");

            return _mapper.Map<UserDto>(entity);
        }

        public async Task<UserDto> RegisterAsync(RegisterUserDto dto)
        {
            var entity = new User(dto.UserName, dto.Email, dto.Password, dto.FullName);

            var isUserNameExists = await _userRepository.IsUserNameInUse(dto.UserName);
            if (isUserNameExists)
                throw new UserException("Username already in use! Please type another username.");

            var isEmailExists = await _userRepository.IsEmailInUse(dto.Email);
            if (isEmailExists)
                throw new UserException("Email address already in use! Please type another email.");

            _userRepository.Add(entity);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(entity);
        }

        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto dto)
        {
            var isUserNameExists = await _userRepository.IsUserNameInUse(dto.UserName, id);
            if (isUserNameExists)
                throw new UserException("Username already in use! Please type another username.");

            var entity = await _userRepository.FindAsync(id);

            if (entity == null)
                throw new UserException("User not found.");

            entity.Update(dto);

            _userRepository.Update(entity);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(entity);
        }

        public async Task<UserDto> ChangePasswordAsync(Guid id, ChangePasswordDto dto)
        {
            var entity = await _userRepository.FindAsync(id);

            if (entity == null)
                throw new UserException("User not found.");

            entity.ChangePassword(dto);

            _userRepository.Update(entity);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(entity);
        }

        public async Task<UserDto> DeleteAsync(Guid id)
        {
            var entity = await _userRepository.FindAsync(id);

            if (entity == null)
                throw new Exception("User not found.");

            _userRepository.SoftDelete(entity);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(entity);
        }
    }
}