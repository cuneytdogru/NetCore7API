using AutoMapper;
using NetCore7API.Domain.DTOs.User;
using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Repositories;
using NetCore7API.Domain.Services;
using NetCore7API.Domain.Providers;
using FluentValidation;
using System;
using NetCore7API.Domain.Results;
using NetCore7API.Domain.Errors;

namespace NetCore7API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserProvider _userProvider;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<User> _validator;

        public UserService(
            IUserRepository userRepository,
            IUserProvider userProvider,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IValidator<User> validator
            )
        {
            _userRepository = userRepository;
            _userProvider = userProvider;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<IResult<Guid>> RegisterAsync(RegisterUserRequestDto dto)
        {
            var user = new User(dto.UserName, dto.Email, dto.FullName, dto.Password);

            var validationResult = await _validator.ValidateAsync(user);

            if (!validationResult.IsValid)
                return Result<Guid>.Failure(Error.ValidationErrors(validationResult.ToDictionary()));

            _userRepository.Add(user);

            await _unitOfWork.SaveChangesAsync();

            return Result<Guid>.Success(user.Id);
        }

        public async Task<IResult> UpdateAsync(Guid id, UpdateUserRequestDto dto)
        {
            if (_tokenService.UserId != id)
                return Result.Failure(Error.NotFound());

            var user = await _userRepository.FindAsync(id);

            if (user is null)
                return Result.Failure(Error.NotFound());

            user.Update(dto);

            var validationResult = await _validator.ValidateAsync(user);

            if (!validationResult.IsValid)
                return Result.Failure(Error.ValidationErrors(validationResult.ToDictionary()));

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<IResult> ChangePasswordAsync(Guid id, ChangePasswordRequestDto dto)
        {
            if (_tokenService.UserId != id)
                return Result.Failure(Error.NotFound());

            var user = await _userRepository.FindAsync(id);

            if (user is null)
                return Result.Failure(Error.NotFound());

            user.ChangePassword(dto);

            var validationResult = await _validator.ValidateAsync(user);

            if (!validationResult.IsValid)
                return Result.Failure(Error.ValidationErrors(validationResult.ToDictionary()));

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            if (_tokenService.UserId != id)
                return Result.Failure(Error.NotFound());

            var user = await _userRepository.FindAsync(id);

            if (user is null)
                return Result.Failure(Error.NotFound());

            _userRepository.SoftDelete(user);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}