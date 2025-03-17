using Microsoft.AspNetCore.Mvc;
using Rise.Shared.Movies;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Serilog;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WatchlistController : ControllerBase
{
    private readonly IWatchlistService _watchlistService;

    public WatchlistController(IWatchlistService watchlistService)
    {
        _watchlistService = watchlistService;
    }

    [HttpGet]
    public async Task<ActionResult<List<MovieDto>>> GetWatchlist()
    {
        Log.Information("Fetching watchlist for the current user.");

        var watchlist = await _watchlistService.LoadWatchlistAsync();
        return Ok(watchlist);
    }

    [HttpPost]
    public async Task<ActionResult> AddToWatchlist([FromBody] MovieDto movie)
    {
        if (movie == null)
        {
            Log.Warning("Movie object received is null.");
            return BadRequest("Movie object is null");
        }

        if (movie.Id <= 0)
        {
            Log.Warning("Invalid movie ID received: {MovieId}", movie.Id);
            return BadRequest("Invalid movie ID");
        }

        Log.Information("Adding movie with ID {MovieId} to the watchlist.", movie.Id);
        Log.Debug("Received movie data: {MovieData}", JsonSerializer.Serialize(movie));

        await _watchlistService.AddToWatchlistAsync(movie);
        return Ok(new { message = "Movie added to watchlist successfully." });
    }

    [HttpDelete("{movieId}")]
    public async Task<ActionResult> RemoveFromWatchlist(int movieId)
    {
        Log.Information("Removing movie with ID {MovieId} from the watchlist.", movieId);

        await _watchlistService.RemoveFromWatchlistAsync(movieId);
        return Ok(new { message = "Movie removed from watchlist successfully." });
    }
}
