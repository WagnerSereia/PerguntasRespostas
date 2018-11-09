using System;
using AutoMapper;
using PerguntasRespostas.Domain.Entities;
using PerguntasRespostas.Service.REST.API.ViewModels;

namespace PerguntasRespostas.Service.REST.API.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {            
            CreateMap<PerguntaViewModel, Pergunta>()
                .ConstructUsing(c => new Pergunta(c.Autor,c.Titulo,c.Descricao));

            CreateMap<CategoriaViewModel, Categoria>()
                .ConstructUsing(c => new Categoria(c.Titulo,c.Descricao));

            CreateMap<RespostasViewModel, Respostas>()
                .ConstructUsing(c => new Respostas(c.Autor,c.Descricao));
        }
    }
}