using FluentValidation;
using PerguntasRespostas.Domain.Entities.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PerguntasRespostas.Domain.Entities
{
    public class User : Entity<User>
    {
        public User(string nome, string login, string senha)
        {
            Nome = nome;
            Senha = senha;
            Login = login;
        }
        public User()
        {
        }
        public string Nome { get; private set; }
        public string Login { get; private set; }
        public string Senha { get; private set; }

        public override bool EhValido()
        {
            RuleFor(c => c.Nome)
               .NotEmpty().WithMessage("O nome precisa ser fornecido")
               .Length(2, 150).WithMessage("O nome precisa ter entre 2 e 150 caracteres");

            RuleFor(c => c.Login)
               .NotEmpty().WithMessage("O login precisa ser fornecido")
               .Length(2, 20).WithMessage("O login precisa ter entre 2 e 20 caracteres");

            RuleFor(c => c.Senha)
               .NotEmpty().WithMessage("A senha precisa ser fornecida")
               .Length(4, 8).WithMessage("A senha precisa ter entre 4 e 8 caracteres");
            

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }

        internal static User getFixUser()
        {
            return new User
            {
                Id = Guid.Parse("C32F1A6F-2761-4624-B422-DCBE36D43F46"),
                Nome = "Wagner Sereia Santos",
                Senha = "abc123@",
                Login = "wagner"
            };
        }
    }
}
