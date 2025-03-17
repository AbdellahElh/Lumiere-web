// MovieInfo.razor.cs
using Microsoft.AspNetCore.Components;
using Rise.Shared.Movies;

namespace Rise.Client.Movies;

public partial class MovieInfo : ComponentBase
{
    [Parameter]
    public int IdMovie { get; set; }

    private MovieEventDto? foundItem;
    private bool isLoading = true;

    [Inject]
    public required IMovieService MovieService { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        foundItem = await MovieService.GetMovieByIdAsync(IdMovie);
        isLoading = false;
    }
}
