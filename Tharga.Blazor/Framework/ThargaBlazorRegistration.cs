using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using Tharga.Blazor.Features.BreadCrumbs;
using Tharga.Blazor.Features.Team;

namespace Tharga.Blazor.Framework;

public static class ThargaBlazorRegistration
{
    public static void RegisterThargaBlazor(this IServiceCollection services, Action<ThargaBlazorOptions> options = default)
    {
        services.AddScoped<BreadCrumbService>();
        services.AddScoped<ITeamStateService, TeamStateService>();
        services.AddBlazoredLocalStorage();

        services.Configure(options ?? (_ => new ThargaBlazorOptions()));
    }
}