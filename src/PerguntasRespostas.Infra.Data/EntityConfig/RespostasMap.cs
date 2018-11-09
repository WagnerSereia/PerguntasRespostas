using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerguntasRespostas.Domain.Entities;
using PerguntasRespostas.Infra.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PerguntasRespostas.Infra.Data.EntityConfig
{
    public class RespostasMap : EntityTypeConfiguration<Respostas>
    {
        public override void Map(EntityTypeBuilder<Respostas> builder)
        {
            builder.HasKey(c => c.Id);
            
            builder.Property(c => c.Autor)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Property(c => c.Descricao)
                .IsRequired()
                .HasColumnType("varchar(155)");

            builder.Ignore(c => c.Pergunta);
            builder.Ignore(e => e.ValidationResult);
            builder.Ignore(e => e.CascadeMode);
                                   
            builder.HasOne(e => e.Pergunta)
               .WithMany(e => e.Respostas)
               .HasForeignKey(e => e.PerguntaId);

            builder.ToTable("Respostas");
        }

    }
}
