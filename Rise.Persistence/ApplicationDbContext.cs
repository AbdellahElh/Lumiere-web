using Microsoft.EntityFrameworkCore;
using Rise.Domain.Products;
using Rise.Domain.Movies;
using Rise.Domain.Accounts;
using Rise.Domain.MovieWatchlists;
using Rise.Domain.Tickets;
using Rise.Domain.Giftcards;
using Rise.Domain.Tenturncards;
using Rise.Domain.Events;

namespace Rise.Persistence;

/// <inheritdoc />
public class ApplicationDbContext : DbContext
{
 //   public DbSet<Product> Products => Set<Product>();

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Watchlist> Watchlists => Set<Watchlist>();
    public DbSet<MovieWatchlist> MovieWatchlist => Set<MovieWatchlist>();
    public DbSet<Cinema> Cinemas => Set<Cinema>();

    public DbSet<Showtime> Showtime => Set<Showtime>();

    public DbSet<Ticket> Tickets => Set<Ticket>();

    public DbSet<Giftcard> Giftcards => Set<Giftcard>();

    public DbSet<Tenturncard> Tenturncards => Set<Tenturncard>();

    public DbSet<Event> Events => Set<Event>();




    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // All columns in the database have a maxlength of 255.
        // in NVARACHAR 255 is the maximum length that can be indexed by a database.
        // Some columns need more length, but these can be set on the configuration level for that Entity in particular.
        configurationBuilder.Properties<string>().HaveMaxLength(255);
        // All decimals columns should have 2 digits after the comma
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        // configurationBuilder.EnableSensitiveDataLogging(); //not recommended

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Applying all types of IEntityTypeConfiguration in the Persistence project.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

}

