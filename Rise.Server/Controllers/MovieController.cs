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

}
