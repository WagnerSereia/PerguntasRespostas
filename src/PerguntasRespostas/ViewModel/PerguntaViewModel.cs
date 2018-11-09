using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerguntasRespostas.ViewModel
{
    public class PerguntaViewModel
    {
        public PerguntaViewModel()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Autor { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public Guid? CategoriaId { get; set; }
        public virtual CategoriaViewModel Categoria { get; set; }
        public virtual ICollection<string> Tags { get; set; }
        public virtual ICollection<RespostasViewModel> Respostas { get; set; }
        public DateTime DataCadastro { get; set; }       
    }
}
