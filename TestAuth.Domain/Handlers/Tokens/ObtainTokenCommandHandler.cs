﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using MediatR;

using Microsoft.Extensions.Logging;

using TestAuth.Domain.Abstraction.Providers;
using TestAuth.Domain.Abstraction.UnitOfWorks;
using TestAuth.Domain.Model.Commands.Tokens;
using TestAuth.Domain.Model.Dtos.Tokens;

namespace TestAuth.Domain.Handlers.Tokens
{
    public class ObtainTokenCommandHandler : IRequestHandler<ObtainTokenCommand, ObtainTokenDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtGenerator;
        private readonly ILogger _logger;

        public ObtainTokenCommandHandler(
            IUnitOfWork unitOfWork,
            IJwtProvider jwtGenerator,
            ILogger<ObtainTokenCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _jwtGenerator = jwtGenerator ?? throw new ArgumentNullException(nameof(jwtGenerator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<ObtainTokenDto> Handle(ObtainTokenCommand request, CancellationToken cancellation)
        {
            var user = await _unitOfWork.UserManager.FindByEmailAsync(request.Email!);
            if (user == null)
            {
                throw new Exception($"The user has not been found with e-mail {request.Email}");
            }

            var pwdCheck = await _unitOfWork.SignInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!pwdCheck.Succeeded)
            {
                throw new Exception($"The password in not correct fo user with e-mail {request.Email}");
            }

            // TODO: Add token cache check

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

            var obtainTokenDto = new ObtainTokenDto
            {
                AccessToken = _jwtGenerator.CreateAccessToken(claims),
                RefreshToken = _jwtGenerator.CreateRefreshToken()
            };

            // TODO: Store token to cache

            return obtainTokenDto;
        }
    }
}
