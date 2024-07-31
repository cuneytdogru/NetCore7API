using FluentValidation;
using NetCore7API.Domain.Models.Constants.DataAnnotations;
using NetCore7API.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore7API.Domain.Validations
{
    public class CommentValidation : AbstractValidator<Comment>
    {
        public CommentValidation()
        {
            RuleFor(x => x.Text)
                .NotEmpty()
                .MaximumLength(MaxLength.Text);
        }
    }
}