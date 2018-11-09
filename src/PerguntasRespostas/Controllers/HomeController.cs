using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PerguntasRespostas.Models;
using PerguntasRespostas.ViewModel;

namespace PerguntasRespostas.Controllers
{
    public class HomeController : Controller
    {
        HttpClient client;
        Uri perguntaUri;
        public HomeController()
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
                var percount = perguntas==null?0:perguntas.Count();
                ViewBag.Perguntas = percount;

                //SOMATORIO DE CATEGORIAS PARA O DASHBOARD
                response = client.GetAsync("api/v1.0/categorias").Result;
                var categorias = response.Content.ReadAsAsync<IEnumerable<CategoriaViewModel>>().Result;
                var catcount = categorias == null?0:categorias.Count();
                ViewBag.Categorias = catcount;


                //SOMATORIO DE RESPOSTAS PARA O DASHBOARD
                response = client.GetAsync("api/v1.0/respostas/todas-respostas").Result;
                var respostas = response.Content.ReadAsAsync<IEnumerable<RespostasViewModel>>().Result;
                var respcount = respostas==null?0:respostas.Count();
                ViewBag.Respostas = respcount;
                return View();
            }

            return View();
        }      
    }
}
