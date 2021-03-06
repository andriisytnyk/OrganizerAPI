using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using OrganizerAPI.Domain.Interfaces;
using OrganizerAPI.Shared.ModelsDTO;
using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using OrganizerAPI.Shared.Exceptions;

namespace OrganizerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: users
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _userService.GetAll());
            }
            catch (Exception ex)
            {
                return NotFound(new { ex.Message });
            }
        }

        // GET: users/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                return Ok(await _userService.GetById(id));
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // POST: users
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserRequestDto user)
        {
            try
            {
                var authToken = Request.Cookies["refreshToken"];

                return Created(
                    Request.GetDisplayUrl(),
                    await _userService.Create(
                        user,
                        await _userService.GetCurrentUserId(authToken))
                    );
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return BadRequest(new { ex.InnerException.Message });
                return BadRequest(new { ex.Message });
            }
        }

        // PUT: users
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserRequestDto user)
        {
            try
            {
                var authToken = Request.Cookies["refreshToken"];
                await _userService.Update(
                    user,
                    await _userService.GetCurrentUserId(authToken));
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return BadRequest(new { ex.InnerException.Message });
                return BadRequest(new { ex.Message });
            }
        }

        // DELETE: users
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] UserRequestDto user)
        {
            try
            {
                var authToken = Request.Cookies["refreshToken"];
                await _userService.Delete(user, await _userService.GetCurrentUserId(authToken));

                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // DELETE: users/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var authToken = Request.Cookies["refreshToken"];
                await _userService.DeleteById(id, await _userService.GetCurrentUserId(authToken));

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
        public async Task<IActionResult> Registration([FromBody] UserRequestDto request)
        {
            try
            {
                var response = await _userService.Registration(request, IpAddress());
                SetTokenCookie(response.RefreshToken);

                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { errors = ex.Errors.ToList().Select(e => e.ErrorMessage) });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // POST: users/authenticate
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthRequestDto request)
        {
            try
            {
                var response = await _userService.Authenticate(request, IpAddress());
                SetTokenCookie(response.RefreshToken);

                return Ok(response);
            }
            catch (InvalidAuthDataException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: users/refresh-token
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];

                var response = await _userService.UpdateRefreshToken(refreshToken, IpAddress());

                SetTokenCookie(response.RefreshToken);

                return Ok(response);
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (NotActiveTokenException ex)
            {
                return Unauthorized(new { ex.Message });
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: users/revoke-token
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] UserAuthRevokeTokenRequest request)
        {
            try
            {
                // accept token from request body or cookie
                var token = request.Token ?? Request.Cookies["refreshToken"];

                await _userService.RevokeToken(token, IpAddress());

                return Ok(new { message = "Token revoked" });
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (NotActiveTokenException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: users/current-user
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                var user = await _userService.GetCurrentUser(refreshToken);

                return Ok(user);
            }
            catch (UserNotFoundException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception)
            {
                throw;
            }
        }

        // helper methods

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }
    }
}
