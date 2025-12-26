using FlowerBot.src.Core.Handlers.Authorization;
using FlowerBot.src.Data.Models.Common;
using FlowerBot.src.Data.Models.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowerBot.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator) =>
            _mediator = mediator;

        [HttpPost("login")]
        public async Task<ActionResult<string?>> Login(
            [FromBody] string? InitDataRaw,
            CancellationToken cancellationToken)
        {
            AuthorizationUserCommand query = new AuthorizationUserCommand(InitDataRaw);
            TokensResult? token = await _mediator.Send(query);

            if (token == null) 
                return BadRequest("Invalid Data");

            Response.Cookies.Append("refreshToken", token.RefreshToken.ToString()!, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7),
                Path = "/api/auth"
            });

            return Ok(token.AccessToken);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<string?>> Refresh(
            CancellationToken cancellationToken)
        {
            string? tokenFromCookie = Request.Cookies["refreshToken"];
            Console.WriteLine(tokenFromCookie);
            if (string.IsNullOrEmpty(tokenFromCookie) || !Guid.TryParse(tokenFromCookie, out var tokenId))
                return Unauthorized();

            RefreshUserSessionCommand query = new RefreshUserSessionCommand(tokenId);
            string? token = await _mediator.Send(query);

            if (token == null) 
                return Unauthorized();

            return Ok(token);
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout(
            CancellationToken cancellationToken)
        {
            string? tokenFromCookie = Request.Cookies["refreshToken"];

            if(Guid.TryParse(tokenFromCookie, out var tokenId))
            {
                LogoutUserCommand query = new LogoutUserCommand(tokenId);
                await _mediator.Send(query);
            }

            Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                Path = "/api/auth"
            });
            return Ok();
        }
    }
}
