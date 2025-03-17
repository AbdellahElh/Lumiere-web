using Xunit;
using Bunit;
using Rise.Client.Account.components;
using Shouldly;

namespace Rise.Client.Account;

public class AccountPageTests : TestContext
{
    [Fact]
    public void AccountPage_Should_RenderSuccessfully()
    {
        // Act
        var cut = RenderComponent<Account>();

        // Assert
        cut.Markup.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public void AccountPage_ShouldDisplayAccountOverzichtHeading()
    {
        // Arrange
        var cut = RenderComponent<Account>();

        // Act
        var heading = cut.Find("h1");

        // Assert
        heading.TextContent.ShouldContain("Account Overzicht");
    }

    [Fact]
    public void AccountPage_ShouldIncludeSidebarComponent()
    {
        // Arrange
        var cut = RenderComponent<Account>();

        // Act & Assert
        cut.FindComponent<Sidebar>().ShouldNotBeNull();
    }

    [Fact]
    public void AccountPage_ShouldIncludeWelkomComponent()
    {
        // Arrange
        var cut = RenderComponent<Account>();

        // Act & Assert
        cut.FindComponent<Welkom>().ShouldNotBeNull();
    }
}