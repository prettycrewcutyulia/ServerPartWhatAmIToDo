using Microsoft.AspNetCore.Mvc;
using ServerPartWhatAmIToDo.Models;
using ServerPartWhatAmIToDo.Models.Goals;

namespace ServerPartWhatAmIToDo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FiltersController : ControllerBase
{
    private static List<Filter> filters = new List<Filter>();

    [HttpGet]
    public IActionResult GetAllFilters([FromQuery] string userId)
    {
        // Фильтрация целей по userId
        var filteredCategory = filters.Where(g => g.UserId == userId).ToList();
        return Ok(filteredCategory);
    }

    [HttpPost("create")]
    public IActionResult CreateFilter([FromQuery] string userId, [FromBody] UpdateFilterRequest newFilter)
    {
        filters.Add(new Filter(userId, newFilter.Title, newFilter.Color));
        
        return Ok(new { Message = "Filter created successfully" });
    }

    [HttpPut("update")]
    public IActionResult UpdateFilter([FromQuery] string id, [FromBody] UpdateFilterRequest request)
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
    
    [HttpDelete("delete")]
    public IActionResult DeleteFilter([FromQuery] string id)
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