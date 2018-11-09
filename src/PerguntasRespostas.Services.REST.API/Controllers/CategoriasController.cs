using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PerguntasRespostas.Domain.Entities;
using PerguntasRespostas.Domain.Interfaces.Services;
using PerguntasRespostas.Service.REST.API.ViewModels;

namespace PerguntasRespostas.Services.REST.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{apiVersion}/[controller]")]
    [EnableCors("Default")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IMapper _mapper;
        public CategoriasController(ICategoriaService categoriaService, IMapper mapper)
        {
            _categoriaService = categoriaService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<CategoriaViewModel>> Get()
        {
            var categorias = _mapper.Map<List<CategoriaViewModel>>(_categoriaService.ObterTodos().ToList());
            
            return categorias;
        }

        [HttpPost]
        public IActionResult Post(CategoriaViewModel categoria)
        {
            if (ModelState.IsValid)
            {
                _categoriaService.Adicionar(_mapper.Map<Categoria>(categoria));
                return Created($"/api/categoria{categoria.Id}", categoria);
            }
            return BadRequest(ModelState);
        }
    }
}