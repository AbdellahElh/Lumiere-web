using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Shared.Tickets;
public interface ITicketService
{
    public Task<List<TicketDto>> GetTicketsAsync();
    public Task<TicketDto> GetTicketByIdAsync(int IdTicket);
    public Task AddTicket(AddTicketDto ticket);

}