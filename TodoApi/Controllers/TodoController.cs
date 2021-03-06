using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonApi.Envelope;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using JsonApi.Wrapper;
using Microsoft.AspNetCore.Routing;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IWrapper _wrapper;
        
        private readonly TodoContext _context;

        protected readonly LinkGenerator _linkGenerator;

        public TodoController(TodoContext context, LinkGenerator linkGenerator)
        {
            _context = context;
            _linkGenerator = linkGenerator;
            var path = ""; // TODO
            _wrapper = WrapperBuilder
                .WithServer(path)
                .WithDefaultConfig(p => p.HyphenCasedTypes())
                .WithTypeConfig<TodoItem>(p => p.WithType("todo"))
                .Build();

            if (_context.TodoItems.Count() == 0)
            {
                // Create a new TodoItems if collection is empty,
                // which means you can't delete all TodoItems.
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }
/*

        // GET api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }*/

        // GET api/Todo
        [HttpGet]
        public async Task<ActionResult<CollectionEnvelope<TodoItem>>> GetTodoItems2()
        {
            var items = await _context.TodoItems.ToListAsync();
            var collectionEnvelope = _wrapper.WrapAll(items);
            collectionEnvelope.Links["self"] = HttpContext.Request.Path;
            return collectionEnvelope;
        }

        // GET api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IResourceEnvelope<TodoItem>>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null) 
            {
                return NotFound();
            }

            // return todoItem;
            var envelope = _wrapper.Wrap(todoItem);
            envelope.Links["self"] = HttpContext.Request.Path;
            return envelope;
        }

        // POST api/todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            var actionName = nameof(GetTodoItem);
            return CreatedAtAction(actionName, new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItem>> PutTodoItem(long id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/Todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}