using AutoMapper;
using PerguntasRespostas.Domain.Entities;
using PerguntasRespostas.Service.REST.API.ViewModels;

namespace PerguntasRespostas.Service.REST.API.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Pergunta, PerguntaViewModel>();            
            CreateMap<Categoria, CategoriaViewModel>();
            CreateMap<Respostas, RespostasViewModel>();
        }
    }
}