using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PerguntasRespostas.Domain.Entities;
using PerguntasRespostas.Domain.Interfaces.Repositories;
using PerguntasRespostas.Domain.Interfaces.Services;
using PerguntasRespostas.Service.REST.API.ViewModels;

namespace PerguntasRespostas.Services.REST.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{apiVersion}/[controller]")]
    [EnableCors("Default")]
    [ApiController]    
    public class PerguntasController : ControllerBase
    {
        private readonly IPerguntaService _perguntaService;
        private readonly IMapper _mapper;
        private readonly IUserService _user;
        public PerguntasController(IPerguntaService perguntaService, IMapper mapper, IUserService user)
        {
            _perguntaService = perguntaService;
            _mapper = mapper;
            _user = user;
        }

        [HttpGet]
        [Route("todas-perguntas")]
        [AllowAnonymous]
        public ActionResult<List<PerguntaViewModel>> Get()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity.Name;
            }

            var perguntas = _mapper.Map<List<PerguntaViewModel>>(_perguntaService.ObterTodos().ToList());
            //if (perguntas.Count() == 0)
            //    return BadRequest();

            return perguntas;
        }

        [Authorize("Bearer")]
        [HttpGet]
        [Route("minhas-perguntas")]
        public ActionResult<List<PerguntaViewModel>> MinhasPerguntas()
        {
            var a = User.Identity.IsAuthenticated;

            if (User.Identity.IsAuthenticated)
            {
                var perguntas = _mapper.Map<List<PerguntaViewModel>>(_perguntaService.ObterMinhasPerguntas(User.Identity.Name).ToList());
                //var perguntas = _mapper.Map<List<PerguntaViewModel>>(_perguntaService.ObterMinhasPerguntas("wagner").ToList());

                return perguntas;
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("minhas-perguntas-respondidas")]
        [Authorize("Bearer")]
        public ActionResult<List<PerguntaViewModel>> MinhasPerguntasRespondidas()
        {
            var a = User.Identity.IsAuthenticated;

            if (User.Identity.IsAuthenticated)
            {
                var perguntas = _mapper.Map<List<PerguntaViewModel>>(_perguntaService.ObterMinhasPerguntas(User.Identity.Name).ToList());

                return perguntas;
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("obter-pergunta/{id:guid}")]
        public ActionResult<PerguntaViewModel> Get(Guid id)
        {
            return _mapper.Map<PerguntaViewModel>(_perguntaService.ObterPorId(id));
        }

        [HttpPost]
        [Route("criar-pergunta")]
        public IActionResult Post(PerguntaViewModel pergunta)
        {
            if (ModelState.IsValid)
            {
                _perguntaService.Adicionar(_mapper.Map<Pergunta>(pergunta));
                return Created($"/api/pergunta{pergunta.Id}", pergunta);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        [Route("atualizar-pergunta/{id}")]
        [Authorize("Bearer")]
        public IActionResult Put([FromRoute]Guid id, [FromBody]PerguntaViewModel pergunta)
        {
            if (ModelState.IsValid)
            {
                _perguntaService.Atualizar(_mapper.Map<Pergunta>(pergunta));
                return Created($"/api/pergunta{pergunta.Id}", pergunta);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        [Route("remover-pergunta/{id}")]
        [Authorize("Bearer")]
        public IActionResult Delete([FromRoute]Guid id)
        {
            if (ModelState.IsValid)
            {
                _perguntaService.Remover(id);
                return Ok();
            }
            return BadRequest(ModelState);
        }        
    }
}

