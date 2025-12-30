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
    public static void AddThargaBlazor<TTeamService, TUserService>(this IServiceCollection services, Action<ThargaBlazorOptions> options = null)
        where TTeamService : ITeamService
        where TUserService : IUserService
    {
        services.AddScoped<BreadCrumbService>();
        services.AddBlazoredLocalStorage();

        var o = new ThargaBlazorOptions();
        options?.Invoke(o);

        //if (o._teamService != null)
        //{
            services.AddThargaTeam();
            services.AddScoped<ITeamStateService, TeamStateService>();

            services.AddScoped(typeof(TUserService));
            services.AddScoped(typeof(IUserService), sp => sp.GetRequiredService(typeof(TUserService)));

        services.AddScoped(typeof(TTeamService));
            services.AddScoped(typeof(ITeamService), sp => sp.GetRequiredService(typeof(TTeamService)));
            //TODO: builder.Services.AddScoped<ITeamService<TeamEntity>>(sp => sp.GetRequiredService<TeamService>());

            services.AddTransient<IClaimsTransformation, ClaimsTransformation>();
        //}

        services.AddSingleton(Options.Create(o));
    }
}