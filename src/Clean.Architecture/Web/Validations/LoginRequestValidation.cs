using Clean.Architecture.Application.Common.Models.User;
using FluentValidation;

namespace Clean.Architecture.Web.Validations
{
    public class LoginRequestValidation : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidation()
        {
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(100);
        }
    }
}
