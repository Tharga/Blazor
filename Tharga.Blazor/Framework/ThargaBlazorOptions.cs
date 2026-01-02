using Tharga.Team;

namespace Tharga.Blazor.Framework;

public record ThargaBlazorOptions
{
    internal Type _teamService;
    internal (Type Service, Type Repository) _userService;
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

    public void RegisterUserService<TUserService, TUserRepository>()
        //where TUserService : UserServiceBase
        //where TUserRepository : IUserRep
    {
        _userService = (typeof(TUserService), typeof(TUserRepository));
    }

    //public void RegisterTeamRepository<T>()
    //    where T : ITeamRepository
    //{
    //    _teamRepository = typeof(T);
    //}
}