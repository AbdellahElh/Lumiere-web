using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Persistence.Triggers;
using Auth0Net.DependencyInjection;
using Microsoft.OpenApi.Models;
using Rise.Shared.Movies;
using Rise.Services.Movies;
using Rise.Shared.Events;
using Rise.Services.Events;
using Rise.Services.Watchlists;
using Rise.Shared.Accounts;
using Rise.Services.Accounts;
using Rise.Shared.Tenturncards;
using Rise.Services.Tenturncards;
using Rise.Shared.Tickets;
using Rise.Services.Tickets;
using Rise.Services.Auth;
using Rise.Server.Auth;
using Serilog;
using Rise.Server.Middleware;
using Auth0.ManagementApi;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
Log.Information("Starting web application");

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

// Basic services
builder.Services.AddSerilog();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger / OpenAPI setup
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri($"{builder.Configuration["Auth0:Authority"]}/oauth/token"),
                AuthorizationUrl = new Uri($"{builder.Configuration["Auth0:Authority"]}/authorize?audience={builder.Configuration["Auth0:Audience"]}"),
            }
        }
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new string[] { "openid" }
        }
    });
});

// 1) Read the UsePostgres flag from configuration
var usePostgres = builder.Configuration.GetValue<bool>("UsePostgres");

// 2) Conditionally choose which database to use
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (usePostgres)
    {
        // Use PostgreSQL (e.g., on Neon)
        // Make sure you have installed Npgsql.EntityFrameworkCore.PostgreSQL
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("PostgresDb")
        );
    }
    else
    {
        // Default to MariaDB (local dev)
        options.UseMySql(
            builder.Configuration.GetConnectionString("MariaDb"),
            new MariaDbServerVersion(new Version(11, 5))
        );
    }

    // Common EF Core options
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
    options.UseTriggers(triggers => triggers.AddTrigger<EntityBeforeSaveTrigger>());
});

// Register your services
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IWatchlistService, WatchListService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITenturncardService, TenturncardService>();
builder.Services.AddScoped<ITicketService, TicketService>();

builder.Services.AddHttpContextAccessor()
                .AddScoped<IAuthContextProvider, HttpContextAuthProvider>();

// Auth setup
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Authority"];
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.NameIdentifier
    };
});

// Auth0
builder.Services.AddAuth0AuthenticationClient(config =>
{
    config.Domain = builder.Configuration["Auth0:Authority"]!;
    config.ClientId = builder.Configuration["Auth0:M2MClientId"];
    config.ClientSecret = builder.Configuration["Auth0:M2MClientSecret"];
});
builder.Services.AddAuth0ManagementClient().AddManagementAccessToken();

// Build the app
var app = builder.Build();

// Development-only swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1.0");
        options.OAuthClientId(builder.Configuration["Auth0:BlazorClientId"]);
        options.OAuthClientSecret(builder.Configuration["Auth0:BlazorClientSecret"]);
    });
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

// Blazor + static files
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Health check endpoint for deployment platforms
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

app.MapControllers();
app.MapFallbackToFile("index.html");

// Seed the database in Development or Production if you want
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Temporarily disabled seeder to test database connectivity
    // Seeder seeder = new(dbContext);
    // seeder.Seed();
}

app.Run();

public partial class Program { }
