using Microsoft.AspNetCore.Mvc;
using ServerPartWhatAmIToDo.Models;

namespace ServerPartWhatAmIToDo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FiltersController : ControllerBase
{
    private static List<Filter> filters = new List<Filter>();

    [HttpGet]
    public IActionResult GetAllFilters()
    {
        return Ok(filters);
    }

    [HttpPost]
    public IActionResult CreateFilter([FromBody] Filter newFilter)
    {
        filters.Add(newFilter);
        return Ok(new { Message = "Filter created successfully" });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateFilter(int id, [FromBody] UpdateFilterRequest request)
    {
        var filter = filters.FirstOrDefault(f => f.Id == id);
        if (filter == null)
        {
            return NotFound("Filter not found");
        }

        filter.Title = request.Title;
        filter.Color = request.Color;

        return Ok(new { Message = "Filter updated successfully" });
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteFilter(int id)
    {
        var filter = filters.FirstOrDefault(f => f.Id == id);
        if (filter == null)
        {
            return NotFound("Filter not found");
        }

        filters.Remove(filter);
        return Ok(new { Message = "Filter deleted successfully" });
    }
}