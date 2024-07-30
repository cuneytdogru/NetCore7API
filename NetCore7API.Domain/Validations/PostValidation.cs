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
    public class PostValidation : AbstractValidator<Post>
    {
        public PostValidation()
        {
            RuleFor(x => x.Text)
                .NotEmpty()
                .MaximumLength(MaxLength.Text);

            RuleFor(x => x.ImageURL)
                .NotEmpty();
        }
    }
}