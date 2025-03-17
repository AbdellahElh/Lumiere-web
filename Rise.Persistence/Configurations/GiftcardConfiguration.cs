using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Giftcards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Persistence.Giftcards;

internal class GiftcardConfiguration : EntityConfiguration<Giftcard>
{
    public override void Configure(EntityTypeBuilder<Giftcard> builder)
    {
        base.Configure(builder);
        builder.HasKey(x => x.Id);
        //many to one relationship with Account
        builder.HasOne(e => e.Account)
               .WithMany(e => e.Giftcards)
               .HasForeignKey(e => e.AccountId);
    }
}