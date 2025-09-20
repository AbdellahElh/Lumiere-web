using Microsoft.AspNetCore.Mvc;
using Rise.Shared.Movies;
namespace Rise.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieService movieService;

    public MovieController(IMovieService movieService)
    {
        this.movieService = movieService;
    }


    /* Temporarily disabled due to PostgreSQL timezone issues
    [HttpGet]
    public async Task<ActionResult<List<MovieDto>>> Get([FromQuery] DateTime? date = null, [FromQuery] List<string> cinema = null,[FromQuery] string? title = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)

    {
        var filters = new FiltersDataDto
        {
            SelectedDate = date,
            SelectedCinemas = cinema,
            Title = title,
            pageNumber = pageNumber,
            pageSize = pageSize

        };
        var movies = await movieService.GetMoviesAsync(filters);
        return Ok(movies);
    }
    */

    [HttpGet]
    public async Task<ActionResult<List<object>>> Get()
    {
        try
        {
            var movies = await movieService.GetSimpleMoviesAsync();
            return Ok(movies);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

     [HttpGet("{id}")]
    public async Task<ActionResult<MovieEventDto>> GetById(int id)
    {
        var result = await movieService.GetMovieByIdAsync(id);
        if (result == null)
        {
            return NotFound($"Movie with ID {id} not found.");
        }

       
        return Ok(result);
    }

        /* Temporarily disabled due to PostgreSQL timezone issues
    [HttpGet("posters")]
    public async Task<ActionResult<IEnumerable<MoviePosterDto>>> GetRecentMoviePosters()
    {
        var movies = await movieService.GetRecentMoviePostersAsync();
        if (movies == null)
        {
            return NotFound();
        }
        return Ok(movies);
    }

    [HttpGet("future-posters")]
    // android needs posters but for future movies, the existing /posters returns recent movies, therefore this endpoint
    public async Task<ActionResult<IEnumerable<MoviePosterDto>>> GetFutureMoviePosters()
    {
        var movies = await movieService.GetFutureMoviePostersAsync();
        if (movies == null)
        {
            return NotFound();
        }
        return Ok(movies);
    }

    [HttpGet("banners")]
    public async Task<ActionResult<IEnumerable<MovieBannerDto>>> GetMovieBanners()
    {
        var movies = await movieService.GetMovieBannersAsync();
        if (movies == null)
        {
            return NotFound();
        }
        return Ok(movies);
    }
    */

    [HttpGet("simple")]
    public async Task<ActionResult<IEnumerable<object>>> GetSimpleMovies()
    {
        try
        {
            var movies = await movieService.GetSimpleMoviesAsync();
            return Ok(movies);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpGet("mock")]
    public ActionResult<IEnumerable<object>> GetMockMovies()
    {
        var mockMovies = new[]
        {
            new { Id = 1, Title = "Test Movie 1", Description = "A test movie" },
            new { Id = 2, Title = "Test Movie 2", Description = "Another test movie" },
            new { Id = 3, Title = "Test Movie 3", Description = "Yet another test movie" }
        };
        return Ok(mockMovies);
    }

}
