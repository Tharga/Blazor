using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace Tharga.Blazor.Features.Team;

internal class TeamStateService : ITeamStateService
{
    private readonly ITeamService _teamService;
    private readonly NavigationManager _navigationManager;
    private readonly ILocalStorageService _localStorageService;
    private ITeam _selectedTeam;

    public TeamStateService(ITeamService teamService, NavigationManager navigationManager, ILocalStorageService localStorageService)
    {
        _teamService = teamService;
        _navigationManager = navigationManager;
        _localStorageService = localStorageService;

        _teamService.TeamsListChangedEvent += (s, e) => { TeamsListChangedEvent?.Invoke(s, e); };
    }

    public event EventHandler<TeamsListChangedEventArgs> TeamsListChangedEvent;
    public event EventHandler<SelectedTeamChangedEventArgs> SelectedTeamChangedEvent;

    public async Task<ITeam> GetSelectedTeamAsync()
    {
        if (_selectedTeam == null)
        {
            var teams = await _teamService.GetTeamsAsync().ToArrayAsync();
            if (!teams.Any())
            {
                var team = await _teamService.CreateTeamAsync();
                _selectedTeam = team;
                SelectedTeamChangedEvent?.Invoke(this, new SelectedTeamChangedEventArgs(_selectedTeam));
            }
            else if (teams.Length == 1)
            {
                _selectedTeam = teams.Single();
                SelectedTeamChangedEvent?.Invoke(this, new SelectedTeamChangedEventArgs(_selectedTeam));
            }
            else
            {
                var teamKey = await _localStorageService.GetItemAsStringAsync("SelectedTeam");
                _selectedTeam = teams.FirstOrDefault(x => x.Key == teamKey) ?? teams.FirstOrDefault();
                SelectedTeamChangedEvent?.Invoke(this, new SelectedTeamChangedEventArgs(_selectedTeam));
            }
        }

        return _selectedTeam;
    }

    public async Task<string> GetSelectedTeamKeyAsync()
    {
        var team = await GetSelectedTeamAsync();
        return team.Key;
    }

    public async Task SetSelectedTeamAsync(ITeam selectedTeam)
    {
        if (_selectedTeam.Key == selectedTeam.Key) return;

        _selectedTeam = selectedTeam;
        await _localStorageService.SetItemAsStringAsync("SelectedTeam", selectedTeam.Key);
        //SelectedTeamChangedEvent?.Invoke(this, new SelectedTeamChangedEventArgs(_selectedTeam));
        _navigationManager.Refresh(true);
    }

    //public ITeam SelectedTeam
    //{
    //    get
    //    {
    //        return _selectedTeam;
    //    }
    //    set
    //    {
    //        if (_selectedTeam == value) return;
    //        _selectedTeam = value;
    //        Task.Run(async () =>
    //        {
    //            await _localStorageService.SetItemAsStringAsync("SelectedTeam", value.Key);
    //        });
    //    }
    //}

    //public async Task<string> GetLastSelectedTeamKeyAsync()
    //{
    //    var result = await _localStorageService.GetItemAsStringAsync("SelectedTeam");
    //    return result;
    //}

    //public ITeam SelectedTeam => _selectedTeam;

    //public async Task SetCurrent(ITeam team)
    //{
    //    if (_selectedTeam?.Key == team.Key) return;

    //    var firstTime = _selectedTeam == null;
    //    _selectedTeam = team;
    //    await _localStorageService.SetItemAsStringAsync("Team", team.Key);
    //    if (!firstTime)
    //    {
    //        //TODO: Here the selection changed, we need to reload data but without missing the selected team
    //        _navigationManager.Refresh(true);
    //    }
    //    //_navigationManager.Refresh(refresh);
    //    OnTeamChangeEvent();
    //}

    ////public async Task<bool> SetCurrent(ITeam team, bool refresh)
    ////{
    ////    var change = _selectedTeam != null && _selectedTeam != team;
    ////    _selectedTeam = team;
    ////    //_localStorageService.SetItem("Team", team.Key);
    ////    await _localStorageService.SetItemAsStringAsync("Team", team.Key);
    ////    return change;
    ////}

    ////public async Task SetCurrentAndRefresh(ITeam team)
    ////{
    ////    var changed = await SetCurrent(team);
    ////    //_navigationManager.NavigateTo("/");
    ////    //_navigationManager.Refresh(changed);
    ////    _navigationManager.Refresh(false);
    ////    OnTeamChangeEvent();
    ////}

    //public void OnTeamChangeEvent()
    //{
    //    TeamChangeEvent?.Invoke(this, new TeamChangeEventArgs());
    //}

    //public async Task<string> GetDefaultTeamKey()
    //{
    //    try
    //    {
    //        return await _localStorageService.GetItemAsStringAsync("Team");
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}

    ////public void OverrideRedirect(Action action)
    ////{
    ////    _doNotRedirect.TryAdd(_navigationManager.Uri, true);
    ////    FarmChangedEvent += (_, _) => { action?.Invoke(); };
    ////}

    ////public void RequestReload()
    ////{
    ////    RequestReloadEvent?.Invoke(this, EventArgs.Empty);
    ////}
}