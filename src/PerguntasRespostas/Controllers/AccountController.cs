using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace PerguntasRespostas.Controllers
{
    public class AccountController : Controller
    {
        HttpClient client;
        private static string _urlBase;
        public AccountController()
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
            
            HttpResponseMessage response = client.PostAsync("Account", Content).Result;

            //se retornar com sucesso busca os dados
            if (response.IsSuccessStatusCode)
            {
                Retorno retorno = await response.Content.ReadAsAsync<Retorno>();

                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, model.Login));
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
                claims.Add(new Claim("AcessToken", string.Format("Bearer {0}", retorno.accessToken)));
                
                var identity = new ClaimsIdentity(claims, "ApplicationCookie");

                var principal = new ClaimsPrincipal(identity);


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

    public class Retorno
    {
        public Retorno()
        {

        }
        public bool success;
        public bool authenticated;
        public string accessToken;
        public string message;
    }
}