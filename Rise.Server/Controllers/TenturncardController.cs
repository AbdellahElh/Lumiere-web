using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rise.Shared.Tenturncards;

namespace Rise.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TenturncardController : Controller
{
    private readonly ITenturncardService tenturncardService;

    public TenturncardController(ITenturncardService tenturncardService)
    {
        this.tenturncardService = tenturncardService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TenturncardDto>>> GetTenturncardsAsync()
    {
        try
        {
            var tenturncards = await tenturncardService.GetTenturncardsAsync();
            return Ok(tenturncards);
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Unauthorized access while retrieving tenturncards.");
            return Unauthorized("You must be logged in to access this resource.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving tenturncards: {ex.Message}");
            return StatusCode(500, "An error occurred while retrieving tenturncards.");
        }
    }

    [HttpPost("add/{tenturncardCode}")] 
    public async Task<ActionResult> AddTenturncard(string tenturncardCode)
    {
        await tenturncardService.AddTenturncard(tenturncardCode);
        return Ok(); 
    }

    [HttpPost("update/{ActivationCode}")]
    public async Task<ActionResult> UpdateTenturncardValue(string ActivationCode)
    {
        
        await tenturncardService.UpdateTenturncardValueAsync(ActivationCode);
        return Ok();
    }

    [HttpPost("edit")]
    public async Task<ActionResult> EditTenturncard([FromBody] TenturncardDto tenturncardDto)
    {
        await tenturncardService.EditTenturncardAsync(tenturncardDto);
        return Ok();
    }
}
