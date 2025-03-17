using Rise.Shared.Movies;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;
using Bunit;
using Moq;
using Rise.Client.Movies.components;

namespace Rise.Client.Movies
{
    public class MoviesListShould : TestContext
    {
        private readonly Mock<IWatchlistService> _MovieServiceMock;

        public MoviesListShould()
        {
            this.AddTestAuthorization();
            // Set up the mock MovieService
            _MovieServiceMock = new Mock<IWatchlistService>();

            // Register the mock in the test DI container
            Services.AddSingleton<IWatchlistService>(_MovieServiceMock.Object);
        }

        [Fact]
        public void ShowMovieList()
        {
            var movies = new List<MovieDto>
            {
                new MovieDto
                {
                    Id = 2, Title = "Movie 2", CoverImageUrl = "cover2.jpg",
                    Genre = "Genre 2", Description = "Description 2", Duration = 150, Director = "Director 2", ReleaseDate = DateTime.Now.AddYears(-2), MovieLink = "link2",
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
                    Genre = "Genre 2", Description = "Description 2", Duration = 150, Director = "Director 2", ReleaseDate = DateTime.Now.AddYears(-2), MovieLink = "link2",
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
                },
            };

            var cut = RenderComponent<MoviesList>(parameters => parameters.Add(p => p.Movies, movies));
            cut.FindAll("ul.grid li").Count.ShouldBe(2);
        }

        [Fact]
        public void ShowNoMoviesMessage_WhenNoMoviesProvided()
        {
            var emptyMovieList = new List<MovieDto>();
            var cut = RenderComponent<MoviesList>(parameters => parameters.Add(p => p.Movies, emptyMovieList));
            cut.Markup.ShouldContain("Er zijn geen films om weer te geven.");
        }
    }
}