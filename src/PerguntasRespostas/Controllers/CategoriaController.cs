using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PerguntasRespostas.ViewModel;

namespace PerguntasRespostas.Controllers
{
    public class CategoriaController : Controller
    {
        HttpClient client;
        private static string _urlBase;
        public CategoriaController()
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
            HttpResponseMessage response = client.GetAsync("categorias").Result;

            var categorias = response.Content.ReadAsAsync<IEnumerable<CategoriaViewModel>>().Result;
            return View(categorias);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoriaViewModel categoriaViewModel)
        {
            if (ModelState.IsValid)
            {

            }
            return View(categoriaViewModel);
        }


        public IActionResult Edit()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoriaViewModel categoriaViewModel)
        {
            if (ModelState.IsValid)
            {

            }
            return View(categoriaViewModel);
        }

        public IActionResult Details(int? id)
        {
            @ViewBag.Autor = User.Identity.Name;

            return View();
        }
    }
}