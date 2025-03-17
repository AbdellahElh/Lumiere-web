using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Movies;
using Rise.Domain.MovieWatchlists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Persistence.Movies;

internal class MovieConfiguration : EntityConfiguration<Movie>
{
    public override void Configure(EntityTypeBuilder<Movie> builder)
    {
        base.Configure(builder);
        builder.HasKey(x => x.Id);
        //many to many relationship with watchlist with navigations to join entity 
        //with the help of MovieWatchlist conjuction table entity
        builder.HasMany(e => e.Cinemas)
               .WithMany(e => e.Movies)
               .UsingEntity<Showtime>(
                    j => j.HasOne(e => e.Cinema)
                          .WithMany(e => e.Showtimes)
                          .HasForeignKey(e => e.CinemaId),
                    j => j.HasOne(e => e.Movie)
                          .WithMany(e => e.Showtimes)
                          .HasForeignKey(e => e.MovieId)
                );
        //many to many relationship with cinema with navigations to join entity 
        //with the help of Showtimes conjuction table entity
        builder.HasMany<MovieWatchlist>()
               .WithOne(mw => mw.Movie)
               .HasForeignKey(mw => mw.MovieId);
    }
}
