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
        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] UserRequestDTO request)
        {
            try
            {
                var response = await userService.Registration(request, ipAddress());

                setTokenCookie(response.RefreshToken);

                return Ok(response);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: users/authenticate
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserAuthRequestDTO request)
        {
            try
            {
                var response = userService.Authenticate(request, ipAddress());

                if (response == null)
                    return BadRequest(new { message = "Username or password is incorrect" });

                setTokenCookie(response.RefreshToken);

                return Ok(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // POST: users/refresh-token
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

        // POST: users/revoke-token
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

        // GET: users/current
        [HttpGet("current")]
        public IActionResult GetCurrentUser()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = userService.GetCurrentUser(refreshToken);
            if (user == null) 
                return NotFound();

            return Ok(user);
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
