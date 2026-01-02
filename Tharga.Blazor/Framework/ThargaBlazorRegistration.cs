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
            //TODO: builder.Services.AddScoped<ITeamService<TeamEntity>>(sp => sp.GetRequiredService<TeamService>());

            if (o._userService.Service != null)
            {
                services.AddScoped(o._userService.Service);
                services.AddScoped(typeof(IUserService), sp => sp.GetRequiredService(o._userService.Service));
                //services.AddScoped(typeof(IUserRep), o._userService.Repository);
            }
            else
            {
            }

            //if (o._teamRepository != null)
            //{
            //    //services.AddScoped(o._teamRepository);
            //    //services.AddScoped(typeof(ITeamRepo), sp => sp.GetRequiredService(o._teamRepository));
            //}
            //else
            //{
            //}

            services.AddTransient<IClaimsTransformation, ClaimsTransformation>();
        }
        else
        {
            if (o._userService.Service != null)
            {
            }

            //if (o._teamRepository != null)
            //{
            //}
        }

        services.AddSingleton(Options.Create(o));
    }

    //public static void UseThargaBlazor(this IApplicationBuilder app)
    //{
    //    app.Map("/TeamInvite", async context =>
    //    {
    //        //TODO: Read query parameter "inviteCode".
    //        //TODO: Store that in a cookie.
    //        //TODO: Redirect to login.
    //    });
    //}
}