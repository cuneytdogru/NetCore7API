using NetCore7API.Domain.Exceptions;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Repositories;
using NetCore7API.Domain.Results;
using NetCore7API.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Services
{
    public abstract class BaseService
    {
        protected readonly IUserRepository _userRepository;
        protected readonly ITokenService _tokenService;
        protected readonly IUnitOfWork _unitOfWork;

        protected BaseService(ITokenService tokenService, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        internal async Task<User> GetCurrentUser()
        {
            if (_tokenService.UserId is null)
                throw new UserUnauthorizedException("User is not authorized!");

            var user = await _userRepository.FindAsync(_tokenService.UserId.Value);

            if (user is null)
                throw new UserUnauthorizedException("User not found.");

            return user;
        }
    }
}