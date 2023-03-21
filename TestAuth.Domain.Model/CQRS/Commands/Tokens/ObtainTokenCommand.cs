using MediatR;
using TestAuth.Domain.Model.CQRS.Dtos.Tokens;

namespace TestAuth.Domain.Model.CQRS.Commands.Tokens
{
    public class ObtainTokenCommand : IRequest<ObtainTokenDto>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
