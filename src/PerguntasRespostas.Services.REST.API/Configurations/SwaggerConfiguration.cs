using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerguntasRespostas.Services.REST.API.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "FIAP.PerguntasRespostas API",
                    Description = "API do site FIAP.PerguntasRespostas.IO",
                    TermsOfService = "Para uso exclusivo de estudo",
                    Contact = new Contact { Name = "Wagner Sereia", Email = "wsinfo@msn.com", Url = "http://FIAP.PerguntasRespostas.IO" },
                    License = new License { Name = "MIT", Url = "http://FIAP.PerguntasRespostas.IO/licensa" }
                });
            });
        }
    }
}
