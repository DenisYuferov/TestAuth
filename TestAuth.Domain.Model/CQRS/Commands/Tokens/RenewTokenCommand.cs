using MediatR;
using TestAuth.Domain.Model.CQRS.Dtos.Tokens;

namespace TestAuth.Domain.Model.CQRS.Commands.Tokens
{
    public class RenewTokenCommand : IRequest<RenewTokenDto>
    {
        public string? Email { get; set; }
        public string? RefreshToken { get; set; }
    }
}
