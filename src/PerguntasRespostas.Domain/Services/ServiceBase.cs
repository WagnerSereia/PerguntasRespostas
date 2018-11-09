using PerguntasRespostas.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PerguntasRespostas.Domain.Interfaces.Services
{
    public class ServiceBase<TEntity> : IDisposable, IServiceBase<TEntity> where TEntity : class
    {
        private readonly IRepositoryBase<TEntity> _repository;


        public ServiceBase(IRepositoryBase<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual void Adicionar(TEntity obj)
        {
            _repository.Adicionar(obj);
        }

        public TEntity ObterPorId(Guid id)
        {
            return _repository.ObterPorId(id);
        }

        public IEnumerable<TEntity> ObterTodos()
        {
            return _repository.ObterTodos();
        }

        public virtual TEntity Atualizar(TEntity obj)
        {
            return _repository.Atualizar(obj);
        }

        public virtual TEntity Remover(Guid id)
        {
            _repository.Remover(id);
            return null;
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
