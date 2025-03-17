using Rise.Client.Auth;
using Rise.Shared.Tickets;
using System.Net.Http;
using System.Net.Http.Json;
using System.Xml;

namespace Rise.Client.Tickets.services;

public class TicketService : ITicketService
{
    private readonly HttpClient _httpClient;

    public TicketService(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }
    public async Task<TicketDto> GetTicketByIdAsync(int id)
    {
   
        var ticket = await _httpClient.GetFromJsonAsync<TicketDto>($"Ticket/{id}");
        return ticket!;
   
    }

   public async Task<List<TicketDto>> GetTicketsAsync()
    {
        
        var tickets = await _httpClient.GetFromJsonAsync<List<TicketDto>>("Ticket");
        return tickets!;
    
    }
    
  public async Task AddTicket(AddTicketDto ticket)
    {
        if (!string.IsNullOrEmpty(ticket.CinemaName) && ticket.ShowTime != default)
        {
            await _httpClient.PostAsJsonAsync("Ticket/add", ticket);
        }
    }
 
  
   
    
}