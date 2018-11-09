using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerguntasRespostas.Domain.Entities;
using PerguntasRespostas.Infra.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PerguntasRespostas.Infra.Data.EntityConfig
{
    public class CategoriasMap: EntityTypeConfiguration<Categoria>
    {
        public override void Map(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(c => c.Id);
                      
            builder.Property(c => c.Titulo)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(c => c.Descricao)
                .HasColumnType("varchar(150)");

            builder.Ignore(e => e.ValidationResult);
            builder.Ignore(e => e.CascadeMode);


            builder.ToTable("Categorias");
        }

    }
}
