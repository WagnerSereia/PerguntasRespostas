using PerguntasRespostas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PerguntasRespostas.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Guid GetUserId();
        bool IsAuthenticated();        
        Task<bool> IsValidUserAndPasswordCombination(string login, string senha);
        User GetUser();
    }
}
