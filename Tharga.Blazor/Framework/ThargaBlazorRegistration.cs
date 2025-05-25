using Microsoft.Extensions.DependencyInjection;
using Tharga.Blazor.Features.BreadCrumbs;

namespace Tharga.Blazor.Framework;

public static class ThargaBlazorRegistration
{
    public static void RegisterThargaBlazor(this IServiceCollection services, Action<ThargaBlazorOptions> options = default)
    {
        services.AddScoped<BreadCrumbService>();

        services.Configure(options ?? (_ => new ThargaBlazorOptions()));
    }
}