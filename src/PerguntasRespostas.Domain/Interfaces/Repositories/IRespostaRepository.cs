using PerguntasRespostas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PerguntasRespostas.Domain.Interfaces.Repositories
{
    public interface IRespostaRepository : IRepositoryBase<Respostas>
    {
        IEnumerable<Respostas> ObterMinhasRespostas(String autor);    
    }
}
