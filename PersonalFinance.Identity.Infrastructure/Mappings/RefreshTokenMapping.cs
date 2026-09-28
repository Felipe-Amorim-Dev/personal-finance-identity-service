using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PersonalFinance.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinance.Identity.Infrastructure.Mappings
{
    public class RefreshTokenMapping : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.TokenHash).IsRequired().HasMaxLength(64);

            builder.Property(x => x.ExpiresAt).IsRequired();

            builder.Property(x => x.CreatedAt).IsRequired();

            builder.Property(x => x.RevokedAt);

            builder.Ignore(x => x.IsActive);

            builder.HasIndex(x => x.TokenHash).IsUnique();

            builder.HasIndex(x => x.UserId);
        }
    }
}