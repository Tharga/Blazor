using Blazored.LocalStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tharga.Blazor.Features.BreadCrumbs;
using Tharga.Blazor.Framework.Localization;

namespace Tharga.Blazor.Framework;

public static class BlazorRegistration
{
    public static void AddThargaBlazor(this IServiceCollection services, Action<BlazorOptions> configure = null, IConfiguration configuration = null)
    {
        services.AddScoped<BreadCrumbService>();
        services.AddBlazoredLocalStorage();
        services.TryAddScoped<IBlazorLanguageProvider, DefaultBlazorLanguageProvider>();
        services.TryAddScoped<LanguageResolver>();

        if (configuration != null)
        {
            services.Configure<BlazorOptions>(configuration.GetSection("Tharga:Blazor"));
        }

        var options = new BlazorOptions();
        configure?.Invoke(options);

        services.Configure<BlazorOptions>(bo =>
        {
            if (options.Title != null) bo.Title = options.Title;
            if (options.ShowExceptionDetails != null) bo.ShowExceptionDetails = options.ShowExceptionDetails;
            if (options.Language != null) bo.Language = options.Language;
        });
    }
}
