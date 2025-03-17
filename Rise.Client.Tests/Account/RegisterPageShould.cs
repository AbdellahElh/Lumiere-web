using Moq;
using Rise.Client.MockServices;
using Rise.Shared.Accounts;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Client.Account;

public class RegisterPageShould : TestContext
{
    [Fact]
    public void RegisterPage_Should_RenderSuccessfully()
    {
        Services.AddSingleton<IAccountService>(new MockUserService());
        var cut = RenderComponent<Register>();

        cut.Markup.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public void show_error_if_passwords_do_not_match()
    {
        using var ctx = new TestContext();

        var mockService = new Mock<IAccountService>();
        ctx.Services.AddSingleton<IAccountService>(mockService.Object);

        var component = ctx.RenderComponent<Register>();

        var password = component.Find("#password");
        password.Change("test");

        var passwordRepeat = component.Find("#passwordRepeat");
        passwordRepeat.Change("test12");

        var button = component.Find("button[type='submit']");
        button.Click();

        component.Markup.ShouldContain("Wachtwoorden moeten overeen komen");

    }
    [Fact]
    public void show_error_if_email_is_invalid()
    {
        using var ctx = new TestContext();

        var mockService = new Mock<IAccountService>();
        mockService.Setup(service => service.AddUserAsync(It.IsAny<RegisterDto>()))
                   .ThrowsAsync(new Exception("Geen geldig emailadres"));
        ctx.Services.AddSingleton<IAccountService>(mockService.Object);

        var component = ctx.RenderComponent<Register>();

        var email = component.Find("#email");
        email.Change("test");

        var password = component.Find("#password");
        password.Change("Test1234!");

        var passwordRepeat = component.Find("#passwordRepeat");
        passwordRepeat.Change("Test1234!");

        var button = component.Find("button[type='submit']");
        button.Click();

        component.Markup.ShouldContain("Geen geldig emailadres");

    }
}
