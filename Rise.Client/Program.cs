using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Web;
using Rise.Client;
using Rise.Client.Auth;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

using Rise.Client.Movies.services;
using Rise.Shared.Movies;
using Rise.Shared.Events;
using Rise.Shared.Tickets;
using Rise.Client.Account.services;
using Rise.Shared.Tenturncards;
using Rise.Client.Tenturncards;
using Rise.Client.Infrastructure;
using Rise.Client.events.services;
using Rise.Shared.Accounts;
using Rise.Client.Tickets.services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Uncomment and configure the following lines if needed
//builder.RootComponents.Add<HeadOutlet>("head::after");

builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.PostLogoutRedirectUri = builder.HostEnvironment.BaseAddress;
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]!);

}).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

builder.Services.AddTransient<CustomAuthorizationMessageHandler>();
builder.Services.AddTransient<CleanErrorHandler>();



builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
       .CreateClient("Gent5API"));


// Register the services
builder.Services.AddScoped<IWatchlistService, WatchListService>(); 
builder.Services.AddScoped<ITenturncardService, TenturncardService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IEventService, EventService>();

// Get API base URL from configuration (pointing to Render backend)
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? $"{builder.HostEnvironment.BaseAddress}api/";

builder.Services.AddHttpClient("Gent5API", client => client.BaseAddress = new Uri(apiBaseUrl))
       .AddHttpMessageHandler<CustomAuthorizationMessageHandler>()
       .AddHttpMessageHandler<CleanErrorHandler>();


builder.Services.AddHttpClient<IMovieService, Rise.Client.Movies.services.MovieService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<CleanErrorHandler>();

builder.Services.AddHttpClient<IEventService, EventService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<CleanErrorHandler>();

builder.Services.AddHttpClient<IAccountService, AccountService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<CleanErrorHandler>();

builder.Services.AddHttpClient<ITenturncardService, TenturncardService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<CustomAuthorizationMessageHandler>()
  .AddHttpMessageHandler<CleanErrorHandler>();

  builder.Services.AddHttpClient<IWatchlistService, WatchListService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<CustomAuthorizationMessageHandler>()
  .AddHttpMessageHandler<CleanErrorHandler>();

builder.Services.AddHttpClient<ITicketService, TicketService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<CustomAuthorizationMessageHandler>()
  .AddHttpMessageHandler<CleanErrorHandler>();


await builder.Build().RunAsync();