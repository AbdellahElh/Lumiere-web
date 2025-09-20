using Rise.Shared.Movies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rise.Client.Movies;
internal class FakeMovieService : IMovieService
{
    public Task<MovieEventDto> GetMovieByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<MoviePosterDto>> GetRecentMoviePostersAsync()
    {
        var moviePosters = new List<MoviePosterDto>
        {
            new() { Id = 1, Cover = "poster1.jpg", ReleaseDate = DateTime.Now.AddYears(-1) },
            new() { Id = 2, Cover = "poster2.jpg", ReleaseDate = DateTime.Now.AddYears(-2) },
        };
        return Task.FromResult(moviePosters);
    }

    public Task<List<MovieDto>> GetMoviesAsync(FiltersDataDto filters)
    {
        var movies = new List<MovieDto>
        {
            new MovieDto
            {
                Id = 1, Title = "Movie 1", CoverImageUrl = "cover1.jpg",
                Genre = "Action",
                Description = "An action-packed movie.",
                Duration = 120,
                Director = "Director 1",
                ReleaseDate = DateTime.Now.AddYears(-1),
                MovieLink = "http://example.com/movie1",
                Cinemas = new List<CinemaDto>
                {
                    new CinemaDto
                    {
                        Name = "Cinema 1",
                        Showtimes = new List<DateTime>
                        {
                            DateTime.Now.AddHours(1),
                            DateTime.Now.AddHours(3)
                        }
                    }
                }
            },
            new MovieDto
            {
                Id = 2, Title = "Movie 2", CoverImageUrl = "cover2.jpg",
                Genre = "Drama",
                Description = "A dramatic movie.",
                Duration = 140,
                Director = "Director 2",
                ReleaseDate = DateTime.Now.AddYears(-2),
                MovieLink = "http://example.com/movie2",
                Cinemas = new List<CinemaDto>
                {
                    new CinemaDto
                    {
                        Name = "Cinema 1",
                        Showtimes = new List<DateTime>
                        {
                            DateTime.Now.AddHours(2),
                            DateTime.Now.AddHours(4)
                        }
                    },
                    new CinemaDto
                    {
                        Name = "Cinema 2",
                        Showtimes = new List<DateTime>
                        {
                            DateTime.Now.AddHours(5),
                            DateTime.Now.AddHours(7)
                        }
                    }
                }
            }
        };

        if (filters.Title == "Lalala")
        { 
            return Task.FromResult(new List<MovieDto>());
        }

        return Task.FromResult(movies);
    }

    public Task<List<MovieBannerDto>> GetMovieBannersAsync()
    {
        var movieBanners = new List<MovieBannerDto>
        {
            new() { Id = 1, BannerImageUrl = "banner1.jpg", Title = "Movie 1", Description = "Description 1" },
            new() { Id = 2, BannerImageUrl = "banner2.jpg", Title = "Movie 2", Description = "Description 2" },
        };
        return Task.FromResult(movieBanners);
    }

    public Task<List<MoviePosterDto>> GetFutureMoviePostersAsync()
    {
        var moviePosters = new List<MoviePosterDto>
        {
            new() { Id = 1, Cover = "poster1.jpg", ReleaseDate = DateTime.Now.AddDays(100) },
            new() { Id = 2, Cover = "poster2.jpg", ReleaseDate = DateTime.Now.AddDays(200) },
        };
        return Task.FromResult(moviePosters);
    }

    public Task<List<object>> GetSimpleMoviesAsync()
    {
        var simpleMovies = new List<object>
        {
            new { Id = 1, Title = "Test Movie 1", Description = "A test movie" },
            new { Id = 2, Title = "Test Movie 2", Description = "Another test movie" },
        };
        return Task.FromResult(simpleMovies);
    }
}
