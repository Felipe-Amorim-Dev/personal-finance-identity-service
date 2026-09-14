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
    public class EnderecoMapping : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.ToTable("Enderecos");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Logradouro).IsRequired().HasMaxLength(200);

            builder.Property(x => x.Numero).IsRequired().HasMaxLength(20);

            builder.Property(x => x.Complemento).HasMaxLength(150);

            builder.Property(x => x.Bairro).IsRequired().HasMaxLength(100);

            builder.Property(x => x.Cidade).IsRequired().HasMaxLength(100);

            builder.Property(x => x.Estado).IsRequired().HasMaxLength(100);

            builder.Property(x => x.Cep).IsRequired().HasMaxLength(20);

            builder.Property(x => x.Pais).IsRequired().HasMaxLength(100);

            builder.Property(x => x.CreatedAt).IsRequired();

            builder.Property(x => x.UpdatedAt);
        }
    }
}
