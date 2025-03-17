using AngleSharp.Dom;
using Rise.Shared.Movies;
using System.Net.Http.Json;
namespace Rise.Client.Movies.services;

public class MovieService : IMovieService
{
    private readonly HttpClient httpClient;

    public MovieService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<List<MovieDto>> GetMoviesAsync(FiltersDataDto args)
    {
        string url = "Movie";
        List<string> queryParams = new List<string>();
       
        if (args.pageNumber >0)
        {
            queryParams.Add($"pageNumber={args.pageNumber}&pageSize={args.pageSize}");
        }
        if (args.SelectedDate.HasValue)
        {
            queryParams.Add($"date={args.SelectedDate.Value:yyyy-MM-dd}");
        }

        if (args.SelectedCinemas != null && args.SelectedCinemas.Any())
        {
            foreach (var cinema in args.SelectedCinemas)
            {
                queryParams.Add($"cinema={Uri.EscapeDataString(cinema)}");
            }
        }

        if (!string.IsNullOrWhiteSpace(args.Title))
        {
            queryParams.Add($"title={Uri.EscapeDataString(args.Title)}");
        }

        if (queryParams.Any())
        {
            url += "?" + string.Join("&", queryParams);
        }

        var movies = await httpClient.GetFromJsonAsync<List<MovieDto>>(url);
       

        return movies!;
    }

    public async Task<MovieEventDto> GetMovieByIdAsync(int id)
    {
        var results = await httpClient.GetFromJsonAsync<MovieEventDto>($"Movie/{id}");
        return results ?? new MovieEventDto();
    }

    public async Task<List<MoviePosterDto>> GetRecentMoviePostersAsync()
    {
        var movies = await httpClient.GetFromJsonAsync<List<MoviePosterDto>>("Movie/posters");
        return movies ?? new List<MoviePosterDto>();
    }

    public async Task<List<MovieBannerDto>> GetMovieBannersAsync()
    {
        var movies = await httpClient.GetFromJsonAsync<List<MovieBannerDto>>("Movie/banners");
        return movies ?? new List<MovieBannerDto>();
    }

    public async Task<List<MoviePosterDto>> GetFutureMoviePostersAsync()
    {
        var movies = await httpClient.GetFromJsonAsync<List<MoviePosterDto>>("Movie/future-posters");
        return movies ?? new List<MoviePosterDto>();
    }
}
