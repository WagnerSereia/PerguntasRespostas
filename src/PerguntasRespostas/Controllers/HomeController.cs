using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PerguntasRespostas.Models;
using PerguntasRespostas.ViewModel;

namespace PerguntasRespostas.Controllers
{
    public class HomeController : Controller
    {
        HttpClient client;
        private static string _urlBase;
        public HomeController()
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
                var percount = perguntas==null?0:perguntas.Count();
                ViewBag.Perguntas = percount;

                //SOMATORIO DE CATEGORIAS PARA O DASHBOARD
                response = client.GetAsync("categorias").Result;
                var categorias = response.Content.ReadAsAsync<IEnumerable<CategoriaViewModel>>().Result;
                var catcount = categorias == null?0:categorias.Count();
                ViewBag.Categorias = catcount;


                //SOMATORIO DE RESPOSTAS PARA O DASHBOARD
                response = client.GetAsync("respostas/todas-respostas").Result;
                var respostas = response.Content.ReadAsAsync<IEnumerable<RespostasViewModel>>().Result;
                var respcount = respostas==null?0:respostas.Count();
                ViewBag.Respostas = respcount;
                return View();
            }

            return View();
        }      
    }
}
