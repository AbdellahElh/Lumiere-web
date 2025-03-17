using Xunit;
using Bunit;
using System.Collections.Generic;
using Rise.Client.Movies.components;
using Shouldly;
using Rise.Shared.Movies;

namespace Rise.Client.Movies.Tests
{
    public class MoviePostersTests : TestContext
    {
        [Fact]
        public void Component_Should_RenderSuccessfully()
        {
            var cut = RenderComponent<MoviePosters>(parameters => parameters.Add(p => p.MoviePosterDtos, new List<MoviePosterDto>()));

            cut.Markup.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public void MoviePosters_ShouldHandleEmptyMoviesList()
        {
            
            var emptyMovies = new List<MoviePosterDto>();

            var cut = RenderComponent<MoviePosters>(parameters => parameters.Add(p => p.MoviePosterDtos, emptyMovies));

            var result = cut.Instance.MoviePosterDtos;
            result.ShouldBeEmpty();
            cut.Markup.ShouldContain("No movies available to display.");
        }
    }
}