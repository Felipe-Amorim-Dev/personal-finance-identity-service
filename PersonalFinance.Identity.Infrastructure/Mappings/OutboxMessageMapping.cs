using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PersonalFinance.Identity.Infrastructure.Outbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Infrastructure.Mappings
{
    public class OutboxMessageMapping : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Type).IsRequired().HasMaxLength(500);

            builder.Property(x => x.Content).IsRequired().HasColumnType("longtext");

            builder.Property(x => x.OccurredAt).IsRequired();

            builder.Property(x => x.ProcessedAt);

            builder.Property(x => x.Error).HasColumnType("longtext");

            builder.HasIndex(x => x.ProcessedAt);

            builder.Property(x => x.RetryCount).IsRequired();

            builder.Property(x => x.NextRetryAt);            

            builder.Property(x => x.FailedAt);

            builder.HasIndex(x => new { x.ProcessedAt, x.FailedAt, x.NextRetryAt });
        }
    }
}
