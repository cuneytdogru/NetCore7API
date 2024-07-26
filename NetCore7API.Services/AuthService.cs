using AutoMapper;
using NetCore7API.Domain.DTOs.Auth;
using NetCore7API.Domain.DTOs.Post;
using NetCore7API.Domain.Errors;
using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Models.Interfaces;
using NetCore7API.Domain.Repositories;
using NetCore7API.Domain.Results;
using NetCore7API.Domain.Services;
using NetCore7API.Domain.Utils;

namespace NetCore7API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(
            ITokenRepository tokenRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork
            )
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IResult<LoginResponseDto>> Login(LoginRequestDto dto)
        {
            var user = await _userRepository.GetByUserNameAsync(dto.UserName);

            if (user == null)
                return Result<LoginResponseDto>.Failure(LoginErrors.InvalidUsernameOrPassword);

            if (!user.CheckPassword(dto.Password))
                return Result<LoginResponseDto>.Failure(LoginErrors.InvalidUsernameOrPassword);

            var existingToken = await _tokenRepository.GetActiveTokenFromUser(user.Id);

            if (existingToken != null)
                existingToken.Disable();

            var token = new Token(user);

            _tokenRepository.Add(token);

            await _unitOfWork.SaveChangesAsync();

            return Result<LoginResponseDto>.Success(new LoginResponseDto(token.Key));
        }

        public async Task<IResult> Logout(string key)
        {
            if (key.StartsWith("Bearer"))
                key = key.Substring(7).Trim();

            var existingToken = await _tokenRepository.GetActiveTokenFromKey(key);

            if (existingToken != null)
            {
                existingToken.Disable();

                _tokenRepository.Update(existingToken);

                await _unitOfWork.SaveChangesAsync();
            }

            return Result.Success();
        }
    }
}