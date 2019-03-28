using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PerguntasRespostas.Models;
using PerguntasRespostas.ViewModel;

namespace PerguntasRespostas.Controllers
{
    [Authorize]
    public class RespostasController : Controller
    {
        HttpClient client;
        private static string _urlBase;
        public RespostasController()
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
            HttpResponseMessage response = client.GetAsync("respostas/todas-respostas").Result;

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
            HttpResponseMessage response = client.GetAsync("respostas/minhas-respostas").Result;

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

                var response = await client.PostAsJsonAsync("respostas/criar-resposta", respostaViewModel);
                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest();
                }
            }
            return RedirectToAction("MinhasPerguntasRespondidas", "Perguntas");
        }

        public IActionResult Create()
        {
            HttpResponseMessage response = client.GetAsync("categorias").Result;
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RespostasViewModel RespostasViewModel)
        {
            if (ModelState.IsValid)
            {
                RespostasViewModel.Autor = User.Identity.Name;
                var response = await client.PostAsJsonAsync("respostas/criar-resposta", RespostasViewModel);
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

            var resposta = await client.GetAsync($"respostas/obter-resposta/{id}");
            if (resposta == null)
            {
                return NotFound();
            }

            if (resposta.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            await client.GetAsync($"respostas/remover-resposta/{id}");

            return Ok(resposta);
        }
    }
}