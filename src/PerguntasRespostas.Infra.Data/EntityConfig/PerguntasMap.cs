using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PerguntasRespostas.Domain.Entities;
using PerguntasRespostas.Infra.Data.Extensions;

namespace PerguntasRespostas.Infra.Data.EntityConfig
{
    public class PerguntasMap : EntityTypeConfiguration<Pergunta>
    {
        public override void Map(EntityTypeBuilder<Pergunta> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Autor)
                .HasColumnType("varchar(150)");

            builder.Property(c => c.Titulo)
                .HasColumnType("varchar(150)");

            builder.Property(c => c.Tags)
                .HasColumnType("varchar(300)");

            builder.Property(c => c.Descricao)
                .IsRequired()
                .HasColumnType("varchar(150)");

            builder.Ignore(e => e.Tags);
            builder.Ignore(e => e.Categoria);
            builder.Ignore(e => e.ValidationResult);
            builder.Ignore(e => e.CascadeMode);                        
            builder.Ignore(e => e.Respostas);

            builder.ToTable("Perguntas");
        }
    }
}