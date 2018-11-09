using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerguntasRespostas.Models
{
    public class LoginViewModel
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool IsPersistent { get; set; }
    }
}
