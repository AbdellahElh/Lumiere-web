using Bunit;
using Xunit;
using Rise.Client.Tenturncards.Components;
using Rise.Shared.Tickets;
using Rise.Client.TicketServices; 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Rise.Client.Tickets;

namespace Rise.Client.Tickets;

public class TicketsPageShould : TestContext
{
 

    [Fact]
    public void BlueButtonOpensModal()
    {
        var mockTicketService = new MockTicketService();
        Services.AddSingleton<ITicketService>(mockTicketService);

        var cut = RenderComponent<Tickets>();

        Assert.DoesNotContain("fixed inset-0", cut.Markup);

        var blueButton = cut.Find("button.bg-blue-600");
        blueButton.Click();

        Assert.Contains("fixed inset-0", cut.Markup);
    }
  
  
}
