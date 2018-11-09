using System;
using System.Collections.Generic;
using System.Text;

namespace PerguntasRespostas.Domain.Interfaces.Services
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        void Adicionar(TEntity obj);
        TEntity ObterPorId(Guid id);
        IEnumerable<TEntity> ObterTodos();
        TEntity Atualizar(TEntity obj);
        TEntity Remover(Guid id);        
        void Dispose();
    }
}
