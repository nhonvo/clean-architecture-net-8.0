using CleanArchitecture.Application.Common.Models.User;
using FluentValidation;

namespace CleanArchitecture.Web.Validations
{
    public class LoginRequestValidation : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidation()
        {
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(100);
        }
    }
}
