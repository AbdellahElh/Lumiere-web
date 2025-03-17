using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using Respawn;
using Rise.Persistence;
using Rise.Persistence.Triggers;
using Rise.Server.Auth;
using Rise.Server.Tests.Auth;
using Rise.Services.Auth;
using Rise.Services.Tenturncards;
using Rise.Shared.Tenturncards;
using System;

namespace Rise.Server.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program> 
{
    public IConfiguration Configuration { get; private set; } = default!;
    private string connectionString = string.Empty;
    public Respawner respawner = default!;


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            Configuration = new ConfigurationBuilder()  
                .AddUserSecrets("f33d7267-bb0e-4210-a689-c7bb49ad770b")
                .Build();
            config.AddConfiguration(Configuration);
        });


        builder.UseEnvironment("Testing");


        builder.ConfigureServices((context, services) =>
        {
            // Remove the existing ApplicationDbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            // Add the testdb for testing
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                connectionString = Configuration.GetConnectionString("MariaTestDb")
                                   ?? throw new InvalidOperationException("Connection string 'MariaTestDb' not found."); 
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                    );
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
                options.UseTriggers(options => options.AddTrigger<EntityBeforeSaveTrigger>());
            });

            // Add the test auth context provider
            services.AddHttpContextAccessor().AddScoped<IAuthContextProvider, TestAuthContextProvider>();


            var serviceProvider = services.BuildServiceProvider();
            TestDatabaseInitializer.Init(serviceProvider);
        });

    }
    public async Task InitializeAsync()
    {
        MySqlConnection conn = new MySqlConnection(connectionString);
        conn.Open();

        respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
        {
            DbAdapter = DbAdapter.MySql
        });

        await respawner.ResetAsync(conn);
    }


    protected override void Dispose(bool disposing)
    {
        // No need to dispose of respawner as it does not implement IDisposable
        //The call to base.Dispose(disposing) is retained to ensure that any resources managed by the base class are properly disposed of.
        base.Dispose(disposing);
    }

    /// <summary>
    /// Singleton service to initialize the database once
    /// </summary>
    internal class TestDatabaseInitializer
    {
        private static readonly object _lock = new();
        private static bool _databaseInitialized = false;

        public static void Init(IServiceProvider serviceProvider)
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<ApplicationDbContext>();

                        db.Database.EnsureDeleted();
                        db.Database.Migrate();
                       // db.Database.EnsureCreated();

                        // Ensure seeding is consistent 
                        new Seeder(db).Seed();
                    }

                    _databaseInitialized = true;
                }
            }
        }

    }

}
