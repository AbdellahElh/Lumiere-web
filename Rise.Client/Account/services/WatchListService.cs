using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Rise.Shared.Movies;

namespace Rise.Client.Account.services
{
    public class WatchListService : IWatchlistService
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<int, bool> MovieStarStatus = new();

        public WatchListService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<MovieDto>> LoadWatchlistAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<MovieDto>>("Watchlist");
            if (response == null)
            {
                Console.WriteLine("Warning: Received empty watchlist response from the server.");
            }
            return response ?? new List<MovieDto>();
        }

        public async Task AddToWatchlistAsync(MovieDto movie)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "Watchlist")
            {
                Content = JsonContent.Create(movie)
            };
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            await _httpClient.SendAsync(request);

            MovieStarStatus[movie.Id] = true;
        }

        public async Task RemoveFromWatchlistAsync(int movieId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Watchlist/{movieId}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Movie with ID {movieId} successfully removed from the watchlist.");
                    MovieStarStatus[movieId] = false;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Movie with ID {movieId} not found on server, likely already deleted.");
                    MovieStarStatus[movieId] = false;
                }
                else
                {
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while removing movie {movieId}: {ex.Message}");
                throw;
            }
        }


        public async Task<bool> IsMovieStarredAsync(int movieId)
        {
            if (!MovieStarStatus.ContainsKey(movieId))
            {
                var watchlist = await LoadWatchlistAsync();
                foreach (var movie in watchlist)
                {
                    MovieStarStatus[movie.Id] = true;
                }
            }
            return MovieStarStatus.TryGetValue(movieId, out var isStarred) && isStarred;
        }
    }
}
