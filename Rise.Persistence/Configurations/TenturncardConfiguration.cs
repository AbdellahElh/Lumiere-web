using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Tenturncards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Persistence.Tenturncards;

internal class TenturncardConfiguration : EntityConfiguration<Tenturncard>
{
    public override void Configure(EntityTypeBuilder<Tenturncard> builder)
    {
        base.Configure(builder);
        builder.HasKey(x => x.Id);
        //many to one relationship with Account
        builder.HasOne(e => e.Account)
               .WithMany(e => e.Tenturncards)
               .HasForeignKey(e => e.AccountId);
    }
}
