
using Rise.Client.Layout.components;
using Rise.Client.Movies;
using Rise.Shared.Movies;
using Shouldly;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Rise.Client.Layout;

public class SearchBarShould : TestContext
{
    public SearchBarShould()
    {
        Services.AddScoped<IMovieService, FakeMovieService>();

    }

    [Fact]
    public void Should_Display_Movies_When_Valid_Search_Is_Executed()
    {
        var validSearchTerm = "Movie";


        var searchBarComponent = RenderComponent<SearchBar>();

        var toggleButton = searchBarComponent.Find("[data-testid='search-toggle-button']");
        toggleButton.Click();

        var inputElement = searchBarComponent.Find("input");
        inputElement.Change(validSearchTerm);

        var searchButton = searchBarComponent.Find("[data-testid='search-movie-button']");
        searchButton.Click();

        var movieItems = searchBarComponent.FindAll("ul li");
        movieItems.Count.ShouldBe(2);

        var closeButton = searchBarComponent.Find("[data-testid='close-toggle-button']");
        closeButton.Click();

        var items = searchBarComponent.FindAll("ul li");
        items.Count.ShouldBe(0);
    }

    [Fact]
    public void Should_Display_Error_When_No_Search_Term()
    {
        var searchBarComponent = RenderComponent<SearchBar>();

        var toggleButton = searchBarComponent.Find("[data-testid='search-toggle-button']");
        toggleButton.Click();

        var inputElement = searchBarComponent.Find("input");
        inputElement.Change("");

        var searchButton = searchBarComponent.Find("[data-testid='search-movie-button']");
        searchButton.Click();

        var inputValue = inputElement.GetAttribute("placeholder");
        Assert.Contains("Invoer mag niet leeg zijn", inputValue);
    }

    [Fact]
    public void Should_Display_No_List_When_Valid_Search_Is_Executed()
    {
        var validSearchTerm = "Lalala";


        var searchBarComponent = RenderComponent<SearchBar>();

        var toggleButton = searchBarComponent.Find("[data-testid='search-toggle-button']");
        toggleButton.Click();

        var inputElement = searchBarComponent.Find("input");
        inputElement.Change(validSearchTerm);

        var searchButton = searchBarComponent.Find("[data-testid='search-movie-button']");
        searchButton.Click();

        var noMoviesMessage = searchBarComponent.Find("[data-testid= 'noMovies_message']");
        Assert.Contains("Geen films gevonden", noMoviesMessage.TextContent);

    }

}





