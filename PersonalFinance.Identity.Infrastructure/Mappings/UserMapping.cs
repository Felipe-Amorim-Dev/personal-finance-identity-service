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
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Nome).IsRequired().HasMaxLength(100);

            builder.Property(x => x.Sobrenome).IsRequired().HasMaxLength(100);

            builder.Property(x => x.DataNascimento).IsRequired();

            builder.Property(x => x.Email).IsRequired().HasMaxLength(255);

            builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(255);

            builder.Property(x => x.IsActive).IsRequired();

            builder.Property(x => x.CreatedAt).IsRequired();

            builder.Property(x => x.UpdatedAt);

            builder.HasIndex(x => x.Email).IsUnique();

            builder.HasOne(x => x.Endereco).WithOne().HasForeignKey<Endereco>("UserId").IsRequired().OnDelete(DeleteBehavior.Cascade);
        }
    }
}