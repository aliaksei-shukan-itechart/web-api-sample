using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.DAL.Models;
using Sample.Impl.Services.ToDoTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Web.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _tasksService;

        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        [Authorize(Roles = "admin", Policy = "OnlyForAleksei")]
        [HttpGet]
        public async Task<IEnumerable<ToDoItem>> GetAll()
        {
            return await _tasksService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ToDoItem> Get(int id)
        {
            return await _tasksService.GetAsync(id);
        }

        [HttpPost]
        public async Task Post([FromBody] ToDoItem item)
        {
            await _tasksService.CreateAsync(item);
        }

        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] ToDoItem item)
        {
            await _tasksService.UpdateAsync(id, item);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _tasksService.DeleteAsync(id);
        }
    }
}
