using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PerguntasRespostas.Domain.Entities;
using PerguntasRespostas.Domain.Interfaces.Services;
using PerguntasRespostas.Service.REST.API.ViewModels;

namespace respostasRespostas.Services.REST.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{apiVersion}/[controller]")]
    [EnableCors("Default")]
    [ApiController]
    //[Authorize]
    public class RespostasController : Controller
    {
        private readonly IRespostaService _respostaService;
        private readonly IMapper _mapper;
        private readonly IUserService _user;
        public RespostasController(IRespostaService respostaservice, IMapper mapper, IUserService user)
        {
            _respostaService = respostaservice;
            _mapper = mapper;
            _user = user;
        }


        [HttpGet]
        [Route("todas-respostas")]
        [AllowAnonymous]
        public ActionResult<List<RespostasViewModel>> Get()
        {
            var respostas = _mapper.Map<List<RespostasViewModel>>(_respostaService.ObterTodos().ToList());
            //if (perguntas.Count() == 0)
            //    return BadRequest();

            return respostas;
        }

        [HttpGet]
        [Route("obter-resposta/{id:guid}")]
        public ActionResult<RespostasViewModel> Get(Guid id)
        {
            return _mapper.Map<RespostasViewModel>(_respostaService.ObterPorId(id));
        }

        [HttpPost]
        [Route("criar-resposta")]
        public IActionResult Post(RespostasViewModel resposta)
        {
            if (ModelState.IsValid)
            {
                _respostaService.Adicionar(_mapper.Map<Respostas>(resposta));
                return Created($"/api/resposta{resposta.Id}", resposta);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        [Route("atualizar-resposta/{id}")]
        public IActionResult Put(int id, [FromBody]RespostasViewModel resposta)
        {
            if (ModelState.IsValid)
            {
                _respostaService.Atualizar(_mapper.Map<Respostas>(resposta));
                return Created($"/api/resposta{resposta.Id}", resposta);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        [Route("remover-resposta/{id}")]
        public IActionResult Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                _respostaService.Remover(id);
                return RedirectToAction("todas-respostas");
            }
            return BadRequest(ModelState);
        }
    }
}