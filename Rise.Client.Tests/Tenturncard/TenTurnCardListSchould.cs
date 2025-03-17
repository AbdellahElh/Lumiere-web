using Bunit;
using Xunit;
using Rise.Client.Tenturncards.Components; // Your component's namespace
using Rise.Shared.Tenturncards;
using System;
using System.Collections.Generic;
using Moq;
using System.Threading.Tasks;

namespace Rise.Client.Tenturncards;

public class TenTurnCardListTests : TestContext
{
    [Fact]
    public void RittenKaartenIsNull_DisplaysNoCardsMessage()
    {
        // Arrange: Render the component with null RittenKaarten
        var cut = RenderComponent<TenTurnCardList>(parameters =>
            parameters.Add(p => p.RittenKaarten, null));

        // Assert: Check that the "Geen rittenkaarten beschikbaar." message is displayed
        cut.Markup.Contains("Geen rittenkaarten beschikbaar.");
    }

    [Fact]
    public void RittenKaartenIsEmpty_DisplaysNoCardsMessage()
    {
        // Arrange: Render the component with an empty RittenKaarten list
        var cut = RenderComponent<TenTurnCardList>(parameters =>
            parameters.Add(p => p.RittenKaarten, new List<TenturncardDto>()));

        // Assert: Check that the "Geen rittenkaarten beschikbaar." message is displayed
        cut.Markup.Contains("Geen rittenkaarten beschikbaar.");
    }

    [Fact]
    public void RittenKaartenHasData_DisplaysCards()
    {
        // Arrange: Create a list of TenturncardDto with mock data
        var mockRittenKaarten = new List<TenturncardDto>
        {
            new TenturncardDto
            {
                AmountLeft = 5,
                PurchaseDate = DateTime.Now.AddMonths(-2),
                ExpirationDate = DateTime.Now.AddMonths(6)
            },
            new TenturncardDto
            {
                AmountLeft = 8,
                PurchaseDate = DateTime.Now.AddMonths(-1),
                ExpirationDate = DateTime.Now.AddMonths(12)
            }
        };

        // Arrange: Render the component with the mock data
        using var ctx = new TestContext();

        var mockService = new Mock<ITenturncardService>();

        ctx.Services.AddSingleton<ITenturncardService>(mockService.Object);

        var cut = ctx.RenderComponent<TenTurnCardList>(parameters =>
            parameters.Add(p => p.RittenKaarten, mockRittenKaarten));


        // Assert: Check that the component renders two items
        var listItems = cut.FindAll("li");
        Assert.Equal(2, listItems.Count);

        // Optionally, check for specific content in each card (e.g., AmountLeft)
        Assert.Contains("5", cut.Markup);  // Check if 5 is displayed in one of the cards
        Assert.Contains("2", cut.Markup);  // Check if 8 is displayed in one of the cards
    }
}
