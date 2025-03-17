using Microsoft.AspNetCore.Mvc;
using Rise.Shared.Events;
namespace Rise.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly IEventService eventService;

    public EventController(IEventService eventService)
    {
        this.eventService = eventService;
    }


    [HttpGet]
    public async Task<ActionResult<List<EventDto>>> Get([FromQuery] DateTime? date = null, [FromQuery] List<string> cinema = null!)
    {
       
            var filters = new FiltersDataDto
            {
                SelectedDate = date,
                SelectedCinemas = cinema
            };
            var events = await eventService.GetEventAsync(filters);
            if (events == null)
            {
                return NotFound($"Events not found.");
            }
            return Ok(events);
        
       
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetById(int id)
    {
        
            var events = await eventService.GetEventByIdAsync(id);
            if (events == null)
            {
                return NotFound($"Event met ID {id} niet gevonden.");
            }

            return Ok(events);
        
    }
}
