using System.Security.Claims;

using MediatR;

using Microsoft.IdentityModel.JsonWebTokens;

using SharedCore.Domain.Abstraction.Providers;

using TestAuth.Domain.Abstraction.UnitOfWorks;
using TestAuth.Domain.Model.CQRS.Commands.Tokens;
using TestAuth.Domain.Model.CQRS.Dtos.Tokens;

namespace TestAuth.Domain.Handlers.Tokens
{
    public class RenewTokenCommandHandler : IRequestHandler<RenewTokenCommand, RenewTokenDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtGenerator;

        public RenewTokenCommandHandler(
            IUnitOfWork unitOfWork,
            IJwtProvider jwtGenerator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _jwtGenerator = jwtGenerator ?? throw new ArgumentNullException(nameof(jwtGenerator));
        }
        public async Task<RenewTokenDto> Handle(RenewTokenCommand request, CancellationToken cancellation)
        {
            var user = await _unitOfWork.UserManager.FindByEmailAsync(request.Email!);
            if (user == null)
            {
                throw new Exception($"The user has not been found with e-mail {request.Email}");
            }

            // TODO: Add token cache check
            // TODO: Validate if refresh token expired

            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
            };

            var userRoles = await _unitOfWork.UserManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var renewTokenDto = new RenewTokenDto
            {
                AccessToken = _jwtGenerator.CreateAccessToken(claims),
                RefreshToken = _jwtGenerator.CreateRefreshToken()
            };

            // TODO: Store token to cache

            return renewTokenDto;
        }
    }
}
