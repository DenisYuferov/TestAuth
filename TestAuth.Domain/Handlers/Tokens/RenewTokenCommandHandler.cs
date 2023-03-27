using MassTransit;

using MediatR;

using SharedCore.Domain.Abstraction.Providers;
using SharedCore.Model.Exceptions;
using SharedCore.Model.Tokens;

using TestAuth.Domain.Abstraction.UnitOfWorks;
using TestAuth.Domain.Model.CQRS.Commands.Tokens;
using TestAuth.Domain.Model.CQRS.Dtos.Tokens;

namespace TestAuth.Domain.Handlers.Tokens
{
    public class RenewTokenCommandHandler : IRequestHandler<RenewTokenCommand, RenewTokenDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityUserProvider _identityUserProvider;
        private readonly IRedisCacheProvider _cache;
        private readonly IJwtProvider _jwtProvider;
        private readonly IBus _bus;

        public RenewTokenCommandHandler(
            IUnitOfWork unitOfWork,
            IIdentityUserProvider identityUserProvider,
            IRedisCacheProvider cache,
            IJwtProvider jwtProvider,
            IBus bus)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _identityUserProvider = identityUserProvider ?? throw new ArgumentNullException(nameof(identityUserProvider));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _jwtProvider = jwtProvider ?? throw new ArgumentNullException(nameof(jwtProvider));
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }
        public async Task<RenewTokenDto> Handle(RenewTokenCommand request, CancellationToken cancellation)
        {
            var user = await _unitOfWork.UserManager.FindByEmailAsync(request.Email!);
            if (user == null)
            {
                throw new NotFoundAppException($"The user has not been found with e-mail {request.Email}");
            }

            var tokenDtoFromCache = await _cache.GetAsync<RenewTokenDto>(user.Email!);
            if (_jwtProvider.CanUseTokens(tokenDtoFromCache?.AccessToken, tokenDtoFromCache?.RefreshToken, request.RefreshToken))
            {
                return tokenDtoFromCache!;
            }

            var claims = await _identityUserProvider.GetClaimsAsync(user);

            var tokenDto = new RenewTokenDto
            {
                AccessToken = _jwtProvider.CreateAccessToken(claims),
                RefreshToken = _jwtProvider.CreateRefreshToken()
            };

            await _cache.SetAsync(user.Email!, tokenDto);

            await _bus.Publish(tokenDto.RefreshToken);

            return tokenDto;
        }
    }
}
