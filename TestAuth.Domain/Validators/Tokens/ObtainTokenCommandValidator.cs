using FluentValidation;
using TestAuth.Domain.Model.CQRS.Commands.Tokens;

namespace TestAuth.Domain.Validators.Tokens
{
    public class ObtainTokenCommandValidator : AbstractValidator<ObtainTokenCommand>
    {
        public ObtainTokenCommandValidator()
        {
            RuleFor(t => t.Email).NotNull().NotEmpty().EmailAddress();

            RuleFor(t => t.Password).NotNull().NotEmpty();
        }
    }
}
