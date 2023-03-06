using MediatR;
using TestAuth.Domain.Model.Dtos.Tokens;

namespace TestAuth.Domain.Model.Commands.Tokens
{
    public class RefreshTokenCommand : IRequest<ObtainTokenDto>
    {
        public string? Email { get; set; }
        public string? RefreshToken { get; set; }
    }
}
