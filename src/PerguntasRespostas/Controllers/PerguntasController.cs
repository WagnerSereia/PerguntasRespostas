using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using PerguntasRespostas.ViewModel;

namespace PerguntasRespostas.Controllers
{
    [Authorize]
    public class PerguntasController : Controller
    {
        HttpClient client;
        private static string _urlBase;
        public PerguntasController()
        {
            #region Recupera as configurações base de url do appSetting
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile($"appsettings.json");
            var config = builder.Build();

            _urlBase = config.GetSection("API_Access:UrlBase").Value;
            #endregion

            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(_urlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
        public IActionResult Index()
        {
            HttpResponseMessage response = client.GetAsync("perguntas/todas-perguntas").Result;

            //se retornar com sucesso busca os dados
            if (response.IsSuccessStatusCode)
            {
                //Pegando os dados do Rest e armazenando na variável perguntas
                var perguntas = response.Content.ReadAsAsync<IEnumerable<PerguntaViewModel>>().Result;
                var percount = perguntas.Count();
                ViewBag.Perguntas = percount;

                //SOMATORIO DE CATEGORIAS PARA O DASHBOARD
                response = client.GetAsync("categorias").Result;
                var categorias = response.Content.ReadAsAsync<IEnumerable<CategoriaViewModel>>().Result;
                var catcount = categorias.Count();
                ViewBag.Categorias = catcount;

                return View(perguntas);
            }

            return View();
        }
                
        public IActionResult MinhasPerguntas()
        {            
            var accessToken = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst((x) => x.Type == "AcessToken").Value.ToString();
            client.DefaultRequestHeaders.Add("Authorization", accessToken);

            HttpResponseMessage response = client.GetAsync("perguntas/minhas-perguntas").Result;

            //se retornar com sucesso busca os dados
            if (response.IsSuccessStatusCode)
            {

                //Pegando os dados do Rest e armazenando na variável perguntas
                var perguntas = response.Content.ReadAsAsync<IEnumerable<PerguntaViewModel>>().Result;

                return View(perguntas);
            }
            else
                return BadRequest("usuario não autenticado");

            //return View();
        }

        public IActionResult MinhasPerguntasRespondidas()
        {
            var accessToken = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst((x) => x.Type == "AcessToken").Value.ToString();
            client.DefaultRequestHeaders.Add("Authorization", accessToken);
            HttpResponseMessage response = client.GetAsync("perguntas/minhas-perguntas-respondidas").Result;

            //se retornar com sucesso busca os dados
            if (response.IsSuccessStatusCode)
            {

                //Pegando os dados do Rest e armazenando na variável perguntas
                var perguntas = response.Content.ReadAsAsync<IEnumerable<PerguntaViewModel>>().Result;

                return View(perguntas);
            }
            return View();
        }

        public IActionResult Create()
        {
            HttpResponseMessage response = client.GetAsync("categorias").Result;
            ViewBag.CategoriaId = null;
            if (response.IsSuccessStatusCode)
            {
                ViewBag.CategoriaId = new SelectList(response.Content.ReadAsAsync<IEnumerable<CategoriaViewModel>>().Result, "Id", "Titulo");
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
                await client.PostAsJsonAsync("perguntas/criar-pergunta", perguntaViewModel);
                return RedirectToAction(nameof(Index));

            }
            return View(perguntaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Responder(PerguntaViewModel perguntaViewModel)
        {
            if (ModelState.IsValid)
            {
                perguntaViewModel.Autor = User.Identity.Name;

                var response = await client.PostAsJsonAsync("resposta/criar-resposta", perguntaViewModel.Respostas);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(MinhasPerguntasRespondidas));
                }
            }
            return View(perguntaViewModel);
        }

        // GET: api/Perguntas/5
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            var response = await client.GetAsync($"perguntas/obter-pergunta/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(response);
            }

            var perguntaViewModel = response.Content.ReadAsAsync<PerguntaViewModel>().Result;

            return View(perguntaViewModel);
        }

        // DELETE: api/Perguntas/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(PerguntaViewModel perguntaViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accessToken = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst((x) => x.Type == "AcessToken").Value.ToString();
            client.DefaultRequestHeaders.Add("Authorization", accessToken);

            var response = await client.DeleteAsync($"perguntas/remover-pergunta/{perguntaViewModel.Id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<ActionResult> Edit([FromRoute] Guid id)
        {
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            var response = await client.GetAsync($"perguntas/obter-pergunta/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(response);
            }

            var perguntaViewModel = response.Content.ReadAsAsync<PerguntaViewModel>().Result;

            response = client.GetAsync("categorias").Result;
            ViewBag.CategoriaId = null;
            if (response.IsSuccessStatusCode)
            {
                ViewBag.CategoriaId = new SelectList(response.Content.ReadAsAsync<IEnumerable<CategoriaViewModel>>().Result, "Id", "Titulo");
            }

            return View(perguntaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PerguntaViewModel perguntaViewModel)
        {
            if (ModelState.IsValid)
            {
                var accessToken = ((ClaimsIdentity)HttpContext.User.Identity).FindFirst((x) => x.Type == "AcessToken").Value.ToString();
                client.DefaultRequestHeaders.Add("Authorization", accessToken);
                var response = await client.PutAsJsonAsync($"perguntas/atualizar-pergunta/{perguntaViewModel.Id}", perguntaViewModel);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(perguntaViewModel);
        }

        public async Task<IActionResult> Details([FromRoute]Guid id)
        {
            if (id == null)
            {
                return BadRequest(ModelState);
            }

            var response = await client.GetAsync($"perguntas/obter-pergunta/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(response);
            }

            var perguntaViewModel = response.Content.ReadAsAsync<PerguntaViewModel>().Result;

            response = client.GetAsync("categorias").Result;
            ViewBag.CategoriaId = null;
            if (response.IsSuccessStatusCode)
            {
                ViewBag.CategoriaId = new SelectList(response.Content.ReadAsAsync<IEnumerable<CategoriaViewModel>>().Result, "Id", "Titulo");
            }

            return View(perguntaViewModel);
        }
    }
}