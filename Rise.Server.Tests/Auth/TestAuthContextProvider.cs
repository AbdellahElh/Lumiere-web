using Auth0.ManagementApi.Models;
using Newtonsoft.Json.Linq;
using Rise.Services.Auth;
using System.Security.Claims;

namespace Rise.Server.Tests.Auth;

public class TestAuthContextProvider : IAuthContextProvider
{
    private User loggedInUser
    {
        get
        {
            var user = new User
            {
                Email = "Test@testUser.be",
                UserId = "auth0|12345"
            };
            return user;
        }
        set { }
    }
    public ClaimsPrincipal? User
    {
        get
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loggedInUser.Email),
                new Claim(ClaimTypes.NameIdentifier, loggedInUser.UserId)
            };

       

            var identity = new ClaimsIdentity(claims, "TestAuth");
            return new ClaimsPrincipal(identity);
        }
    }


    public int GetAccountIdFromMetadata(ClaimsPrincipal user)
    {
        return 2;
    }
}