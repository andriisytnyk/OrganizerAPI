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
    [Route("user-tasks")]
    public class UserTasksController : ControllerBase
    {
        private readonly IUserTaskService userTaskService;
        private readonly IUserService userService;

        public UserTasksController(IUserTaskService userTaskService, IUserService userService)
        {
            this.userTaskService = userTaskService;
            this.userService = userService;
        }

        // GET: user-tasks
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                string authToken = Request.Cookies["refreshToken"];
                return Ok(await userTaskService.GetAll(userService.GetCurrentUserId(authToken)));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // GET: user-tasks/{id}
        [Route("{id:int}")]
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                return Ok(await userTaskService.GetById(id));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // POST: user-tasks
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserTaskDTO userTask)
        {
            try
            {
                string authToken = Request.Cookies["refreshToken"];
                return Ok(await userTaskService.Create(userTask, userService.GetCurrentUserId(authToken)));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT: user-tasks
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserTaskDTO userTask)
        {
            try
            {
                await userTaskService.Update(userTask);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE: user-tasks
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] UserTaskDTO userTask)
        {
            try
            {
                await userTaskService.Delete(userTask);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // DELETE: user-tasks/{id}
        [Route("{id:int}")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await userTaskService.DeleteById(id);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}