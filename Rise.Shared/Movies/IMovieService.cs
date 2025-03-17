
namespace Rise.Shared.Movies;

public interface IMovieService
{
    Task<List<MovieDto>> GetMoviesAsync(FiltersDataDto filtersData);

    Task<MovieEventDto> GetMovieByIdAsync(int id);

    Task<List<MoviePosterDto>> GetRecentMoviePostersAsync();

    Task<List<MovieBannerDto>> GetMovieBannersAsync();

    Task<List<MoviePosterDto>> GetFutureMoviePostersAsync();


}