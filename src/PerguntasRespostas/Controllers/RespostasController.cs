using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerguntasRespostas.ViewModel;

namespace PerguntasRespostas.Controllers
{
    [Authorize]
    public class RespostasController : Controller
    {
        HttpClient client;
        Uri respostaUri;
        public RespostasController()
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
            HttpResponseMessage response = client.GetAsync("api/v1.0/respostas/todas-respostas").Result;

            //se retornar com sucesso busca os dados
            if (response.IsSuccessStatusCode)
            {
                //Pegando os dados do Rest e armazenando na variável respostas
                var respostas = response.Content.ReadAsAsync<IEnumerable<RespostasViewModel>>().Result;

                return PartialView(respostas);
            }
            return View();
        }

        public IActionResult Minhasrespostas()
        {
            //****************************************************************
            //PRECISO PASSAR O BEARER PREENCHDIO AQUI PARA CONSEGUIR RECUPERAR
            //****************************************************************

            HttpResponseMessage response = client.GetAsync("api/v1.0/respostas/minhas-respostas").Result;

            //se retornar com sucesso busca os dados
            if (response.IsSuccessStatusCode)
            {

                //Pegando os dados do Rest e armazenando na variável respostas
                var respostas = response.Content.ReadAsAsync<IEnumerable<RespostasViewModel>>().Result;

                return PartialView(respostas);
            }
            return View();
        }

        public async Task<IActionResult> ResponderPergunta(RespostasViewModel respostaViewModel)
        {
            if (ModelState.IsValid)
            {
                respostaViewModel.Autor = User.Identity.Name;
                var teste = respostaViewModel.Descricao;
                teste = respostaViewModel.PerguntaId.ToString();

                var response = await client.PostAsJsonAsync("api/v1.0/respostas/criar-resposta", respostaViewModel);
                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest();
                }
            }
            return RedirectToAction("MinhasPerguntasRespondidas", "Perguntas");
        }

        public IActionResult Create()
        {
            HttpResponseMessage response = client.GetAsync("api/v1.0/categorias").Result;
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RespostasViewModel RespostasViewModel)
        {
            if (ModelState.IsValid)
            {
                RespostasViewModel.Autor = User.Identity.Name;
                var response = await client.PostAsJsonAsync("api/v1.0/respostas/criar-resposta", RespostasViewModel);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(RespostasViewModel);
        }

        // DELETE: api/respostas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resposta = await client.GetAsync($"api/v1.0/respostas/obter-resposta/{id}");
            if (resposta == null)
            {
                return NotFound();
            }

            if (resposta.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            await client.GetAsync($"api/v1.0/respostas/remover-resposta/{id}");

            return Ok(resposta);
        }
    }
}