using Microsoft.AspNetCore.Mvc;


namespace PerguntasRespostas.UI.Mvc.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        
    }
}
