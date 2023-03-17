using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TestAuth.Domain.Model.Commands.Tokens;

namespace TestAuth.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TokensController : ControllerBase
    {
        private readonly ISender _sender;

        public TokensController(ISender sender)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        [HttpGet("TestAuthentication")]
        public ActionResult TestAuthentication()
        {
            return Ok("Authenticated!");
        }

        [AllowAnonymous]
        [HttpPost("obtain")]
        public async Task<ActionResult> ObtainAsync(ObtainTokenCommand command, CancellationToken cancellation)
        {
            var result = await _sender.Send(command, cancellation);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshAsync(RefreshTokenCommand command, CancellationToken cancellation)
        {
            var result = await _sender.Send(command, cancellation);

            return Ok(result);
        }
    }
}
