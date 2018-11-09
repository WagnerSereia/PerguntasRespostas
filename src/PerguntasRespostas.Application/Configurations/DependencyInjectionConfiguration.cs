using Microsoft.Extensions.DependencyInjection;
using PerguntasRespostas.Infra.CrossCutting.IoC;

namespace PerguntasRespostas.Application.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static void AddDIConfiguration(this IServiceCollection services)
        {
            NativeInjectorBootStrapper.RegisterServices(services);
        }
    }
}