using CleanArchitecture.Shared.Models.AuthIdentity.UsersIdentity;
using FluentValidation;

namespace CleanArchitecture.Web.Validations;

public class SendPasswordResetCodeRequestValidation : AbstractValidator<SendPasswordResetCodeRequest>
{
    public SendPasswordResetCodeRequestValidation()
    {

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
              .EmailAddress().WithMessage("Email must be a valid email address.");

    }
}
