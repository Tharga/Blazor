using Blazored.LocalStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tharga.Blazor.Features.BreadCrumbs;

namespace Tharga.Blazor.Framework;

public static class BlazorRegistration
{
    public static void AddThargaBlazor(this IServiceCollection services, Action<BlazorOptions> configure = null, IConfiguration configuration = null)
    {
        services.AddScoped<BreadCrumbService>();
        services.AddBlazoredLocalStorage();

        if (configuration != null)
        {
            services.Configure<BlazorOptions>(configuration.GetSection("Tharga:Blazor"));
        }

        var options = new BlazorOptions();
        configure?.Invoke(options);

        services.Configure<BlazorOptions>(bo =>
        {
            if (options.Title != null) bo.Title = options.Title;
        });
    }
}
