using Xunit;
using Bunit;
using Bunit.TestDoubles;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rise.Client.Account.components;
using Rise.Shared.Movies;
using Shouldly;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System;
using Microsoft.AspNetCore.Components;
using Rise.Client.Movies.components;

namespace Rise.Client.Tests.Account
{
    public class MockAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly Task<AuthenticationState> _authenticationState;

        public MockAuthenticationStateProvider(Task<AuthenticationState> authenticationState)
        {
            _authenticationState = authenticationState;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return _authenticationState;
        }
    }

    public class WatchlistShould : TestContext
    {
        private readonly Mock<IWatchlistService> _watchlistServiceMock;

        public WatchlistShould()
        {
            _watchlistServiceMock = new Mock<IWatchlistService>();

            Services.AddSingleton<IWatchlistService>(_watchlistServiceMock.Object);

            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Email, "testuser2@test.be")
            }, "mock"))));

            Services.AddSingleton<AuthenticationStateProvider>(new MockAuthenticationStateProvider(authState));
        }

        [Fact]
        public void DisplayEmptyMessage_WhenNoMoviesInWatchlist()
        {
            var emptyMovies = new List<MovieDto>();
            _watchlistServiceMock.Setup(service => service.LoadWatchlistAsync()).ReturnsAsync(emptyMovies);

            var cut = RenderComponent<CascadingAuthenticationState>(parameters => parameters
                .AddChildContent<Watchlist>()
            );

            cut.WaitForAssertion(() => cut.Markup.ShouldContain("Je hebt geen films in je watchlist."));
            cut.Markup.ShouldContain("Je hebt geen films in je watchlist.");
        }


        [Fact]
        public void RemoveMovieFromWatchlist_WhenRemoveButtonClicked()
        {
            var movie = new MovieDto
            {
                Id = 1,
                Title = "Test Movie",
                CoverImageUrl = "cover.jpg",
                Genre = "Action",
                Description = "Test Description",
                Duration = 120,
                Director = "Test Director",
                ReleaseDate = new DateTime(2021, 1, 1),
                MovieLink = "testlink"
            };

            _watchlistServiceMock.Setup(service => service.RemoveFromWatchlistAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            _watchlistServiceMock.Setup(service => service.LoadWatchlistAsync())
                .ReturnsAsync(new List<MovieDto> { movie });

            var cut = RenderComponent<CascadingAuthenticationState>(parameters => parameters
                .AddChildContent<Watchlist>()
            );

            
            var removeIcon = cut.Find("[data-test-id='removeIcon']");
            removeIcon.ShouldNotBeNull();
            removeIcon.Click();

            
            _watchlistServiceMock.Verify(service => service.RemoveFromWatchlistAsync(movie.Id), Times.Once);
        }

        [Fact]
        public void AddMovieToWatchlist_WhenStarButtonClicked()
        {
            
            var movie = new MovieDto
            {
                Id = 1,
                Title = "Test Movie",
                CoverImageUrl = "cover.jpg",
                Genre = "Action",
                Description = "Test Description",
                Duration = 120,
                Director = "Test Director",
                ReleaseDate = new DateTime(2021, 1, 1),
                MovieLink = "testlink"
            };

            _watchlistServiceMock.Setup(service => service.AddToWatchlistAsync(It.IsAny<MovieDto>()))
                .Returns(Task.CompletedTask);

            _watchlistServiceMock.Setup(service => service.LoadWatchlistAsync())
                .ReturnsAsync(new List<MovieDto>());

            var cut = RenderComponent<CascadingAuthenticationState>(parameters => parameters
                .AddChildContent<MovieCard>(movieCardParams => movieCardParams
                    .Add(p => p.MovieData, movie)
                    .Add(p => p.IsInWatchlistPage, false)
                    .Add(p => p.OnStarStatusChanged, EventCallback.Factory.Create<bool>(this, async isStarred =>
                    {
                        if (isStarred)
                        {
                            await _watchlistServiceMock.Object.AddToWatchlistAsync(movie);
                        }
                    }))
                    .Add(p => p.IsAuthenticated, true)
                )
            );

            var regularStar = cut.Find("[data-test-id='regularStar']");
            regularStar.ShouldNotBeNull();
            regularStar.Click();

            _watchlistServiceMock.Verify(service => service.AddToWatchlistAsync(It.Is<MovieDto>(m => m.Id == movie.Id)), Times.Once);
        }


        [Fact]
        public void EnsureUserIsAuthenticated()
        {
            var authStateProvider = Services.GetRequiredService<AuthenticationStateProvider>();
            var authState = authStateProvider.GetAuthenticationStateAsync().Result;

            authState?.User?.Identity?.IsAuthenticated.ShouldBeTrue();
            authState?.User?.Identity?.Name.ShouldBe("Test User");
        }
    }
}
