using PerguntasRespostas.Domain.Entities;
using PerguntasRespostas.Domain.Interfaces.Repositories;
using PerguntasRespostas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PerguntasRespostas.Domain.Services
{
    public class RespostaService : ServiceBase<Respostas>, IRespostaService
    {
        private readonly IRespostaRepository _respostaRepository;

        public RespostaService(IRespostaRepository respostaRepository)
            : base(respostaRepository)
        {
            _respostaRepository = respostaRepository;
        }

        public IEnumerable<Respostas> ObterMinhasRespostas(String autor)
        {
            return _respostaRepository.ObterMinhasRespostas(autor);
        }
    }
}
