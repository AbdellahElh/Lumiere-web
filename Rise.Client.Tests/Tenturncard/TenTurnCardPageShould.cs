using Bunit;
using Xunit;
using Rise.Client.Tenturncards.Components;
using Rise.Shared.Tenturncards;
using Rise.Client.MockServices; 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Rise.Client.Tenturncards;

namespace Rise.Client.Tenturncards;

public class TenturnCardPageTests : TestContext
{
    [Fact]
    public void GreenButtonNavigatesToActivatePage()
    {
        // Arrange: Create and register the mock service from MockServices folder
        var mockTenturncardService = new MockTenturncardService();
        Services.AddSingleton<ITenturncardService>(mockTenturncardService);

        // Create the NavigationManager for the test
        var navigationManager = Services.GetRequiredService<NavigationManager>();
        navigationManager.NavigateTo("/account/tenturncard/activateCard");

        // Render the component
        var cut = RenderComponent<TenturncardPage>();

        // Find the green button and click it
        var greenButton = cut.Find("button.bg-green-600");
        greenButton.Click();

        // Assert that the navigation happened
        Assert.Equal("http://localhost/account/tenturncard/activateCard", navigationManager.Uri);
    }

    [Fact]
    public void BlueButtonOpensModal()
    {
        // Arrange: Create and register the mock service from MockServices folder
        var mockTenturncardService = new MockTenturncardService();
        Services.AddSingleton<ITenturncardService>(mockTenturncardService);

        // Render the component
        var cut = RenderComponent<TenturncardPage>();

        // Initially, modal is not visible
        Assert.DoesNotContain("fixed inset-0", cut.Markup);

        // Find the blue button and click it
        var blueButton = cut.Find("button.bg-blue-600");
        blueButton.Click();

        // Assert that the modal is visible
        Assert.Contains("fixed inset-0", cut.Markup);
    }
  
  
}
