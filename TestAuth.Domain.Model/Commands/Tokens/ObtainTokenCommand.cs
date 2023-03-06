using MediatR;
using TestAuth.Domain.Model.Dtos.Tokens;

namespace TestAuth.Domain.Model.Commands.Tokens
{
    public class ObtainTokenCommand : IRequest<ObtainTokenDto>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
