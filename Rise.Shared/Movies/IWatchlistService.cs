using System.Collections.Generic;
using System.Threading.Tasks;
using Rise.Shared.Movies;

public interface IWatchlistService
{
    Task<List<MovieDto>> LoadWatchlistAsync();
    Task AddToWatchlistAsync(MovieDto movie);
    Task RemoveFromWatchlistAsync(int movieId);
    Task<bool> IsMovieStarredAsync(int movieId);
}