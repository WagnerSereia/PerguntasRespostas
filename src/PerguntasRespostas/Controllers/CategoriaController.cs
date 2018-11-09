using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PerguntasRespostas.ViewModel;

namespace PerguntasRespostas.Controllers
{
    public class CategoriaController : Controller
    {
        HttpClient client;
        Uri perguntaUri;
        public CategoriaController()
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
            HttpResponseMessage response = client.GetAsync("api/v1.0/categorias").Result;

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

            @ViewBag.Autor = "Wagner Serea";


            return View();
        }
    }
}