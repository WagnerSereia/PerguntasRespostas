
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PerguntasRespostas.Infra.Data.Extensions
{
    public abstract class EntityTypeConfiguration<TEntity> where TEntity : class
    {
        public abstract void Map(EntityTypeBuilder<TEntity> builder);
    }
}
