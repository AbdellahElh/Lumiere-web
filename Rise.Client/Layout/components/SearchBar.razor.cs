using Microsoft.AspNetCore.Components;
using Rise.Shared.Movies;

namespace Rise.Client.Layout.components;

public partial class SearchBar
{

    private bool isSearchVisible = false;
    private List<MovieDto>? moviesFound = null;

    private FiltersDataDto filters = new FiltersDataDto();

    private string errorMessage = string.Empty;

    [Inject]
    public required IMovieService MovieService { get; set; }

    private async Task<List<MovieDto>> SearchMovies(string movieTitle)
    {
        if (string.IsNullOrWhiteSpace(movieTitle))
        {
            return new List<MovieDto>();
        }

        var filters = new FiltersDataDto { Title = movieTitle };
        var movies = await MovieService.GetMoviesAsync(filters);

        return movies;
    }

    private void ToggleSearchBar()
    {
        isSearchVisible = !isSearchVisible;
        if (!isSearchVisible)
        {
            moviesFound = null;
            filters.Title = string.Empty;
            errorMessage = string.Empty;
        }
    }

    private async Task ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(filters.Title))
        {
            errorMessage = "Invoer mag niet leeg zijn";
            moviesFound = null;
        }
        else
        {
            errorMessage = string.Empty;
            moviesFound = await SearchMovies(filters.Title);
        }
    }

    private string GetPlaceholder()
    {
        return string.IsNullOrWhiteSpace(errorMessage) ? "Film zoeken..." : errorMessage;
    }

    private string GetInputClass()
    {
        return string.IsNullOrWhiteSpace(errorMessage) ? "border-gray-300" : "border-red-500 placeholder-red-500";
    }

    private void HandleMovieSelected()
    {
        moviesFound = null;
        isSearchVisible = false;
        filters.Title = string.Empty;
    }

}
