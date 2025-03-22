using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;

namespace Notes.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            //services.AddMediatR(Assembly.GetExecutingAssembly()); 
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}
