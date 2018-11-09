using System;

namespace PerguntasRespostas.Service.REST.API.ViewModels
{
    public class CategoriaViewModel
    {       
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
    }
}