using Bunit;
using Xunit;
using System.Collections.Generic;
using Rise.Shared.Movies;
using Rise.Client.Movies.components; 
using Moq;
using System;

namespace Rise.Client.Tests.Movies;

public class BannerTests : TestContext
{
    private readonly Mock<IMovieService> _movieServiceMock;

    public BannerTests()
    {
        // Initialize the mock
        _movieServiceMock = new Mock<IMovieService>();

        // Register the mock service with the TestContext's service collection
        Services.AddSingleton<IMovieService>(_movieServiceMock.Object);
    }

    [Fact]
    public void Banner_DisplaysImages_WhenMoviesArePresent()
    {
        // Arrange: Define the list of movie banner DTOs with image URLs
        var movieBanners = new List<MovieBannerDto>
        {
            new MovieBannerDto { Id = 1, BannerImageUrl = "/images/up.jpg", Title = "Up", Description = "An animated movie." },
            new MovieBannerDto { Id = 2, BannerImageUrl = "/images/avengers.jpg", Title = "Avengers", Description = "Superhero movie." }
        };

        // Setup the mock to return the movieBanners when GetMovieBannersAsync is called
        _movieServiceMock.Setup(service => service.GetMovieBannersAsync())
                         .ReturnsAsync(movieBanners);

        // Act: Render the Banner component without passing MovieBannerDtos via parameters
        var cut = RenderComponent<Banner>();

        // Wait for the component to finish rendering async operations
        cut.WaitForState(() => cut.FindAll("img").Count == 2, timeout: TimeSpan.FromSeconds(1));

        // Assert: Ensure both images are displayed
        var imgElements = cut.FindAll("img");
        Assert.Equal(2, imgElements.Count);
    }

    [Fact]
    public void Banner_PrevSlide_ChangesSlideIndex()
    {
        // Arrange
        var movieBanners = new List<MovieBannerDto>
        {
            new MovieBannerDto { Id = 1, BannerImageUrl = "/images/up.jpg", Title = "Up", Description = "An animated movie." },
            new MovieBannerDto { Id = 2, BannerImageUrl = "/images/avengers.jpg", Title = "Avengers", Description = "Superhero movie." }
        };

        _movieServiceMock.Setup(service => service.GetMovieBannersAsync())
                         .ReturnsAsync(movieBanners);

        var cut = RenderComponent<Banner>();

        // Wait for images to render
        cut.WaitForState(() => cut.FindAll("img").Count == 2, timeout: TimeSpan.FromSeconds(1));

        // Act: Find and click the prev button
        var prevButton = cut.Find("[data-cy='prev-slide']");
        Assert.NotNull(prevButton);
        prevButton.Click();

        // Assert: Check if the slide index changed (style change to translate the slide)
        var slideContainer = cut.Find("div.flex.transition-transform");
        Assert.Contains("translate3d(-100%", slideContainer.GetAttribute("style"));
    }

    [Fact]
    public void Banner_NextSlide_ChangesSlideIndex()
    {
        // Arrange: Define the list of movie banner DTOs with image URLs
        var movieBanners = new List<MovieBannerDto>
        {
            new MovieBannerDto { Id = 1, BannerImageUrl = "/images/up.jpg", Title = "Up", Description = "An animated movie." },
            new MovieBannerDto { Id = 2, BannerImageUrl = "/images/avengers.jpg", Title = "Avengers", Description = "Superhero movie." }
        };

        // Setup the mock to return the movieBanners when GetMovieBannersAsync is called
        _movieServiceMock.Setup(service => service.GetMovieBannersAsync())
                        .ReturnsAsync(movieBanners);

        // Act: Render the Banner component
        var cut = RenderComponent<Banner>();

        // Wait for the images to render
        cut.WaitForState(() => cut.FindAll("img").Count == 2, timeout: TimeSpan.FromSeconds(1));

        // Act: Find and click the next button
        var nextButton = cut.Find("[data-cy='next-slide']");
        Assert.NotNull(nextButton);
        nextButton.Click();

        // Wait for the component to re-render and the style to change
        cut.WaitForState(() =>
        {
            var slideContainer = cut.Find("div.flex.transition-transform");
            return slideContainer.GetAttribute("style").Contains("translate3d(-100%");
        }, timeout: TimeSpan.FromSeconds(1));

        // Assert: Check if the style attribute contains the translate3d(-100%)
        var updatedSlideContainer = cut.Find("div.flex.transition-transform");
        Assert.Contains("translate3d(-100%", updatedSlideContainer.GetAttribute("style"));
    }


}
