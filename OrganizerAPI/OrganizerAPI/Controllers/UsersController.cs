using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrganizerAPI.Domain.Interfaces;
using OrganizerAPI.Shared.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserAuthRequestDTO request)
        {
            var response = userService.Authenticate(request, ipAddress());

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = userService.UpdateRefreshToken(refreshToken, ipAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [HttpPost("revoke-token")]
        public IActionResult RevokeToken([FromBody] UserAuthRevokeTokenRequest request)
        {
            // accept token from request body or cookie
            var token = request.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = userService.RevokeToken(token, ipAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = userService.GetById(id);
            if (user == null) 
                return NotFound();

            return Ok(user);
        }

        [HttpGet("{id}/refresh-tokens")]
        public IActionResult GetRefreshTokens(int id)
        {
            var user = userService.GetById(id);
            if (user == null) 
                return NotFound();

            return Ok(user.UserRefreshTokens);
        }

        // helper methods

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
