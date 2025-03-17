using Microsoft.AspNetCore.Components;
using Rise.Shared.Movies;
using Rise.Client.Movies.components;

namespace Rise.Client.Movies;

public partial class MoviesProgram
{
    [Inject] public required IMovieService MovieService { get; set; }

    private List<MovieDto>? movies;
    private int TotalPages { get; set; }
    private int TotalMovies { get; set; }

    [Parameter]
    [SupplyParameterFromQuery]
    public DateTime? SelectedDate { get; set; }

    [Parameter]
    [SupplyParameterFromQuery]
    public string? SelectedCinemas { get; set; }

    private static readonly List<string> DefaultCinemas = new()
    {
        "Brugge", "Antwerpen", "Mechelen", "Cinema Cartoons"
    };

    private FiltersDataDto filters = new()
    {
        SelectedDate = DateTime.Today,
        SelectedCinemas = DefaultCinemas,
        pageNumber = 1,
        pageSize = 4
    };

    protected override async Task OnParametersSetAsync()
    {
        filters.SelectedDate = SelectedDate ?? DateTime.Today;
        filters.SelectedCinemas = string.IsNullOrEmpty(SelectedCinemas)
            ? DefaultCinemas
            : SelectedCinemas.Split(',').ToList();

        await LoadMovies();
    }

    private async Task LoadMovies()
    {


        var countFilters = new FiltersDataDto
        {
            SelectedDate = filters.SelectedDate,
            SelectedCinemas = filters.SelectedCinemas
        };

        var allMovies = await MovieService.GetMoviesAsync(countFilters);
        TotalMovies = allMovies?.Count ?? 0;

        TotalPages = (int)Math.Ceiling((decimal)TotalMovies / (decimal)filters.pageSize);
        if (filters.pageNumber > TotalPages)
        {
            filters.pageNumber = 1;
        }
        movies = await MovieService.GetMoviesAsync(filters);

    }

    private async Task NextPage()
    {
        if (CanNext)
        {
            filters.pageNumber++;
            await LoadMovies();
        }
    }

    private async Task PreviousPage()
    {
        if (CanPrevious)
        {
            filters.pageNumber--;
            await LoadMovies();
        }
    }

    private async Task GoToPage(int pageNumber)
    {
        filters.pageNumber = pageNumber;
        await LoadMovies();
    }

    private bool CanNext => filters.pageNumber < TotalPages;
    private bool CanPrevious => filters.pageNumber > 1;

   
}