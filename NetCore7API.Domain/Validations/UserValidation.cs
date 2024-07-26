using FluentValidation;
using NetCore7API.Domain.Errors;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Models.Constants.DataAnnotations;
using NetCore7API.Domain.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Validations
{
    public class UserValidation : AbstractValidator<User>
    {
        private readonly IUserProvider _userProvider;

        public UserValidation(IUserProvider userProvider)
        {
            _userProvider = userProvider;

            RuleFor(x => x.UserName)
                .NotEmpty()
                .MinimumLength(MinLength.Username)
                .MaximumLength(MaxLength.Username)
                .MustAsync((user, userName, token) => IsUsernameAvailable(userName, user.Id))
                .WithMessage(UserErrors.UsernameInUse.Message);

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(MaxLength.Default)
                .EmailAddress()
                .MustAsync((user, email, token) => IsEmailAvailable(email, user.Id))
                .WithMessage(UserErrors.EmailInUse.Message);

            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(MaxLength.Default);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(MinLength.Password)
                .MaximumLength(MaxLength.Password);
        }

        private async Task<bool> IsUsernameAvailable(string userName, Guid userId)
        {
            return !await _userProvider.IsUserNameInUse(userName, userId);
        }

        private async Task<bool> IsEmailAvailable(string email, Guid userId)
        {
            return !await _userProvider.IsEmailInUse(email, userId);
        }
    }
}