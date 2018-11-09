using System;
using System.Collections.Generic;
using PerguntasRespostas.Domain.Entities;
using PerguntasRespostas.Domain.Interfaces.Repositories;
using PerguntasRespostas.Domain.Interfaces.Services;

namespace PerguntasRespostas.Domain.Services
{
    public class PerguntaService : ServiceBase<Pergunta>, IPerguntaService
    {
        private readonly IPerguntaRepository _perguntaRepository;

        public PerguntaService(IPerguntaRepository perguntaRepository)
            : base(perguntaRepository)
        {
            _perguntaRepository = perguntaRepository;
        }

        public IEnumerable<Pergunta> ObterMinhasPerguntas(String autor)
        {
            return _perguntaRepository.ObterMinhasPerguntas(autor);
        }
    }
}
