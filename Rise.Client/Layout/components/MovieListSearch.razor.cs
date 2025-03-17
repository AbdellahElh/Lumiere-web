using Microsoft.AspNetCore.Components;
using Rise.Shared.Movies;

namespace Rise.Client.Layout.components;

public partial class MovieListSearch
{
    [Parameter]
    public List<MovieDto> MoviesFound { get; set; } = new List<MovieDto>();

    [Parameter]
    public EventCallback<MovieDto> OnMovieSelected { get; set; }

    [Inject] NavigationManager Navigation { get; set; } = null!;

    private async Task SelectMovie(MovieDto movie)
    {
        if (OnMovieSelected.HasDelegate)
        {
            await OnMovieSelected.InvokeAsync(movie);
        }
    }

    private void HandleMovieSelected(MovieDto selectedMovie)
    {

        Navigation.NavigateTo($"/Movie/{selectedMovie.Id}");
        OnMovieSelected.InvokeAsync(selectedMovie);
    }
}
