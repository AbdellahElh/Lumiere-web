using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Accounts;
using Rise.Domain.Movies;
using Rise.Domain.MovieWatchlists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Persistence.Accounts;
internal class AccountConfiguration : EntityConfiguration<Account>
{
    public override void Configure(EntityTypeBuilder<Account> builder)
    {
        base.Configure(builder);
        builder.HasKey(x => x.Id);
        //one to one relationship with watchlist
        builder.HasOne(e => e.Watchlist)
                    .WithOne(e => e.Account)
                    .HasForeignKey<Watchlist>(e => e.UserId)
                    .IsRequired()
                    .HasPrincipalKey<Account>(e => e.Id);
        //one to many relationship with tenturncards
        builder.HasMany(e => e.Tenturncards)
               .WithOne(e => e.Account)
               .HasForeignKey(e => e.AccountId);
        //one to many relationship with tickets
        builder.HasMany(e => e.Tickets)
               .WithOne(e => e.Account)
               .HasForeignKey(e => e.AccountId);
        //one to many relationship with giftcards
        builder.HasMany(e => e.Giftcards)
               .WithOne(e => e.Account)
               .HasForeignKey(e => e.AccountId);
    }
}
