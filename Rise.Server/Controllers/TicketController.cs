using Auth0.ManagementApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rise.Shared.Tickets;

namespace Rise.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketController : Controller
{
    private readonly ITicketService ticketService;

    public TicketController(ITicketService ticketService)
    {
        this.ticketService = ticketService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TicketDto>> GetTicketByIdAsync( int id)
    {
       
            var ticket = await ticketService.GetTicketByIdAsync(id);
        
            if (ticket == null)
            {
                return NotFound($"Ticket met ID {id} niet gevonden.");
            }

            return Ok(ticket);

    }

    [HttpGet]
    public async Task<ActionResult<List<TicketDto>>> GetTicketsAsync()
    {
       
            var tickets = await ticketService.GetTicketsAsync();
            if (tickets == null)
            {
                return NotFound($"Tickets not found.");
            }
            return Ok(tickets);
       
    }

   [HttpPost("add")]
    public async Task<IActionResult> AddTicket(AddTicketDto ticket)
    {
        if ((ticket.MovieId > 0 && ticket.EventId > 0) || (ticket.MovieId <= 0 && ticket.EventId <= 0) || string.IsNullOrEmpty(ticket.CinemaName) || ticket.ShowTime == default)
        {
            return BadRequest("Invalid ticket data.");
        }

        
        await ticketService.AddTicket(ticket);
        
        return Ok(new { message = "Ticket added successfully." });
        
    }

    
}
