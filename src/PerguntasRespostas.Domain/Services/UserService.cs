using PerguntasRespostas.Domain.Entities;
using PerguntasRespostas.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PerguntasRespostas.Domain.Services
{
    public class UserService : IUserService
    {
        public UserService()
        {            
        }
       
        private User user;
        public IEnumerable<Claim> GetClaimsIdentity()
        {
            throw new NotImplementedException();
        }

        public Guid GetUserId()
        {            
            return user.Id;
        }

        public bool IsAuthenticated()
        {   
            return user.Login == "wagner" && user.Senha == "abc123@";
        }

        public async Task<bool> IsValidUserAndPasswordCombination(string login, string password)
        {
            if (login == "wagner" && password == "abc123@")
            {
                user = User.getFixUser();
                return true;
            }
            else
                user = new User();

            return false;
        }

        public User GetUser()
        {
            if (user != null)
                return user;

            return null;
        }
    }
}
