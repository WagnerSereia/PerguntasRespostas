using FluentValidation;
using PerguntasRespostas.Domain.Entities.Model;
using System;
using System.Collections.Generic;

namespace PerguntasRespostas.Domain.Entities
{
    public class Pergunta : Entity<Pergunta>
    {
        public Pergunta(string autor,string titulo,string descricao)
        {
            Autor = autor;
            Titulo = titulo;
            Descricao = descricao;

            Tags = new List<string>();
            Respostas = new List<Respostas>();
            
        }
        public Pergunta()
        {
            Tags = new List<string>();
            Respostas = new List<Respostas>();
        }
        public string Autor { get; private set; }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }
        public Guid CategoriaId { get; private set; }
        public virtual Categoria Categoria { get; set; }
        public virtual ICollection<string> Tags { get; private set; }
        public virtual ICollection<Respostas> Respostas { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public override bool EhValido()
        {
            RuleFor(c => c.Descricao)
               .NotEmpty().WithMessage("A Descrição precisa ser fornecida")
               .Length(2, 150).WithMessage("A Descrição precisa ter entre 2 e 150 caracteres");
            
            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }

        public override string ToString()
        {
            return $"[{Id}]-{Titulo}";
        }
    }
}
