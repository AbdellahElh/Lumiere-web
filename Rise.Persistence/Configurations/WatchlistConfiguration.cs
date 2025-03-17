using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Movies;
using Rise.Domain.MovieWatchlists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Persistence.Watchlists;

internal class WatchlistConfiguration : EntityConfiguration<Watchlist>
{
    public override void Configure(EntityTypeBuilder<Watchlist> builder)
    {
        base.Configure(builder);
        builder.HasKey(x => x.Id);
        //many to many relationship with movie
        //with the help of MovieWatchlist conjuction table entity
        builder.HasMany(e => e.Movies)
               .WithMany()
               .UsingEntity<MovieWatchlist>(
                    j => j.HasOne(e => e.Movie)
                          .WithMany(e => e.MovieWatchlists)
                          .HasForeignKey(e => e.MovieId),
                    j => j.HasOne(e => e.Watchlist)
                          .WithMany(e => e.MovieWatchlists)
                          .HasForeignKey(e => e.WatchlistId)
                );
    }
}
