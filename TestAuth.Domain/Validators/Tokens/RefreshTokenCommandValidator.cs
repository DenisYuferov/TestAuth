using FluentValidation;
using TestAuth.Domain.Model.Commands.Tokens;

namespace TestAuth.Domain.Validators.Tokens
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(t => t.Email).NotNull().NotEmpty().EmailAddress();

            RuleFor(t => t.RefreshToken).NotNull().NotEmpty();
        }
    }
}
