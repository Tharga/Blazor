using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace Tharga.Blazor.Features.Team;

internal class TeamStateService : ITeamStateService
{
    //private readonly ConcurrentDictionary<string, bool> _doNotRedirect = new();
    private readonly NavigationManager _navigationManager;
    private readonly ISyncLocalStorageService _localStorageService;
    private ITeam _selectedTeam;

    public TeamStateService(NavigationManager navigationManager, ISyncLocalStorageService localStorageService)
    {
        _navigationManager = navigationManager;
        _localStorageService = localStorageService;
    }

    //public event EventHandler<FarmChangedEventArgs> FarmChangedEvent;
    //public event EventHandler<EventArgs> RequestReloadEvent;

    public ITeam SelectedTeam => _selectedTeam;

    public void SetCurrent(ITeam team)
    {
        _selectedTeam = team;
        //_localStorageService.SetItem("currentTeam", team.Key);
        //FarmChangedEvent?.Invoke(this, new FarmChangedEventArgs(team));
        //if (_doNotRedirect.TryGetValue(_navigationManager.Uri, out _)) return;
    }

    public void SetCurrentAndRefresh(ITeam team)
    {
        SetCurrent(team);
        _navigationManager.NavigateTo("/");
    }

    //public void OverrideRedirect(Action action)
    //{
    //    _doNotRedirect.TryAdd(_navigationManager.Uri, true);
    //    FarmChangedEvent += (_, _) => { action?.Invoke(); };
    //}

    //public void RequestReload()
    //{
    //    RequestReloadEvent?.Invoke(this, EventArgs.Empty);
    //}
}