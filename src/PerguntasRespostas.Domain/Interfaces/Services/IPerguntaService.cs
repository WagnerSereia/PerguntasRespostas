using PerguntasRespostas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PerguntasRespostas.Domain.Interfaces.Services
{
    public interface IPerguntaService:IServiceBase<Pergunta>
    {
        IEnumerable<Pergunta> ObterMinhasPerguntas(String autor);
    }
}
