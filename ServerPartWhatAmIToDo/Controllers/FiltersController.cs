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
    public IActionResult GetAllFilters([FromQuery] int userId)
    {
        
        var filters =  _filterService.GetFiltersByUserIdAsync(userId).Result;
        return Ok(filters);
    }

    [HttpPost("create")]
    public IActionResult CreateFilter([FromBody] FilterRequest newFilter)
    {
        var filter = _filterService.AddFilterAsync(newFilter).Result;
        return Ok(filter);
    }

    [HttpPut("update")]
    public IActionResult UpdateFilter([FromBody] UpdateFilterRequest request)
    {
        var filter = _filterService.UpdateFilterAsync(request).Result;

        return Ok(filter);
    }
    
    [HttpDelete("delete")]
    public IActionResult DeleteFilter([FromQuery] int id)
    {
       _filterService.DeleteFilterAsync(id).Wait();
        return Ok(new { Message = "Filter deleted successfully" });
    }
}