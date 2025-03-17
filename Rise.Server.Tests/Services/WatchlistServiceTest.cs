using Auth0.ManagementApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rise.Domain.Movies;
using Rise.Shared.Accounts;
using Rise.Shared.Movies;

namespace Rise.Server.Tests.Services;

public class WatchlistServiceTest : IntegrationTest
{
    private readonly IWatchlistService _watchlistService;
    public WatchlistServiceTest(CustomWebApplicationFactory fixture)
    : base(fixture)
    {
        _watchlistService = fixture.Services.GetRequiredService<IWatchlistService>();
    }

    [Theory]
    [InlineData(1)]
    public async Task AddMovieToWatchlist_Succes(int movieId)
    {

        RegisterDto registerDto = new RegisterDto
        {
            Email = "Test@testUser.be",
            Password = "Test1234!",
        };


        await RegisterAsync(registerDto);
        await LoginAsync(registerDto);

        var movieDto = new MovieDto
        {
            Id = movieId,
            Title = "",
            Description = "",
            CoverImageUrl = "",
            Genre = "",
            Director = "",
            Cast = [],
            ReleaseDate = new DateTime(2024, 12, 11),
            MovieLink = "",
            Duration = 100
        };

        await _watchlistService.AddToWatchlistAsync(movieDto);

        var watchlist = await _dbContext.Watchlists
                                           .Include(w => w.Movies)
                                           .FirstOrDefaultAsync(w => w.UserId == 2);

        Assert.NotNull(watchlist);
        Assert.Contains(watchlist.Movies, m => m.Id == movieId);


        await _watchlistService.RemoveFromWatchlistAsync(movieId);
    }

    [Theory]
    [InlineData(2)] 
    public async Task RemoveMovieFromWatchlist_Success(int movieId)
    {
       
        const int userId = 2;

       
        var watchlist = await _dbContext.Watchlists
            .Include(w => w.Movies)
            .FirstOrDefaultAsync(w => w.UserId == userId);

        if (watchlist == null)
        {
          
            watchlist = new Watchlist { UserId = userId, Movies = new List<Movie>() };
            _dbContext.Watchlists.Add(watchlist);
            await _dbContext.SaveChangesAsync();
        }

    
        if (!watchlist.Movies.Any(m => m.Id == movieId))
        {
            var movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
            Assert.NotNull(movie); 
            watchlist.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();
        }

       
        await _watchlistService.RemoveFromWatchlistAsync(movieId);

        
        watchlist = await _dbContext.Watchlists
            .Include(w => w.Movies)
            .FirstOrDefaultAsync(w => w.UserId == userId);

      
        Assert.NotNull(watchlist); 
        Assert.Empty(watchlist.Movies);
    }


}