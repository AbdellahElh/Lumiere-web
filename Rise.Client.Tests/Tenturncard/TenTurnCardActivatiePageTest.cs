using Moq;
using Rise.Client.MockServices;
using Rise.Client.Tenturncards;
using Rise.Client.Tenturncards.Components;
using Rise.Shared.Tenturncards;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Client.Tenturncards;

public class TenTurnCardActivatiePageTest : TestContext
{

    [Fact]
    public void TenturncardPage_Should_RenderSuccessfully()
    {
        Services.AddSingleton<ITenturncardService>(new MockTenturncardService());
        var cut = RenderComponent<TenturncardPage>();

        cut.Markup.ShouldNotBeNullOrEmpty();
    }

    [Fact]
public void NewTenturncard_Should_Be_Added()
{
    using var ctx = new TestContext();

    var mockService = new Mock<ITenturncardService>();
    mockService.Setup(service => service.AddTenturncard("a12345b"))
               .Returns(Task.CompletedTask); 

    ctx.Services.AddSingleton<ITenturncardService>(mockService.Object);

    var component = ctx.RenderComponent<TenTurnCardActivate>();

    var input = component.Find("#tenturncardCode");
    input.Change("a12345b");

    var button = component.Find("button[type='submit']");
    button.Click();

    var message = component.Find("#succesmessage");


    Assert.Contains("De tienbeurtenkaart met de code a12345b is toegevoegd", component.Markup);

    mockService.Verify(service => service.AddTenturncard("a12345b"), Times.Once);
}


    [Fact]
public void TenTurnCardActivate_Should_Show_Error_On_Invalid_Code()
{
    using var ctx = new TestContext();

    var mockService = new Mock<ITenturncardService>();
    mockService.Setup(service => service.AddTenturncard(It.IsAny<string>()))
               .ThrowsAsync(new Exception("Card not found"));
    ctx.Services.AddSingleton<ITenturncardService>(mockService.Object);

    var component = ctx.RenderComponent<TenTurnCardActivate>();

    var input = component.Find("#tenturncardCode");
    input.Change("invalid-code");

    var button = component.Find("button[type='submit']");
    button.Click();

    Assert.Contains("geen tienbeurtenkaart met de code", component.Markup);
}


}
