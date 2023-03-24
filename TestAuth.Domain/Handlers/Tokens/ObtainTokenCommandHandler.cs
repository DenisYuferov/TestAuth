using MediatR;

using SharedCore.Domain.Abstraction.Providers;

using TestAuth.Domain.Abstraction.UnitOfWorks;
using TestAuth.Domain.Model.CQRS.Commands.Tokens;
using TestAuth.Domain.Model.CQRS.Dtos.Tokens;

namespace TestAuth.Domain.Handlers.Tokens
{
    public class ObtainTokenCommandHandler : IRequestHandler<ObtainTokenCommand, ObtainTokenDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityUserProvider _identityUserProvider;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRedisCacheProvider _cache;

        public ObtainTokenCommandHandler(
            IUnitOfWork unitOfWork,
            IIdentityUserProvider identityUserProvider,
            IJwtProvider jwtProvider,
            IRedisCacheProvider cache)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _identityUserProvider = identityUserProvider ?? throw new ArgumentNullException(nameof(identityUserProvider));
            _jwtProvider = jwtProvider ?? throw new ArgumentNullException(nameof(jwtProvider));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
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
                throw new Exception($"The password in not correct for user with e-mail {request.Email}");
            }

            var tokenDtoFromCache = await _cache.GetAsync<ObtainTokenDto>(user.Email!);
            if (_jwtProvider.ValidateAccessToken(tokenDtoFromCache?.AccessToken))
            {
                return tokenDtoFromCache!;
            }

            var claims = await _identityUserProvider.GetClaimsAsync(user);

            var tokenDto = new ObtainTokenDto
            {
                AccessToken = _jwtProvider.CreateAccessToken(claims),
                RefreshToken = _jwtProvider.CreateRefreshToken()
            };

            await _cache.SetAsync(user.Email, tokenDto);

            return tokenDto;
        }
    }
}
