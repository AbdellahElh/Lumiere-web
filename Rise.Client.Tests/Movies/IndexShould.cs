using Moq;
using Rise.Client.Movies;
using Rise.Shared.Movies;
using Xunit.Abstractions;
using Shouldly;
using System.Collections.Generic;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Rise.Client.Tests.Account;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rise.Client.Movies;

public class IndexShould : TestContext
{
    private readonly Mock<IWatchlistService> _watchlistServiceMock;

    public IndexShould(ITestOutputHelper outputHelper)
    {
        Services.AddXunitLogger(outputHelper);
        Services.AddScoped<IMovieService, FakeMovieService>();

        _watchlistServiceMock = new Mock<IWatchlistService>();
        Services.AddSingleton<IWatchlistService>(_watchlistServiceMock.Object);
        var emptyMovies = new List<MovieDto>();
        _watchlistServiceMock.Setup(service => service.LoadWatchlistAsync()).ReturnsAsync(emptyMovies);

        var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Email, "testuser2@test.be")
            }, "mock"))));

        Services.AddSingleton<AuthenticationStateProvider>(provider => new MockAuthenticationStateProvider(authState));

    }

    [Fact]
    public void ShowsAllMovies_Sucess()
    {
        var cut = RenderComponent<Index>();
        var movieItems = cut.FindAll("ul li");

        movieItems.Count.ShouldBe(2);
    }

}
