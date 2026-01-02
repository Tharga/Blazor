using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tharga.Blazor.Features.BreadCrumbs;
using Tharga.Blazor.Features.Team;
using Tharga.Team;

namespace Tharga.Blazor.Framework;

public static class ThargaBlazorRegistration
{
    public static void AddThargaBlazor(this IServiceCollection services, Action<ThargaBlazorOptions> options = null)
    {
        services.AddScoped<BreadCrumbService>();
        services.AddBlazoredLocalStorage();

        var o = new ThargaBlazorOptions();
        options?.Invoke(o);

        if (o._teamService != null)
        {
            services.AddThargaTeam();
            services.AddScoped<ITeamStateService, TeamStateService>();

            services.AddScoped(o._teamService);
            services.AddScoped(typeof(ITeamService), sp => sp.GetRequiredService(o._teamService));

            services.AddScoped(o._userService);
            services.AddScoped(typeof(IUserService), sp => sp.GetRequiredService(o._userService));

            services.AddTransient<IClaimsTransformation, ClaimsTransformation>();
        }

        services.AddSingleton(Options.Create(o));
    }
}