using FluentValidation;
using TestAuth.Domain.Model.CQRS.Commands.Tokens;

namespace TestAuth.Domain.Validators.Tokens
{
    public class RefreshTokenCommandValidator : AbstractValidator<RenewTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(t => t.Email).NotNull().NotEmpty().EmailAddress();

            RuleFor(t => t.RefreshToken).NotNull().NotEmpty();
        }
    }
}
