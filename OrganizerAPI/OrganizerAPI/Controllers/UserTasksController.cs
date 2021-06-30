using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using OrganizerAPI.Domain.Interfaces;
using OrganizerAPI.Shared.ModelsDTO;
using System;
using System.Threading.Tasks;

namespace OrganizerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("user-tasks")]
    public class UserTasksController : ControllerBase
    {
        private readonly IUserTaskService _userTaskService;
        private readonly IUserService _userService;

        public UserTasksController(IUserTaskService userTaskService, IUserService userService)
        {
            _userTaskService = userTaskService;
            _userService = userService;
        }

        // GET: user-tasks
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var authToken = Request.Cookies["refreshToken"];
                return Ok(await _userTaskService.GetAll(_userService.GetCurrentUserId(authToken)));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // GET: user-tasks/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                var authToken = Request.Cookies["refreshToken"];
                return Ok(await _userTaskService.GetById(id, _userService.GetCurrentUserId(authToken)));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // POST: user-tasks
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserTaskDto userTask)
        {
            try
            {
                var authToken = Request.Cookies["refreshToken"];
                return Created(
                    Request.GetDisplayUrl(), 
                    await _userTaskService.Create(
                        userTask, 
                        _userService.GetCurrentUserId(authToken))
                    );
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT: user-tasks
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserTaskDto userTask)
        {
            try
            {
                var authToken = Request.Cookies["refreshToken"];
                await _userTaskService.Update(
                    userTask, 
                    _userService.GetCurrentUserId(authToken));
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE: user-tasks
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] UserTaskDto userTask)
        {
            try
            {
                var authToken = Request.Cookies["refreshToken"];
                await _userTaskService.Delete(userTask, _userService.GetCurrentUserId(authToken));
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // DELETE: user-tasks/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var authToken = Request.Cookies["refreshToken"];
                await _userTaskService.DeleteById(id, _userService.GetCurrentUserId(authToken));
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}