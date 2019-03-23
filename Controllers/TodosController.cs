using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodosController : Controller
    {
        private readonly TodoDbContext db;

        public TodosController(TodoDbContext db)
        {
            this.db = db;
        }

        [HttpGet("{user}")]
        public async Task<IActionResult> GetTodos(string user, [FromQuery] string description, [FromQuery] bool? completed)
        {
            if (!TodoAppUser.IsAllowed(user)) { return NotFound(); }

            IQueryable<Todo> query = db.Todos
                .Where(t => t.UserName == user);

            if (!string.IsNullOrWhiteSpace(description))
            {
                query = query.Where(t => t.Description.Contains(description, StringComparison.OrdinalIgnoreCase));
            }

            if (completed != null)
            {
                query = query.Where(t => t.IsComplete == completed.Value);
            }

            var todos = await query.ToListAsync();

            return Ok(todos);
        }

        [HttpPost("{user}")]
        public async Task<IActionResult> AddTodo(string user, [FromBody] AddTodoModel model)
        {
            if (!TodoAppUser.IsAllowed(user)) { return NotFound(); }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = new Todo
            {
                AddedAt = DateTimeOffset.Now,
                LastUpdatedAt = DateTimeOffset.Now,
                IsComplete = false,
                UserName = user,
                Description = model.Description
            };

            db.Todos.Add(todo);

            await db.SaveChangesAsync();

            return Ok(todo);
        }

        [HttpDelete("{user}/{id}")]
        public async Task<IActionResult> DeleteTodo(string user, long id)
        {
            if (!TodoAppUser.IsAllowed(user)) { return NotFound(); }

            var todo = await db.Todos
                .FirstOrDefaultAsync(t => t.Id == id && t.UserName == user);

            if (todo == null)
            {
                return NotFound();
            }

            db.Todos.Remove(todo);

            await db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{user}/{id}")]
        public async Task<IActionResult> UpdateTodo(string user, long id, [FromBody] EditTodoModel model)
        {
            if (!TodoAppUser.IsAllowed(user)) { return NotFound(); }

            var todo = await db.Todos
                .FirstOrDefaultAsync(t => t.Id == id && t.UserName == user);

            if (todo == null)
            {
                return NotFound();
            }

            todo.Description = model.Description;
            todo.IsComplete = model.IsComplete;
            todo.LastUpdatedAt = DateTimeOffset.Now;

            db.Todos.Update(todo);

            await db.SaveChangesAsync();

            return Ok(todo);
        }
    }
}
