using NodaTime;
using Rise.Domain.Accounts;
using Rise.Domain.Events;
using Rise.Domain.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Domain.Tickets;
public enum TicketType
{
    Standaard,
    Senior,   
    Student   
}
public class Ticket : Entity
{
    public required DateTime DateTime { get; set; }

    public required string Location { get; set; }

    public required TicketType Type { get; set; }

    public decimal Price
    {
        get
        {
            return Type switch
            {
                TicketType.Standaard => 12.0m,
                TicketType.Senior => 11.5m,
                TicketType.Student => 10.0m,
                _ => 12.0m
            };
        }
    }
    public  int? MovieId { get; set; } 

    public  Movie? Movie { get; set; } 

    public  int? EventId { get; set; } 

    public  Event? Event { get; set; } 

    public int? AccountId { get; set; }
    public Account? Account { get; set; }
}
