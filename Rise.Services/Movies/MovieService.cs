using Microsoft.EntityFrameworkCore;
using Rise.Domain.Movies;
using Rise.Persistence;
using Rise.Shared.Movies;

using System.Text.Json;

namespace Rise.Services.Movies;

public class MovieService : IMovieService
{
    private const string MockDataFilePath = "MoviesMockData.json";

    private readonly ApplicationDbContext dbContext;

    public MovieService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }


    public async Task<List<MovieDto>> GetMoviesAsync(FiltersDataDto filters)
    {
        var selectedCinemas = filters.SelectedCinemas ?? new List<string>();
        var selectedDate = filters.SelectedDate;
        var title = filters.Title?.ToLower();
        var pageNumber = filters.pageNumber ?? 1;
        var pageSize = filters.pageSize ?? 1;

        var query = dbContext.Movies.AsQueryable();


        var movies = new List<Movie>();

        if (filters.pageNumber == null)
        {
            movies = await dbContext.Movies
      .Where(m => string.IsNullOrEmpty(title) || m.Title.ToLower().Contains(title))
      .Include(m => m.Cinemas.Where(c => selectedCinemas.Any() ? selectedCinemas.Contains(c.Name) : true))
      .ThenInclude(c => c.Showtimes.Where(s => !selectedDate.HasValue || s.ShowTime.Date == selectedDate.Value.Date))
     .ToListAsync();
        }
        else
        {
            movies = await dbContext.Movies
           .Where(m => string.IsNullOrEmpty(title) || m.Title.ToLower().Contains(title))
           .Include(m => m.Cinemas.Where(c => selectedCinemas.Any() ? selectedCinemas.Contains(c.Name) : true))
           .ThenInclude(c => c.Showtimes.Where(s => !selectedDate.HasValue || s.ShowTime.Date == selectedDate.Value.Date))
           .Skip((pageNumber - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();
        }





        var movieDtos = movies.Select(movie => new MovieDto
        {
            Id = movie.Id,
            EventId = movie?.EventId,
            Title = movie.Title,
            Genre = movie.Genre,
            Duration = movie.Duration,
            Director = movie.Director,
            Description = movie.Description,
            VideoPlaceholderUrl = movie.VideoPlaceholderUrl,
            CoverImageUrl = movie.CoverImageUrl,
            Cast = movie.Cast,
            ReleaseDate = movie.ReleaseDate,
            MovieLink = movie.MovieLink,

            Cinemas = movie.Cinemas
                .Select(cinema => new CinemaDto
                {
                    id = cinema.Id,
                    Name = cinema.Name,
                    Showtimes = cinema.Showtimes
                       .Where(showtime => showtime.MovieId == movie.Id
                        && (!selectedDate.HasValue || showtime.ShowTime.Date == selectedDate.Value.Date))
                        .Select(showtime => showtime.ShowTime)
                        .ToList()
                })
                .Where(cinema => cinema.Showtimes.Any())
                .ToList()
        })
        .Where(movie => movie.Cinemas.Any(cinema => cinema.Showtimes.Any()))
        .ToList();

        return movieDtos;

    }



    public async Task<MovieEventDto> GetMovieByIdAsync(int id)
    {
        var today = DateTime.UtcNow.Date;

        var movie = await dbContext.Movies
            .Include(m => m.Cinemas)
            .ThenInclude(c => c.Showtimes)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie! == null!)
        {
            throw new KeyNotFoundException($"Movie with ID {id} not found.");
        }

        var EventId = movie?.EventId;
        var events = await dbContext.Events
            .Include(m => m.Cinemas)
            .ThenInclude(c => c.Showtimes)
            .FirstOrDefaultAsync(m => m.Id == EventId);

        var movieDto = new MovieDto
        {
            Id = movie.Id,
            EventId = movie?.EventId,
            Title = movie.Title,
            Genre = movie.Genre,
            Duration = movie.Duration,
            Director = movie.Director,
            Description = movie.Description,
            VideoPlaceholderUrl = movie.VideoPlaceholderUrl,
            CoverImageUrl = movie.CoverImageUrl,
            Cast = movie.Cast,
            ReleaseDate = movie.ReleaseDate,
            MovieLink = movie.MovieLink,

            Cinemas = movie.Cinemas
                .Select(cinema => new CinemaDto
                {
                    id = cinema.Id,
                    Name = cinema.Name,
                    Showtimes = cinema.Showtimes
                        .Where(showtime => showtime.MovieId == movie.Id && showtime.ShowTime.Date == today)
                        .Select(showtime => showtime.ShowTime)
                        .ToList()
                })
                .Where(cinema => cinema.Showtimes.Any())
                .ToList()
        };

        var eventDto = events! == null! ? null : new Rise.Shared.Events.EventDto
        {
            Id = events.Id,
            Title = events.Title,
            Type = events.Type,
            Genre = events.Genre,
            Duration = $"{events.Duration} minuten",
            Director = events.Director,
            Description = events.Description,
            Video = events.EventLink,
            VideoPlaceholderImage = events.VideoPlaceholderUrl,
            Cover = events.CoverImageUrl,
            Cast = events.Cast,
        };

        return new MovieEventDto
        {
            Movie = movieDto,
            Event = eventDto
        };
    }

    public async Task<List<MoviePosterDto>> GetRecentMoviePostersAsync()
    {
        var movies = await dbContext.Movies
            .OrderByDescending(m => m.ReleaseDate)
            .Select(m => new MoviePosterDto
            {
                Id = m.Id,
                Cover = m.CoverImageUrl!,
                ReleaseDate = m.ReleaseDate,

            })
            .ToListAsync();

        return movies;
    }

    public async Task<List<MoviePosterDto>> GetFutureMoviePostersAsync()
    {
        var today = DateTime.UtcNow.Date;
        var movies = await dbContext.Movies
            .OrderByDescending(m => m.ReleaseDate)
            .Where(m => m.ReleaseDate.Date >= today) //future
            .Select(m => new MoviePosterDto
            {
                Id = m.Id,
                Cover = m.CoverImageUrl!,
                ReleaseDate = m.ReleaseDate,

            })
            .ToListAsync();

        return movies;
    }

    public async Task<List<MovieBannerDto>> GetMovieBannersAsync()
    {
        var movies = await dbContext.Movies
            .OrderByDescending(m => m.ReleaseDate)
            .Where(m => m.ReleaseDate >= DateTime.Today) //future
            .Select(m => new MovieBannerDto
            {
                Id = m.Id,
                BannerImageUrl = m.BannerImageUrl!,
                Title = m.Title,
                Description = m.Description,
            })
            .ToListAsync();

        return movies;
    }

}
public class MockData
{
    public List<MovieDto> Movies { get; set; } = new();
}

