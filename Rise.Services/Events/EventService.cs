using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Rise.Domain.Events;
using Rise.Persistence;
using Rise.Shared.Events;
using Rise.Shared.Movies;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace Rise.Services.Events;

public class EventService : IEventService
{

    private readonly ApplicationDbContext dbContext;

    public EventService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }


    public async Task<List<EventDto>> GetEventAsync(Shared.Events.FiltersDataDto filters)
    {
        var selectedCinemas = filters.SelectedCinemas ?? new List<string>();
        var selectedDate = filters.SelectedDate;

        var events = await dbContext.Events
            .Include(e => e.Cinemas)
                .ThenInclude(c => c.Showtimes)
            .Where(e => e.Cinemas.Any(e => selectedCinemas.Contains(e.Name)) &&
                        e.Showtimes.Any(e => selectedDate == null || e.ShowTime.Date == selectedDate.Value.Date))
            .ToListAsync();


        var eventDtos = events.Select(SpecificEvent =>
        {
            var selectedCinemasForEvent = SpecificEvent.Cinemas
                .Where(cinema => selectedCinemas.Contains(cinema.Name))
                .ToList();

            var showtimes = selectedCinemasForEvent
                .SelectMany(cinema => cinema.Showtimes
                    .Where(showtime => showtime.EventId == SpecificEvent.Id))
                .ToList();

            var dates = showtimes
                .Select(s => s.ShowTime.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            var earliestDate = dates.FirstOrDefault();
            var latestDate = dates.LastOrDefault();

            string dateRange = null!;
            if (earliestDate != default(DateTime) && latestDate != default(DateTime))
            {
                dateRange = earliestDate == latestDate
                    ? earliestDate.ToString("d MMM yyyy")
                    : $"{earliestDate: d MMM yyyy} - {latestDate: d MMM yyyy}";
            }

            int durationMinutes = SpecificEvent.Duration;
            var times = showtimes
                .Select(s => s.ShowTime.TimeOfDay)
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            var earliestTime = times.FirstOrDefault();



            var locations = selectedCinemasForEvent
                .Select(c => c.Name)
                .Distinct()
                .ToList();

            string locationsStr = string.Join(", ", locations);

            return new EventDto
            {
                Id = SpecificEvent.Id,
                Title = SpecificEvent.Title,
                Genre = SpecificEvent.Genre,
                Duration = $"{SpecificEvent.Duration} minuten",
                Director = SpecificEvent.Director,
                Description = SpecificEvent.Description,
                Video = SpecificEvent.EventLink,
                VideoPlaceholderImage = SpecificEvent.VideoPlaceholderUrl!,
                Cover = SpecificEvent.CoverImageUrl!,
                Date = dateRange!,
                Location = locationsStr,
                Type = SpecificEvent.Type,
                Price = SpecificEvent.Price,
                Cast = SpecificEvent.Cast,
                Cinemas = selectedCinemasForEvent
                    .Select(cinema => new CinemaDto
                    {
                        id = cinema.Id,
                        Name = cinema.Name,
                        Showtimes = cinema.Showtimes
                            .Where(showtime => showtime.EventId == SpecificEvent.Id)
                            .Where(showtime => selectedDate == null || showtime.ShowTime.Date == selectedDate.Value.Date)
                            .Select(showtime => showtime.ShowTime)
                            .ToList()
                    })
                    .Where(cinema => cinema.Showtimes.Any())
                    .ToList()
            };
        })
        .Where(eventDto => eventDto.Cinemas.Any())
        .ToList();

        return eventDtos;
    }


    public async Task<EventDto> GetEventByIdAsync(int id)
    {
        var today = DateTime.Today;

        // Haal het event op inclusief Cinemas en Showtimes, en Movies
        var events = await dbContext.Events
            .Where(e => e.Id == id)
            .Include(e => e.Cinemas)
                .ThenInclude(c => c.Showtimes) // Include showtimes in Cinemas
            .Include(e => e.Movies) // Include Movies
            .FirstOrDefaultAsync() ?? throw new KeyNotFoundException($"Event with ID {id} not found.");

        // Map de EventDto
        var eventDto = new EventDto
        {
            Id = events.Id,
            Title = events.Title,
            Type = events.Type,
            Genre = events.Genre,
            Duration = $"{events.Duration} minuten",
            Director = events.Director,
            Description = events.Description,
            Video = events.EventLink,
            VideoPlaceholderImage = events.VideoPlaceholderUrl!,
            Cover = events.CoverImageUrl!,
            Cast = events.Cast,

            // Cinemas met Showtimes
            Cinemas = events.Cinemas
                .Select(cinema => new CinemaDto
                {
                    id = cinema.Id,
                    Name = cinema.Name,
                    Showtimes = cinema.Showtimes
                        .Where(s => s.EventId == id && s.ShowTime.Date == today) // Filteren op EventId en de huidige datum
                        .Select(s => s.ShowTime)
                        .ToList()
                })
                .Where(cinema => cinema.Showtimes.Any()) // Alleen cinemas met showtimes behouden
                .ToList(),

            // Movies
            Movies = events.Movies
                .Select(m => new MoviePosterDto
                {
                    Id = m.Id,
                    Cover = m.CoverImageUrl!,
                    ReleaseDate = m.ReleaseDate
                })
                .ToList()
        };

        // Verwijder duplicaten van Showtimes per Cinema
        foreach (var cinema in eventDto.Cinemas)
        {
            cinema.Showtimes = cinema.Showtimes
                .GroupBy(st => st) // Groepeer showtimes op basis van de showtime zelf
                .Select(g => g.Key)  // Selecteer de unieke showtime uit de groep
                .ToList();
        }

        return eventDto;
    }


}




