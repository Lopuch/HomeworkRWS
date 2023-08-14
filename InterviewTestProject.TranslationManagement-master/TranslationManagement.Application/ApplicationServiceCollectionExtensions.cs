using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Application.BackgroundWorkers;
using TranslationManagement.Application.Database;
using TranslationManagement.Application.Repositories;
using TranslationManagement.Application.Services;

namespace TranslationManagement.Application;
public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITranslationJobRepo, TranslationJobRepo>();
        services.AddScoped<ITranslatorModelRepo, TranslatorModelRepo>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ITranslatorModelService, TranslatorModelService>();
        services.AddScoped<ITranslationJobService, TranslationJobService>();

        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Scoped);

        services.AddSingleton<INotificationService, NotificationService>();

        services.AddHostedService<NotificationBackgroundWorker>();

        return services;
    }
}
