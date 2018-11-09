using System;
using FluentValidation;
using FluentValidation.Results;

namespace PerguntasRespostas.Domain.Entities.Model
{
    public abstract class Entity<T>: AbstractValidator<T> where T : Entity<T>
    {
        protected Entity()
        {
            ValidationResult = new ValidationResult();
            Id = Guid.NewGuid();
        }

        public Guid Id { get; protected set; }

        public abstract bool EhValido();
        public ValidationResult ValidationResult { get; protected set; }
        public override string ToString()
        {
            return GetType().Name + "[Id = " + Id + "]";
        }
    }
}
