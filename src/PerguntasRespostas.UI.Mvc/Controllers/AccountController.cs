using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PerguntasRespostas.UI.Mvc.Controllers
{
    
    public class AccountController : Controller
    {
        HttpClient client;
        Uri perguntaUri;
        public AccountController()
        {
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:44362");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Perguntas");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Models.LoginViewModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var Content = new StringContent(json, UnicodeEncoding.UTF8, "application/json");


            HttpResponseMessage response = client.PostAsync("api/v1.0/Account", Content).Result;

            //se retornar com sucesso busca os dados
            if (response.IsSuccessStatusCode)
            {               
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, model.Login));
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
                var id = new ClaimsIdentity(claims, "password");
                var principal = new ClaimsPrincipal(id);
                
                await HttpContext.SignInAsync("app", principal, new AuthenticationProperties() { IsPersistent = model.IsPersistent });

                return RedirectToAction("Index", "Perguntas");
            }
            return View();
        }

        public async Task<IActionResult> Logoff()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}