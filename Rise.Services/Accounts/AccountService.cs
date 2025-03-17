using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Rise.Domain.Accounts;
using Rise.Domain.Exceptions;
using Rise.Domain.Movies;
using Rise.Persistence;
using Rise.Shared.Accounts;

namespace Rise.Services.Accounts;

public class AccountService : IAccountService
{
    private readonly ApplicationDbContext dbContext;
    private readonly IManagementApiClient _managementApiClient;
    public AccountService(ApplicationDbContext dbContext, IManagementApiClient managementApiClient)
    {
        this.dbContext = dbContext;
        this._managementApiClient = managementApiClient;
    }

    public async Task AddUserAsync(RegisterDto registerDto)
    {
        //check if user already exists in our db
        var email = registerDto.Email;
        if(dbContext.Accounts.Any(a => a.Email == email))
        {
            throw new EntityAlreadyExistsException("User already exists");
        }
        // 1. Create local database user with their watchlist
        var user = new Account
        {
            Email = registerDto.Email,
            Watchlist = null!,
        };
        //add new user to our db so that the primary key id can be auto generated
        dbContext.Accounts.Add(user);
        await dbContext.SaveChangesAsync();
        //get this newely created user
        user = dbContext.Accounts.First(a => a.Email == email);
        var watchlist = new Watchlist
        {
            UserId = user.Id,
            Account = user,
        };
        user.Watchlist = watchlist;
        dbContext.Accounts.Update(user);
        dbContext.Watchlists.Add(watchlist);
        await dbContext.SaveChangesAsync();
        // Try to create a new user on Auth0
        try
        {
            // 2. Create Auth0 user with metadata linking to local user
            var userCreateRequest = new UserCreateRequest
            {
                Email = email,
                Password = registerDto.Password,
                Connection = "Username-Password-Authentication",
                VerifyEmail = false,
                AppMetadata = new Dictionary<string, object>
                {
                    { "lumiereUserId", user.Id } // Link to local database ID
                }
            };

            // Create user on Auth0
            var newUser = await _managementApiClient.Users.CreateAsync(userCreateRequest);
        }
        catch (Exception ex)
        {
            // Handle exceptions and log errors
            // Delete user from our db
            dbContext.Accounts.Remove(user);
            await dbContext.SaveChangesAsync();
            throw new RegisterFailedException(ex.Message);
        }
    }
}
