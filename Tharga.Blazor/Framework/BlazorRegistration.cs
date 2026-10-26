using Blazored.LocalStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tharga.Blazor.Features.BreadCrumbs;
using Tharga.Blazor.Framework.Localization;

namespace Tharga.Blazor.Framework;

public static class BlazorRegistration
{
    /// <summary>
    /// Registers the services Tharga.Blazor components depend on, and binds <see cref="BlazorOptions"/>
    /// from <paramref name="configure"/> and the <c>Tharga:Blazor</c> section of <paramref name="configuration"/>.
    /// </summary>
    /// <remarks>
    /// This also registers Blazored.LocalStorage's <c>ILocalStorageService</c>. That registration is
    /// deprecated: no Tharga.Blazor component uses it any more, and it will be removed in a future major
    /// version. An application that injects <c>ILocalStorageService</c> should reference
    /// Blazored.LocalStorage and call <c>AddBlazoredLocalStorage()</c> itself.
    /// </remarks>
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
