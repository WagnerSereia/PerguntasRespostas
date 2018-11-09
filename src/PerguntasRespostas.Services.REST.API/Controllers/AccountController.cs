using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PerguntasRespostas.Domain.Interfaces.Services;
using PerguntasRespostas.Services.REST.API.ViewModel;

namespace PerguntasRespostas.UI.Mvc.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{apiVersion}/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(await _userService.IsValidUserAndPasswordCombination(model.Login, model.Senha))
            {                
                var response = GerarTokenUsuario(model);
                return Ok(new
                {
                    success = true,
                    authenticated = true,                    
                    accessToken = response.Result,
                    message = "OK"
                });
            }
            else
                return BadRequest(new { success = false, message = "Falha na autenticacao" });
        }

        private async Task<object> GerarTokenUsuario(LoginViewModel login)
        {
            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao.AddDays(1);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, login.Login),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(dataCriacao).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(dataExpiracao).ToUnixTimeSeconds().ToString()),
            };
            var text = Encoding.UTF8.GetBytes("the secret that needs to be at least 16 characeters long for HmacSha256");
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