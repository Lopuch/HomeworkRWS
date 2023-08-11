using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Application.Database;
using TranslationManagement.Application.Repositories;

namespace TranslationManagement.Application;
public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITranslationJobRepo, TranslationJobRepo>();
        services.AddScoped<ITranslatorModelRepo, TranslatorModelRepo>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        //services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
        return services;
    }
}
