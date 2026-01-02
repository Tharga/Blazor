using Tharga.Team;

namespace Tharga.Blazor.Framework;

public record ThargaBlazorOptions
{
    internal Type _teamService;
    internal Type _userService;

    public string Title { get; set; }

    /// <summary>
    /// Automatically create the first team for users.
    /// Default is false.
    /// </summary>
    public bool AutoCreateFirstTeam { get; set; } = false;

    /// <summary>
    /// Add types for team and user services.
    /// </summary>
    /// <typeparam name="TServiceBase"></typeparam>
    /// <typeparam name="TUserService"></typeparam>
    public void RegisterTeamService<TServiceBase, TUserService>()
        where TServiceBase : TeamServiceBase
        where TUserService : UserServiceBase
    {
        _teamService = typeof(TServiceBase);
        _userService = typeof(TUserService);
    }
}