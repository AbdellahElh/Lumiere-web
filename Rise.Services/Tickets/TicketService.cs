using Microsoft.EntityFrameworkCore;
using Rise.Domain.Accounts;
using Rise.Domain.Exceptions;
using Rise.Domain.Tickets;
using Rise.Persistence;
using Rise.Services.Auth;
using Rise.Shared.Movies;
using Rise.Shared.Events;
using Rise.Shared.Products;
using Rise.Shared.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Rise.Domain.Events;
using Rise.Domain.Movies;

namespace Rise.Services.Tickets;

public class TicketService : ITicketService
{
     private readonly ApplicationDbContext dbContext;
    private readonly IAuthContextProvider authContextProvider;
    private readonly SmtpClient smtpClient;
    private readonly string senderEmail;

     public TicketService(ApplicationDbContext dbContext, IAuthContextProvider authContextProvider)
    {
        if (authContextProvider.User is null)
            throw new ArgumentNullException($"{nameof(TicketService)} requires a {nameof(authContextProvider)}");
        this.dbContext = dbContext;
        this.authContextProvider = authContextProvider;

        senderEmail = "rise6698@gmail.com";
        var senderPassword = "zkuq squo tgzz kriv"; 

        smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(senderEmail, senderPassword),
            EnableSsl = true, 
        };
    }

    private Account GetLoggedInUser()
    {
        var accountId = authContextProvider.GetAccountIdFromMetadata(authContextProvider.User!);
      
        var account = dbContext.Accounts.FirstOrDefault(a => a.Id == accountId);
        if (account is null)
            throw new ArgumentNullException($"{nameof(TicketService)} requires a {nameof(account)}");
        return account;
    }
    public async Task<TicketDto> GetTicketByIdAsync(int idTicket)
    {
        var account = GetLoggedInUser();
        var ticket = await dbContext.Tickets.FirstOrDefaultAsync(t => t.Id == idTicket && t.AccountId == account.Id);

        if (ticket is null)
        {
            throw new EntityNotFoundException($"Ticket niet gevonden voor id {idTicket}.");
        }

        MovieDto? movieDto = null;
        EventDto? eventDto = null;

        if (ticket.MovieId.HasValue && ticket.MovieId.Value > 0)
        {
            var foundMovie = await dbContext.Movies.FirstOrDefaultAsync(m => m.Id == ticket.MovieId.Value);

            if (foundMovie is null)
                throw new EntityNotFoundException("Movie niet gevonden voor deze ticket.");

            movieDto = new MovieDto
            {
                Id = foundMovie.Id,
                Title = foundMovie.Title,
                ReleaseDate = foundMovie.ReleaseDate,
                MovieLink = foundMovie.MovieLink,
                Genre = foundMovie.Genre,
                Duration = foundMovie.Duration,
                Director = foundMovie.Director,
                Description = foundMovie.Description,
            };
        }
        else if (ticket.EventId.HasValue && ticket.EventId.Value > 0)
        {
            var foundEvent = await dbContext.Events.FirstOrDefaultAsync(e => e.Id == ticket.EventId.Value);

            if (foundEvent is null)
                throw new EntityNotFoundException("Event niet gevonden voor deze ticket.");

            eventDto = new EventDto
            {
                Id = foundEvent.Id,
                Title = foundEvent.Title,
            };
        }
        else
        {
            throw new InvalidOperationException("Ticket moet of met event of film gekend zijn.");
        }

        var newTicket = new TicketDto
        {
            Id = ticket.Id,
            Type = Rise.Shared.Tickets.TicketType.Standaard,
            DateTime = ticket.DateTime,
            Location = ticket.Location,
            Movie = movieDto,
            Event = eventDto
        };

        return newTicket;
    }
    public async Task<List<TicketDto>> GetTicketsAsync()
    {
        var account = GetLoggedInUser();

        var tickets = await dbContext.Tickets
            .Where(t => t.AccountId == account.Id)
            .ToListAsync();

        var ticketDtos = new List<TicketDto>();

        foreach (var ticket in tickets)
        {
            MovieDto? movieDto = null;
            EventDto? eventDto = null;

            if (ticket.MovieId.HasValue && ticket.MovieId.Value > 0)
            {
                var foundMovie = await dbContext.Movies.FirstOrDefaultAsync(m => m.Id == ticket.MovieId.Value);

                if (foundMovie is null)
                    throw new EntityNotFoundException("Movie niet gevonden voor deze ticket.");

                movieDto = new MovieDto
                {
                    Id = foundMovie.Id,
                    Title = foundMovie.Title,
                    ReleaseDate = foundMovie.ReleaseDate,
                    MovieLink = foundMovie.MovieLink,
                    Genre = foundMovie.Genre,
                    Duration = foundMovie.Duration,
                    Director = foundMovie.Director,
                    Description = foundMovie.Description,
                };
            }
            else if (ticket.EventId.HasValue && ticket.EventId.Value > 0)
            {
                var foundEvent = await dbContext.Events.FirstOrDefaultAsync(e => e.Id == ticket.EventId.Value);

                if (foundEvent is null)
                    throw new EntityNotFoundException("Event niet gevonden voor deze ticket.");

                eventDto = new EventDto
                {
                    Id = foundEvent.Id,
                    Title = foundEvent.Title,
                    Genre = foundEvent.Genre,
                    Type = foundEvent.Type,
                    Description = foundEvent.Description,
                    Duration = foundEvent.Duration.ToString(),
                    Director = foundEvent.Director,
                    Date = foundEvent.ReleaseDate.ToString(),
                    Video = foundEvent.EventLink
                };
            }
            else
            {
                throw new InvalidOperationException("Ticket moet of met event of film gekend zijn.");
            }

            var newTicket = new TicketDto
            {
                Id = ticket.Id,
                Type = Rise.Shared.Tickets.TicketType.Standaard,
                DateTime = ticket.DateTime,
                Location = ticket.Location,
                Movie = movieDto,
                Event = eventDto
            };

            ticketDtos.Add(newTicket);
        }

        return ticketDtos;
    }

    public async Task AddTicket(AddTicketDto newTicket){
    {
        var account = GetLoggedInUser();
    
        if ((newTicket.MovieId > 0 && newTicket.EventId > 0) || (newTicket.MovieId <= 0 && newTicket.EventId <= 0))
        {
            throw new ArgumentException("Je moet of een eventid of movieId geven maar niet beide.");
        }
    
        Movie? foundMovie = null;
        Event? foundEvent = null;
    
        if (newTicket.MovieId > 0)
        {
            foundMovie = await dbContext.Movies.FirstOrDefaultAsync(m => m.Id == newTicket.MovieId);
            if (foundMovie is null){
                throw new EntityNotFoundException("Movie was niet gevonden");
            }
        }
        else if (newTicket.EventId > 0)
        {
            foundEvent = await dbContext.Events.FirstOrDefaultAsync(e => e.Id == newTicket.EventId);
            if (foundEvent is null){
                throw new EntityNotFoundException("Event was niet gevonden");
            }
        }
    
        var ticket = new Ticket
        {
            AccountId = account.Id,
            Account = account,
            MovieId = foundMovie?.Id,
            Movie = foundMovie,
            EventId = foundEvent?.Id,
            Event = foundEvent,
            DateTime = newTicket.ShowTime,
            Location = newTicket.CinemaName,
            Type = Rise.Domain.Tickets.TicketType.Standaard
        };
    
        account.Tickets.Add(ticket);
        dbContext.Accounts.Update(account);
        dbContext.Tickets.Add(ticket);
        await dbContext.SaveChangesAsync();
    
        try
        {
            bool isEvent = newTicket.EventId > 0;
            await SendConfirmationEmail(account.Email, ticket, isEvent);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
        }
    }
    }

     private async Task SendConfirmationEmail(string userEmail, Ticket ticket, bool isEvent)
        {
            var title = isEvent ? ticket.Event?.Title : ticket.Movie?.Title;

           

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = $"Bevestiging van uw aankoop bij stadsbioscoop Lumiere {ticket.Location}",
                Body = $@"
                        <p>Beste Lumiere {ticket.Location} bezoeker, <br/><br/> 
                        Bedankt voor je aankoop. De betaling voor je bestelling met nummer {ticket.Id} is ontvangen en verwerkt. <br/> 
                        Je kan je e-tickets via de volgende link openen:</p>
                        <a href='https://localhost:5001/tickets/{ticket.Id}'>Open je ticket</a>
                        <p>Je hoeft ze niet af te drukken, je kan ze gewoon op je smartphone laten zien aan de ingang van de cinema.</p>
                        <h4>Instructies:</h4>
                        <ul>
                            <li>Noteer veiligheidshalve het bestelnummer.</li>
                            <li>Neem je ticket mee naar de voorstelling.</li>
                            <li>Gelieve je ticket te tonen aan de medewerker bij het binnenkomen van de cinema. Indien de medewerker niet aanwezig is dan zal de kassamedewerker je ticket valideren. In beide gevallen mag je op vertoon en na scan van je ticket de cinema binnen</li>
                        </ul>
                        <h2>Info Tickets:</h2> 
                        <h3>{title}</h3>
                        <p>{ticket.DateTime }</p> 
                        <p>1X {ticket.Type}:{ticket.Price}€</p>
                        <p>Totaal: {ticket.Price}€</p>
                        <p><br/>Veel plezier bij de voorstelling! <br/><br/> vriendelijke groet, <br/><br/> het team van stadsbioscoop Lumiere {ticket.Location}</p>
                    ",
                IsBodyHtml = true,
            };
                mailMessage.To.Add(userEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
       

  
}
