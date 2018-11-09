
using System;

namespace PerguntasRespostas.Service.REST.API.ViewModels
{
    public class RespostasViewModel
    {
        public RespostasViewModel()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Autor { get; set; }
        public string Descricao { get; set; }    
        public Guid PerguntaId { get; set; }
        public PerguntaViewModel Pergunta { get; set; }
    }
}