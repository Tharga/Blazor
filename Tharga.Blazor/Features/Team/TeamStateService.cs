using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Tharga.Team;

namespace Tharga.Blazor.Features.Team;

public record TeamDialogModel
{
    public string Name { get; set; }
}

internal class TeamStateService : ITeamStateService
{
    private readonly ITeamService _teamService;
    private readonly NavigationManager _navigationManager;
    private readonly ILocalStorageService _localStorageService;
    private readonly IJSRuntime _jSRuntime;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private ITeam _selectedTeam;

    public TeamStateService(ITeamService teamService, NavigationManager navigationManager, ILocalStorageService localStorageService, IJSRuntime jSRuntime, AuthenticationStateProvider authenticationStateProvider)
    {
        _teamService = teamService;
        _navigationManager = navigationManager;
        _localStorageService = localStorageService;
        _jSRuntime = jSRuntime;
        _authenticationStateProvider = authenticationStateProvider;

        _teamService.TeamsListChangedEvent += (s, e) => { TeamsListChangedEvent?.Invoke(s, e); };
    }

    public event EventHandler<TeamsListChangedEventArgs> TeamsListChangedEvent;
    public event EventHandler<SelectedTeamChangedEventArgs> SelectedTeamChangedEvent;

    public async Task<ITeam> GetSelectedTeamAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (!(authState.User.Identity?.IsAuthenticated ?? false)) return null;

        try
        {
            await _semaphore.WaitAsync();

            var teams = await _teamService.GetTeamsAsync().ToArrayAsync();

            if (_selectedTeam == null || teams.All(x => x.Key != _selectedTeam.Key) || teams.FirstOrDefault(x => x.Key == _selectedTeam.Key)?.Name != _selectedTeam.Name)
            {
                var t = authState.User.Claims.FirstOrDefault(x => x.Type == "team_id");
                if (t != null)
                {
                    var team = teams.FirstOrDefault(x => x.Key == t.Value) ?? teams.FirstOrDefault();
                    await AssignTeamAsync(team);
                }
                else if (!teams.Any())
                {
                    var team = await _teamService.CreateTeamAsync();
                    await AssignTeamAsync(team, true);
                }
                else if (teams.Length == 1)
                {
                    await AssignTeamAsync(teams.Single(), true);
                }
                else
                {
                    var teamKey = await _localStorageService.GetItemAsStringAsync("SelectedTeam");
                    await AssignTeamAsync(teams.FirstOrDefault(x => x.Key == teamKey) ?? teams.FirstOrDefault(), true);
                }
            }

            return _selectedTeam;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task AssignTeamAsync(ITeam team, bool refresh = false)
    {
        _selectedTeam = team;

        if (refresh)
        {
            await _jSRuntime.InvokeVoidAsync("eval", $"document.cookie = 'selected_team_id={_selectedTeam.Key}; path=/'");
            _navigationManager.Refresh(true);
            return;
        }

        SelectedTeamChangedEvent?.Invoke(this, new SelectedTeamChangedEventArgs(_selectedTeam));
    }

    public async Task<string> GetSelectedTeamKeyAsync()
    {
        var team = await GetSelectedTeamAsync();
        return team?.Key;
    }

    public async Task SetSelectedTeamAsync(ITeam selectedTeam)
    {
        await _teamService.SetLastSeenAsync(selectedTeam);

        if (_selectedTeam.Key == selectedTeam.Key) return;

        _selectedTeam = selectedTeam;
        await _localStorageService.SetItemAsStringAsync("SelectedTeam", selectedTeam.Key);

        await _jSRuntime.InvokeVoidAsync("eval", $"document.cookie = 'selected_team_id={_selectedTeam.Key}; path=/'");
        _navigationManager.Refresh(true);
    }
}