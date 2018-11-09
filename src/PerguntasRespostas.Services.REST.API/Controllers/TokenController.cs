using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PerguntasRespostas.Domain.Entities;
using PerguntasRespostas.Domain.Interfaces.Services;
using PerguntasRespostas.Services.REST.API.ViewModel;

namespace PerguntasRespostas.Services.REST.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class TokenController : Controller
    {
        private readonly IUserService _userService;
        public TokenController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]LoginViewModel model)
        {            
            if (await _userService.IsValidUserAndPasswordCombination(model.Login, model.Senha))
            {                
                var user = _userService.GetUser();
                var token = GenerateToken(user);
                //Salvar no DB
                return new ObjectResult(token);
            }
            return View();
        }

        private string GenerateToken(User user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),            
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString()),
            };

            var text = Encoding.UTF8.GetBytes("the secret that needs to be at least 16 characeters long");
            var key = new SymmetricSecurityKey(text);
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(credential);
            var payload = new JwtPayload(claims);
            var token = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(token);
        }        
    }
}