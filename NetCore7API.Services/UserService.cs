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
    public class UserService : BaseService, IUserService
    {
        private readonly IUserProvider _userProvider;
        private readonly IValidator<User> _validator;

        public UserService(
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IUserProvider userProvider,
            IValidator<User> validator
            ) : base(tokenService, unitOfWork, userRepository)
        {
            _validator = validator;
            _userProvider = userProvider;
        }

        public async Task<IResult<Guid>> RegisterAsync(RegisterUserRequestDto dto)
        {
            if (await _userProvider.IsEmailInUse(dto.Email))
                return Result<Guid>.Failure(UserErrors.EmailInUse);

            if (await _userProvider.IsUserNameInUse(dto.UserName))
                return Result<Guid>.Failure(UserErrors.UsernameInUse);

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
            var currentUser = await GetCurrentUser();

            var user = await _userRepository.FindAsync(id);

            if (user is null)
                return Result.Failure(Error.NotFound());

            user.Update(dto, currentUser);

            var validationResult = await _validator.ValidateAsync(user);

            if (!validationResult.IsValid)
                return Result.Failure(Error.ValidationErrors(validationResult.ToDictionary()));

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<IResult> ChangePasswordAsync(Guid id, ChangePasswordRequestDto dto)
        {
            var currentUser = await GetCurrentUser();

            var user = await _userRepository.FindAsync(id);

            if (user is null)
                return Result.Failure(Error.NotFound());

            user.ChangePassword(dto, currentUser);

            var validationResult = await _validator.ValidateAsync(user);

            if (!validationResult.IsValid)
                return Result.Failure(Error.ValidationErrors(validationResult.ToDictionary()));

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var currentUser = await GetCurrentUser();

            var user = await _userRepository.FindAsync(id);

            if (user is null)
                return Result.Failure(Error.NotFound());

            user.Delete(currentUser);

            _userRepository.SoftDelete(user);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}