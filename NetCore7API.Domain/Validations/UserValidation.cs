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
        public UserValidation()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .MinimumLength(MinLength.Username)
                .MaximumLength(MaxLength.Username);

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(MaxLength.Default)
                .EmailAddress();

            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(MaxLength.Default);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(MinLength.Password)
                .MaximumLength(MaxLength.Password);
        }
    }
}