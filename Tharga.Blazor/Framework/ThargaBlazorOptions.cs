using Tharga.Blazor.Features.Team;

namespace Tharga.Blazor.Framework;

public record ThargaBlazorOptions
{
    internal Type _teamService;

    public string Title { get; set; }

    public void RegisterTeamService<T>() where T : ITeamService
    {
        _teamService = typeof(T);
    }
}