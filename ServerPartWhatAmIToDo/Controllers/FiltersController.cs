using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerPartWhatAmIToDo.Models.Goals;
using ServerPartWhatAmIToDo.Services;

namespace ServerPartWhatAmIToDo.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FiltersController : ControllerBase
{
    
    private readonly IFilterService _filterService;

    public FiltersController(IFilterService filterService)
    {
        _filterService = filterService;
    }

    [HttpGet]
    public IActionResult GetAllFilters([FromQuery] int userId, CancellationToken token)
    {
        
        var filters =  _filterService.GetFiltersByUserIdAsync(userId, token).Result;
        return Ok(filters);
    }

    [HttpPost("create")]
    public IActionResult CreateFilter([FromQuery]int userId, [FromBody] FilterRequest newFilter, CancellationToken token)
    {
        var filter = _filterService.AddFilterAsync(userId, newFilter, token).Result;
        return Ok(filter);
    }

    [HttpPut("update")]
    public IActionResult UpdateFilter([FromBody] UpdateFilterRequest request, CancellationToken token)
    {
        var filter = _filterService.UpdateFilterAsync(request, token).Result;

        return Ok(filter);
    }
    
    [HttpDelete("delete")]
    public IActionResult DeleteFilter([FromQuery] int id, CancellationToken token)
    {
       _filterService.DeleteFilterAsync(id, token).Wait();
        return Ok(new { Message = "Filter deleted successfully" });
    }
}