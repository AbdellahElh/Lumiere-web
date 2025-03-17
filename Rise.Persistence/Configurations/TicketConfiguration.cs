using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NodaTime;
using Rise.Domain.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Rise.Persistence.Tickets;

internal class TicketConfiguration : EntityConfiguration<Ticket>
{
    public override void Configure(EntityTypeBuilder<Ticket> builder)
    {
        base.Configure(builder);
        builder.HasKey(x => x.Id);
        //many to one relationship with Account
        builder.HasOne(e => e.Account)
               .WithMany(e => e.Tickets)
               .HasForeignKey(e => e.AccountId);
        //many to one relationship with Movie
       builder.HasOne(e => e.Movie)
                   .WithMany(e => e.Tickets)
                   .HasForeignKey(e => e.MovieId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Many-to-One relationship with Event 
            builder.HasOne(e => e.Event)
                   .WithMany(e => e.Tickets)
                   .HasForeignKey(e => e.EventId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Ensure EventId is optional
            builder.Property(e => e.EventId).IsRequired(false);
            builder.Property(e => e.MovieId).IsRequired(false);     
    }
}
