using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

using Xunit;
using Microsoft.AspNetCore.Routing;
using Rise.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Rise.Shared.Accounts;
using Rise.Domain.Exceptions;
using Rise.Domain.Accounts;
using Respawn;
using MySqlConnector;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Rise.Domain.Movies;
using Rise.Services.Auth;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Rise.Server.Tests
{
    public abstract class IntegrationTest : IClassFixture<CustomWebApplicationFactory>, IAsyncLifetime
    {
        protected readonly CustomWebApplicationFactory _factory;
        protected readonly HttpClient _client;
        protected readonly ApplicationDbContext _dbContext;
        private readonly AuthenticationApiClient _authenticationApiClient;
        private readonly IManagementApiClient _managementApiClient;
        private readonly List<string> _createdUserIds = [];
        protected string connectionString;
        protected Respawner respawner;


        public IntegrationTest(CustomWebApplicationFactory fixture)
        {
            _factory = fixture;
            _client = fixture.CreateClient();
           
            _dbContext = fixture.Services.GetRequiredService<ApplicationDbContext>();
            _managementApiClient = fixture.Services.GetRequiredService<IManagementApiClient>();

            var config = fixture.Configuration;

            var auth0Domain = config["Auth0:Authority"];
            if (string.IsNullOrEmpty(auth0Domain))
            {
                throw new ArgumentException("Auth0 domain is not configured.");
            }

            _authenticationApiClient = new AuthenticationApiClient(new Uri(auth0Domain));

            connectionString = config.GetConnectionString("MariaTestDb");
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }


        public async Task DisposeAsync()
        {
            //Delete all created users from auth0
            foreach (var userId in _createdUserIds)
            {
                try
                {
                    await _managementApiClient.Users.DeleteAsync(userId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete user with ID {userId}: {ex.Message}");
                }
            }
            _createdUserIds.Clear();
        }

        protected async Task LoginAsync(RegisterDto dto)
        {
            var clientId = _factory.Configuration["Auth0:BlazorClientId"];
            var clientSecret = _factory.Configuration["Auth0:BlazorClientSecret"];
            var audience = _factory.Configuration["Auth0:Audience"];

            var tokenRequest = new ResourceOwnerTokenRequest
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = "openid profile",
                Audience = audience,
                Username = dto.Email,
                Password = dto.Password,
            };

            var retries = 0;
            var retryLimit = 50;
            var success = false;
            while (retries <= retryLimit && !success)
            {
                success = await SendLoginRequest(tokenRequest);
                retries++;
            }
        }

        private async Task<bool> SendLoginRequest(ResourceOwnerTokenRequest tokenRequest)
        {
            try
            {
                var tokenResponse = await _authenticationApiClient.GetTokenAsync(tokenRequest); 
                var token = tokenResponse.AccessToken;
                if (_client == null)
                {
                    throw new InvalidOperationException("HttpClient is not initialized.");
                }
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return true;
            }
            catch (RateLimitApiException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine($"Rate limit exceeded. Retrying after 1 seconds...");
                //Delay so that auth0 api doesn't throw a rate limit exception
                await Task.Delay(TimeSpan.FromSeconds(1));
                return false;
            }
        }

        protected async Task RegisterAsync(RegisterDto registerDto)
        {
            var retries = 0;
            var retryLimit = 50;
            var success = false;
            while (retries <= retryLimit && !success)
            {
                success = await SendCreateUserRequest(registerDto);
                retries++;
            }
        }

        private async Task<bool> SendCreateUserRequest(RegisterDto registerDto)
        {
            //check if user already exists in our db
            var email = registerDto.Email;
            if (_dbContext.Accounts.Any(a => a.Email == email))
            {
                throw new EntityAlreadyExistsException("User already exists");
            }
            //Create a new user and create for them a watchlist
            var user = new Account
            {
                Email = registerDto.Email,
                Watchlist = null,
            };
            //add new user to our db so that the primary key id can be auto generated
            _dbContext.Accounts.Add(user);
            await _dbContext.SaveChangesAsync();
            //get this newely created user
            user = _dbContext.Accounts.First(a => a.Email == email);
            var watchlist = new Watchlist
            {
                UserId = user.Id,
                Account = user,
            };
            user.Watchlist = watchlist;
            _dbContext.Accounts.Update(user);
            _dbContext.Watchlists.Add(watchlist);
            await _dbContext.SaveChangesAsync();
            // Try to create a new user on Auth0
            try
            {
                // Create user request
                var userCreateRequest = new UserCreateRequest
                {
                    Email = email,
                    Password = registerDto.Password,
                    Connection = "Username-Password-Authentication",
                    VerifyEmail = false,
                    AppMetadata = new Dictionary<string, object>
                {
                    { "lumiereUserId", user.Id }
                }
                };

                // Create user on Auth0
                var newUser = await _managementApiClient.Users.CreateAsync(userCreateRequest);
                // Save the userid
                _createdUserIds.Add(newUser.UserId);
                return true;
            }
            catch (RateLimitApiException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine($"Rate limit exceeded. Retrying after 1 seconds...");
                //Delay so that auth0 api doesn't throw a rate limit exception
                await Task.Delay(TimeSpan.FromSeconds(1));
                // Delete user from our db
                _dbContext.Accounts.Remove(user);
                await _dbContext.SaveChangesAsync();
                return false;
            }
        }

        protected void Logout()
        {
            _client.DefaultRequestHeaders.Authorization = null;
        }


    }

}
