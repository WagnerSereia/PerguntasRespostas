using PerguntasRespostas.Domain.Entities;
using PerguntasRespostas.Domain.Interfaces.Repositories;
using PerguntasRespostas.Domain.Interfaces.Services;

namespace PerguntasRespostas.Domain.Services
{
    public class CategoriaService : ServiceBase<Categoria>, ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
            : base(categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }
    }
}
