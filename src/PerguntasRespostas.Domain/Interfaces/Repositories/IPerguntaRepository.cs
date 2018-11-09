using PerguntasRespostas.Domain.Entities;
using System;
using System.Collections.Generic;

namespace PerguntasRespostas.Domain.Interfaces.Repositories
{
    public interface IPerguntaRepository:IRepositoryBase<Pergunta>
    {
        IEnumerable<Pergunta> ObterMinhasPerguntas(String autor);
    }
}
