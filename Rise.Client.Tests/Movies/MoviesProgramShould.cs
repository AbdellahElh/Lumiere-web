using Moq;
using Rise.Client.Movies;
using Rise.Shared.Movies;
using Bunit;
using Xunit;
using Shouldly;
using System.Collections.Generic;
using Rise.Client.Movies.components;
using Moq.Protected;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Rise.Client.Movies;

public class MoviesProgramShould : TestContext
{
    private readonly Mock<IMovieService> _movieServiceMock;

    private readonly Mock<IWatchlistService> _watchlistServiceMock;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

    public MoviesProgramShould()
    {
        this.AddTestAuthorization();
        // Set up a mock movie service
        _movieServiceMock = new Mock<IMovieService>();
        Services.AddSingleton(_movieServiceMock.Object);

        _watchlistServiceMock = new Mock<IWatchlistService>();
        Services.AddSingleton<IWatchlistService>(_watchlistServiceMock.Object);
        var emptyMovies = new List<MovieDto>();
        _watchlistServiceMock.Setup(service => service.LoadWatchlistAsync()).ReturnsAsync(emptyMovies);

        _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[]")
            });


    }

    // Test the "Loading" message when movies are not loaded
    [Fact]
    public void ShowLoadingMessageWhenMoviesAreNotLoaded()
    {
        // Arrange
        _movieServiceMock.Setup(service => service.GetMoviesAsync(It.IsAny<FiltersDataDto>()))
            .Returns(Task.Delay(1000).ContinueWith(_ => new List<MovieDto>())); // Simulate loading state

        var cut = RenderComponent<MoviesProgram>(); 

        // Act
        var loadingText = cut.Find("p.text-center");

        // Assert
        loadingText.TextContent.ShouldBe("Loading movies...");
    }


    // Test the "No Movies Available" message when the movie list is empty
    [Fact]
    public void ShowNoMoviesMessageWhenMoviesListIsEmpty()
    {
        // Arrange: Simulate an empty movie list
        _movieServiceMock.Setup(service => service.GetMoviesAsync(It.IsAny<FiltersDataDto>()))
            .ReturnsAsync(new List<MovieDto>());  // Empty list of movies

        var cut = RenderComponent<MoviesProgram>();  // Automatically invokes OnInitializedAsync()

        // Assert: The component should display the "Geen films beschikbaar." message
        var noMoviesText = cut.Find("p.text-center");
        noMoviesText.TextContent.ShouldBe("Geen films beschikbaar.");
    }

    // Test the "Movies Available" message when there are movies
    [Fact]
    public void ShowMoviesWhenMoviesAreAvailable()
    {
        // Arrange: Mock a list of movies
        var mockMovies = new List<MovieDto>
        {
            new MovieDto { Id = 1, Title = "Movie 1", Genre = "Genre 1", Description = "Description 1", Duration = 120, Director = "Director 1", ReleaseDate = DateTime.Now, MovieLink = "http://example.com/movie1" },
            new MovieDto { Id = 2, Title = "Movie 2", Genre = "Genre 2", Description = "Description 2", Duration = 130, Director = "Director 2", ReleaseDate = DateTime.Now, MovieLink = "http://example.com/movie2" }
        };

        _movieServiceMock.Setup(service => service.GetMoviesAsync(It.IsAny<FiltersDataDto>()))
            .ReturnsAsync(mockMovies);  // Return a list of movies

        var cut = RenderComponent<MoviesProgram>();  // Automatically invokes OnInitializedAsync()

        // Assert: The component should render the MoviesList component with the mock movies
        var movieListComponent = cut.FindComponent<MoviesList>();
        movieListComponent.Instance.Movies.ShouldBe(mockMovies);
    }
}
