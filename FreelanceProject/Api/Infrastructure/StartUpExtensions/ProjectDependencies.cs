using Api.Infrastructure.Helpers;
using Api.Infrastructure.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;
using Repository;
using Repository.Infrastructure;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Api.Infrastructure.StartUpExtensions
{
    public static class ProjectDependencies
    {
        public static IServiceCollection AddProjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var repositoryAssembly = Assembly.GetAssembly(typeof(CityRepository));

            services.RegisterAssemblyPublicNonGenericClasses(repositoryAssembly)
                .Where(c => c.Name.EndsWith("Repository"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            services.RegisterAssemblyPublicNonGenericClasses(repositoryAssembly)
                .Where(c => c.Name.EndsWith("Command"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            services.RegisterAssemblyPublicNonGenericClasses(repositoryAssembly)
                .Where(c => c.Name.EndsWith("Query"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            var serviceAssembly = Assembly.GetAssembly(typeof(CityService));

            services.RegisterAssemblyPublicNonGenericClasses(serviceAssembly)
                .Where(c => c.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<INotifyService, NotifyService>();
            services.AddScoped<ISmsService, SmsService>();

            return services;
        }

    }
}
