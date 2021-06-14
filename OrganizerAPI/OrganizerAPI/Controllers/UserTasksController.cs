using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrganizerAPI.Domain.Interfaces;
using OrganizerAPI.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizerAPI.Controllers
{
    [Route("user-tasks")]
    [ApiController]
    public class UserTasksController : ControllerBase
    {
        readonly IUserTaskService service;

        public UserTasksController(IUserTaskService userTaskService)
        {
            service = userTaskService;
        }

        // GET: user-tasks
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await service.GetAll());
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
                return Ok(await service.GetById(id));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // POST: user-tasks
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserTask userTask)
        {
            try
            {
                return Ok(await service.Create(userTask));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT: user-tasks
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserTask userTask)
        {
            try
            {
                await service.Update(userTask);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE: user-tasks
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] UserTask userTask)
        {
            try
            {
                await service.Delete(userTask);
                return NoContent();
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
                await service.DeleteById(id);
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}