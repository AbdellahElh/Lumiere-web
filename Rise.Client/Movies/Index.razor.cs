using Microsoft.AspNetCore.Components;
using Rise.Shared.Movies;
using Rise.Client.Movies.components;

namespace Rise.Client.Movies;

public partial class Index
{    
    [Inject] public required IMovieService MovieService { get; set; }

    private List<MovieDto>? movies;
    private List<MoviePosterDto>? moviePosters;

   
   
    private int TotalPages { get; set; }
    private int TotalMovies { get; set; }
    private FiltersDataDto filters = new()
    {
        SelectedDate = DateTime.Today,
        SelectedCinemas = new List<string> { "Brugge", "Antwerpen", "Mechelen", "Cinema Cartoons" },
        pageNumber = 1,
        pageSize = 4
    };
   
    protected override async Task OnInitializedAsync()
    {
       

        await LoadMovies();
        moviePosters = await MovieService.GetRecentMoviePostersAsync();   
    }
     private async Task LoadMovies()
    {

        var countFilters = new FiltersDataDto
        {
            SelectedDate = filters.SelectedDate,
            SelectedCinemas = filters.SelectedCinemas
        };

        var allMovies = await MovieService.GetMoviesAsync(countFilters);
        TotalMovies = allMovies.Count;

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
