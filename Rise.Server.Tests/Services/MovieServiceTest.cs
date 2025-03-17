using Microsoft.Extensions.DependencyInjection;
using Rise.Shared.Movies;

namespace Rise.Server.Tests.Services
{
    public class MovieServiceTest : IntegrationTest
    {
        private readonly IMovieService _movieService;

        public MovieServiceTest(CustomWebApplicationFactory fixture)
            : base(fixture)
        {
            _movieService = fixture.Services.GetRequiredService<IMovieService>();
        }

        [Theory]
        [InlineData("Matrix", "Brugge", 2024, 12, 2)]
        public async Task GetMovies_AllFiltersApplied_Success(string title, string cinema, int year, int month, int day)
        {
            var filters = new FiltersDataDto
            {
                SelectedDate = new DateTime(year, month, day),
                SelectedCinemas = new List<string> { cinema },
                Title = title
            };

            var result = await _movieService.GetMoviesAsync(filters);

            Assert.NotNull(result);
            Assert.All(result, movie =>
            {
                Assert.Contains(title, movie.Title, StringComparison.OrdinalIgnoreCase);
                Assert.Contains(movie.Cinemas, c => c.Name == cinema);
            });
        }


        [Theory]
        [InlineData(2024, 12, 2)]
        public async Task GetMovies_ByDateOnly_ReturnsFilteredMovies(int year, int month, int day)
        {
            var selectedDate = new DateTime(year, month, day);

            var filters = new FiltersDataDto
            {
                SelectedDate = selectedDate
            };

            var result = await _movieService.GetMoviesAsync(filters);

            Assert.NotNull(result);
            Assert.All(result, movie =>
            {
                Assert.All(movie.Cinemas, cinema =>
                {
                    Assert.All(cinema.Showtimes, showtime =>
                    {
                        Assert.Equal(selectedDate.Date, showtime.Date);
                    });
                });
            });
        }


        [Theory]
        [InlineData("Inception")]
        public async Task GetMovies_ByTitleOnly_ReturnsFilteredMovies(string title)
        {
            var filters = new FiltersDataDto
            {
                Title = title
            };

            var result = await _movieService.GetMoviesAsync(filters);

            Assert.NotNull(result);
            Assert.All(result, movie =>
            {
                Assert.Contains(title, movie.Title, StringComparison.OrdinalIgnoreCase);
            });
        }

        [Theory]
        [InlineData("NonExistentMovieTitle", "NonExistentCinema", 1900, 1, 1)]
        public async Task GetMovies_InvalidFilters_ReturnsEmptyList(string title, string cinema, int year, int month, int day)
        {
            var filters = new FiltersDataDto
            {
                SelectedDate = new DateTime(year, month, day),
                SelectedCinemas = new List<string> { cinema },
                Title = title
            };

            var result = await _movieService.GetMoviesAsync(filters);

            Assert.NotNull(result);
            Assert.Empty(result);
        }
        [Fact]
        public async Task GetRecentMoviePostersAsync_ReturnsMoviesOrderedByReleaseDate()
        {

            var result = await _movieService.GetRecentMoviePostersAsync();

            Assert.NotNull(result);
            Assert.True(result.Count > 0, "The result should contain movies.");

            for (int i = 0; i < result.Count - 1; i++)
            {
                Assert.True(result[i].ReleaseDate >= result[i + 1].ReleaseDate,
                            $"Movies are not sorted correctly. {result[i].ReleaseDate} is not >= {result[i + 1].ReleaseDate}");
            }
        }
    }
}