using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PerguntasRespostas.UI.Mvc.ViewModels;

namespace PerguntasRespostas.UI.Mvc.Controllers
{
    [Authorize]
    public class PerguntasController : Controller
    {
        HttpClient client;
        Uri perguntaUri;
        public PerguntasController()
        {
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:44362");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));                
            }
        }
        public IActionResult Index()
        {
            HttpResponseMessage response = client.GetAsync("api/v1.0/perguntas/todas-perguntas").Result;

            //se retornar com sucesso busca os dados
            if (response.IsSuccessStatusCode)
            {
                //Pegando os dados do Rest e armazenando na variável perguntas
                var perguntas = response.Content.ReadAsAsync<IEnumerable<PerguntaViewModel>>().Result;
                var percount = perguntas.Count();
                ViewBag.Perguntas = percount;

                //SOMATORIO DE CATEGORIAS PARA O DASHBOARD
                response = client.GetAsync("api/v1.0/categorias").Result;
                var categorias = response.Content.ReadAsAsync<IEnumerable<CategoriaViewModel>>().Result;
                var catcount = categorias.Count();
                ViewBag.Categorias = catcount;

                return PartialView(perguntas);
            }            

            return View();
        }

        public IActionResult MinhasPerguntas()
        {
            //****************************************************************
            //PRECISO PASSAR O BEARER PREENCHDIO AQUI PARA CONSEGUIR RECUPERAR
            //****************************************************************

            HttpResponseMessage response = client.GetAsync("api/v1.0/perguntas/minhas-perguntas").Result;

            //se retornar com sucesso busca os dados
            if (response.IsSuccessStatusCode)
            {

                //Pegando os dados do Rest e armazenando na variável perguntas
                var perguntas = response.Content.ReadAsAsync<IEnumerable<PerguntaViewModel>>().Result;

                return PartialView(perguntas);
            }
            return View();
        }

        public IActionResult Create()
        {
            HttpResponseMessage response = client.GetAsync("api/v1.0/categorias").Result;
            ViewBag.CategoriaId = null;
            if (response.IsSuccessStatusCode)
            {
                ViewBag.CategoriaId = new SelectList(response.Content.ReadAsAsync<IEnumerable<CategoriaViewModel>>().Result, "Id", "Titulo"); ;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PerguntaViewModel perguntaViewModel)
        {
            if (ModelState.IsValid)
            {
                perguntaViewModel.Autor = User.Identity.Name;
                var response = await client.PostAsJsonAsync("api/v1.0/perguntas/criar-pergunta", perguntaViewModel);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Perguntas");
                }
            }
            return View(perguntaViewModel);
        }

        // DELETE: api/Perguntas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pergunta = await client.GetAsync($"api/v1.0/perguntas/obter-pergunta/{id}");
            if (pergunta == null)
            {
                return NotFound();
            }

            if (pergunta.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Perguntas");
            }

            await client.GetAsync($"api/v1.0/perguntas/remover-pergunta/{id}");

            return Ok(pergunta);
        }
    }
}