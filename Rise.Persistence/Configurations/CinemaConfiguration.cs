using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Movies;
using Rise.Domain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Persistence.Configurations;

internal class WatchlistConfiguration : EntityConfiguration<Cinema>
{
    public override void Configure(EntityTypeBuilder<Cinema> builder)
    {
        base.Configure(builder);
        builder.HasKey(x => x.Id);

        // Many-to-many relationship with Movie using Showtime as the join entity
        builder.HasMany(e => e.Movies)
               .WithMany(e => e.Cinemas)
               .UsingEntity<Showtime>(
                     j => j.HasOne(e => e.Movie)
                             .WithMany(e => e.Showtimes)
                             .HasForeignKey(e => e.MovieId),
                     j => j.HasOne(e => e.Cinema)
                             .WithMany(e => e.Showtimes)
                             .HasForeignKey(e => e.CinemaId)
                );

        // Many-to-many relationship with Event using Showtime as the join entity
        builder.HasMany(e => e.Events)
               .WithMany(e => e.Cinemas)
               .UsingEntity<Showtime>(
                     j => j.HasOne(e => e.Event)
                             .WithMany(e => e.Showtimes)
                             .HasForeignKey(e => e.EventId),
                     j => j.HasOne(e => e.Cinema)
                             .WithMany(e => e.Showtimes)
                             .HasForeignKey(e => e.CinemaId)
                );
    }
}