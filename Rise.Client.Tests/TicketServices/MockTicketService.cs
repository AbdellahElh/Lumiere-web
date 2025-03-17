using Rise.Domain.Exceptions;
using Rise.Shared.Tickets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rise.Client.TicketServices;


 public class MockTicketService : ITicketService
    {
        public Task<List<TicketDto>> GetTicketsAsync()
        {
            return Task.FromResult(new List<TicketDto>());
        }

        public Task<TicketDto> GetTicketByIdAsync(int IdTicket)
        {
            return Task.FromResult(new TicketDto());
        }

        public Task AddTicket(AddTicketDto ticket)
        {
            return Task.CompletedTask;
        }
    }
