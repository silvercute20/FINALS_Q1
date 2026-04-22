using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private static List<Todo> _todos = new();

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_todos);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Todo newTodo)
    {
        if (newTodo == null || string.IsNullOrWhiteSpace(newTodo.Title))
        {
            return BadRequest("Title required.");
        }

        newTodo.Id = Guid.NewGuid();
        _todos.Add(newTodo);
        // 201 Created
        return CreatedAtAction(nameof(Get), new { id = newTodo.Id }, newTodo);
    }

    [HttpPut("{id}")]
    public IActionResult Put(Guid id, [FromBody] Todo updatedTodo)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return NotFound();
        }

        if (updatedTodo == null || string.IsNullOrWhiteSpace(updatedTodo.Title))
        {
            return BadRequest("Title required.");
        }

        todo.Title = updatedTodo.Title;
        todo.Completed = updatedTodo.Completed;

        // 204 No Content
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var todo = _todos.FirstOrDefault(t => t.Id == id);
        if (todo == null)
        {
            return NotFound();
        }

        _todos.Remove(todo);
        // 204 No Content
        return NoContent();
    }
}
