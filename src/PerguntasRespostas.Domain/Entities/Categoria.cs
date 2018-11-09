using FluentValidation;
using PerguntasRespostas.Domain.Entities.Model;
using System;

namespace PerguntasRespostas.Domain.Entities
{
    public class Categoria: Entity<Categoria>
    {
        public Categoria(string titulo, string descricao)
        {
            Titulo = titulo;
            Descricao = descricao;            
        }
        public Categoria()
        {
        }
        public string Titulo { get; private set; }
        public string Descricao { get; private set; }

        public override bool EhValido()
        {
            RuleFor(c => c.Titulo)
               .NotEmpty().WithMessage("O titulo precisa ser fornecido")
               .Length(2, 150).WithMessage("O titulo precisa ter entre 2 e 150 caracteres");


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