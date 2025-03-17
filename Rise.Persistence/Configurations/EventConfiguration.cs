using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Events;
using Rise.Domain.Movies;
using System;

namespace Rise.Persistence.Events;

internal class EventConfiguration : EntityConfiguration<Event>
{
    public override void Configure(EntityTypeBuilder<Event> builder)
    {
        base.Configure(builder);
        builder.HasKey(x => x.Id);

        // many to many relationship with cinema with navigations to join entity 
        // with the help of Showtimes conjunction table entity
        builder.HasMany(e => e.Cinemas)
               .WithMany(e => e.Events)
               .UsingEntity<Showtime>(
                     j => j.HasOne(e => e.Cinema)
                             .WithMany(e => e.Showtimes)
                             .HasForeignKey(e => e.CinemaId),
                     j => j.HasOne(e => e.Event)
                             .WithMany(e => e.Showtimes)
                             .HasForeignKey(e => e.EventId)
                );
    }
}