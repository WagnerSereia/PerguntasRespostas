using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PerguntasRespostas.ViewModel;

namespace PerguntasRespostas.Controllers
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

                return View(perguntas);
            }

            return View();
        }

        public IActionResult MinhasPerguntas()
        {
            //****************************************************************
            //PRECISO PASSAR O BEARER PREENCHDIO AQUI PARA CONSEGUIR RECUPERAR
            //****************************************************************
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoid2FnbmVyIiwibmJmIjoiMTU0MTcwNjMyNyIsImV4cCI6IjE1NDE3OTI3MjcifQ.HyA76-eaku2YQ0NleIxxNvkfEjlaOsL-55ZkycAtlBs";
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            HttpResponseMessage response = client.GetAsync("api/v1.0/perguntas/minhas-perguntas").Result;

            //se retornar com sucesso busca os dados
            if (response.IsSuccessStatusCode)
            {

                //Pegando os dados do Rest e armazenando na variável perguntas
                var perguntas = response.Content.ReadAsAsync<IEnumerable<PerguntaViewModel>>().Result;

                return View(perguntas);
            }
            return View();
        }

        public IActionResult MinhasPerguntasRespondidas()
        {
            //****************************************************************
            //PRECISO PASSAR O BEARER PREENCHDIO AQUI PARA CONSEGUIR RECUPERAR
            //****************************************************************
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoid2FnbmVyIiwibmJmIjoiMTU0MTcwNjMyNyIsImV4cCI6IjE1NDE3OTI3MjcifQ.HyA76-eaku2YQ0NleIxxNvkfEjlaOsL-55ZkycAtlBs";
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            HttpResponseMessage response = client.GetAsync("api/v1.0/perguntas/minhas-perguntas-respondidas").Result;

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
            HttpResponseMessage response = client.GetAsync("api/v1.0/categorias").Result;
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
                await client.PostAsJsonAsync("api/v1.0/perguntas/criar-pergunta", perguntaViewModel);
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
                
                var response = await client.PostAsJsonAsync("api/v1.0/resposta/criar-resposta", perguntaViewModel.Respostas);
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

            var response = await client.GetAsync($"api/v1.0/perguntas/obter-pergunta/{id}");
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

            var response = await client.DeleteAsync($"api/v1.0/perguntas/remover-pergunta/{perguntaViewModel.Id}");
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

            var response = await client.GetAsync($"api/v1.0/perguntas/obter-pergunta/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(response);
            }

            var perguntaViewModel = response.Content.ReadAsAsync<PerguntaViewModel>().Result;

            response = client.GetAsync("api/v1.0/categorias").Result;
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
                var response = await client.PutAsJsonAsync($"api/v1.0/perguntas/atualizar-pergunta/{perguntaViewModel.Id}", perguntaViewModel);

                if(response.IsSuccessStatusCode)
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

            var response = await client.GetAsync($"api/v1.0/perguntas/obter-pergunta/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(response);
            }

            var perguntaViewModel = response.Content.ReadAsAsync<PerguntaViewModel>().Result;

            response = client.GetAsync("api/v1.0/categorias").Result;
            ViewBag.CategoriaId = null;
            if (response.IsSuccessStatusCode)
            {
                ViewBag.CategoriaId = new SelectList(response.Content.ReadAsAsync<IEnumerable<CategoriaViewModel>>().Result, "Id", "Titulo");
            }

            return View(perguntaViewModel);
        }
    }
}