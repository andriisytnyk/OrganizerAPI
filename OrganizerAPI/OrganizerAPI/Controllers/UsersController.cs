using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
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

        // GET: users
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await userService.GetAll());
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // GET: users/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                return Ok(await userService.GetById(id));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // POST: users
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDTO user)
        {
            try
            {
                string authToken = Request.Cookies["refreshToken"];
                return Created(
                    Request.GetDisplayUrl(),
                    await userService.Create(
                        user,
                        userService.GetCurrentUserId(authToken))
                    );
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT: users/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put([FromBody] UserDTO user)
        {
            try
            {
                string authToken = Request.Cookies["refreshToken"];
                await userService.Update(
                    user,
                    userService.GetCurrentUserId(authToken));
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE: users
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] UserDTO user)
        {
            try
            {
                string authToken = Request.Cookies["refreshToken"];
                await userService.Delete(user, userService.GetCurrentUserId(authToken));
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // DELETE: users/{id}
        [Route("{id:int}")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                string authToken = Request.Cookies["refreshToken"];
                await userService.DeleteById(id, userService.GetCurrentUserId(authToken));
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // POST: users/registration
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
            catch (Exception ex)
            {
                return BadRequest(ex);
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
