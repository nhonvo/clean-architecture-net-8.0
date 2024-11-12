using CleanArchitecture.Shared.Models.AuthIdentity.UsersIdentity;
using FluentValidation;

namespace CleanArchitecture.Web.Validations;

public class ResetPasswordRequestValidation : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidation()
    {

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
              .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.OTP)
           .NotEmpty().WithMessage("OTP is required.")
           .MaximumLength(6).WithMessage("OTP must not exceed 6 characters.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("NewPassword is required.")
            .MinimumLength(8).WithMessage("NewPassword must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("NewPassword must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("NewPassword must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("NewPassword must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("NewPassword must contain at least one special character.");
    }
}
