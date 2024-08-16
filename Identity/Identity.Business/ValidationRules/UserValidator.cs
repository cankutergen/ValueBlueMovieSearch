using FluentValidation;
using Identity.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Business.ValidationRules
{
    public class UserValidator : AbstractValidator<UserDocument>
    {
        public UserValidator()
        {
            RuleFor(x => x.Username)
                .NotNull()
                .WithMessage("Username can not be null")
                .NotEmpty()
                .WithMessage("Username can not be empty");

            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage("UserId can not be null");

            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage("Password can not be null")
                .NotEmpty()
                .WithMessage("Password can not be empty");

            RuleFor(x => x.Role.RoleName)
                .NotNull()
                .WithMessage("User role can not be null")
                .NotEmpty()
                .WithMessage("User role can not be empty");

            RuleFor(x => x.Role.Id)
                .NotNull()
                .WithMessage("User role id can not be null");
        }
    }
}
