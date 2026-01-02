using Tharga.Team;

namespace Tharga.Blazor.Framework;

public record ThargaBlazorOptions
{
    internal Type _teamService;
    internal Type _userService;
    //internal Type _teamRepository;

    public string Title { get; set; }

    /// <summary>
    /// Automatically create the first team for users.
    /// Default is false.
    /// </summary>
    public bool AutoCreateFirstTeam { get; set; } = false;

    public void RegisterTeamService<T>() where T : ITeamService
    {
        _teamService = typeof(T);
    }

    public void RegisterUserService<T>() where T : IUserService
    {
        _userService = typeof(T);
    }

    //public void RegisterTeamRepository<T>()
    //    where T : ITeamRepository
    //{
    //    _teamRepository = typeof(T);
    //}
}