
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PerguntasRespostas.Domain.Interfaces.Repositories;
using PerguntasRespostas.Domain.Interfaces.Services;
using PerguntasRespostas.Domain.Services;
using PerguntasRespostas.Infra.Data.Context;
using PerguntasRespostas.Infra.Data.Repositories;
using System.IO;


namespace PerguntasRespostas.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASPNET
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));

            services.AddScoped(typeof(IServiceBase<>), typeof(ServiceBase<>));
            services.AddTransient<IPerguntaService, PerguntaService>();
            services.AddTransient<IRespostaService, RespostaService>();
            services.AddTransient<ICategoriaService, CategoriaService>();
            services.AddTransient<IUserService, UserService>();


            // Infra - Data
            services.AddScoped(typeof(IRepositoryBase<>),typeof(RepositoryBase<>));
            services.AddTransient<IPerguntaRepository, PerguntaRepository>();
            services.AddTransient<IRespostaRepository, RespostaRepository>();
            services.AddTransient<ICategoriaRepository, CategoriaRepository>();



            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();

            services.AddDbContext<DBContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));           
        }
    }
}