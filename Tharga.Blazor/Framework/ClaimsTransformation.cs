using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Tharga.Team;
using Tharga.Toolkit;

namespace Tharga.Blazor.Framework;

internal static class Constants
{
    public const string TeakKeyCookie = "team_id";
    public const string SelectedTeakKeyCookie = "selected_team_id";
}

public class ClaimsTransformation : IClaimsTransformation
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITeamService _teamService;
    private readonly IUserService _userService;

    public ClaimsTransformation(IHttpContextAccessor httpContextAccessor, ITeamService teamService, IUserService userService)
    {
        _httpContextAccessor = httpContextAccessor;
        _teamService = teamService;
        _userService = userService;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var context = _httpContextAccessor.HttpContext;
        var identity = (ClaimsIdentity)principal.Identity;

        if (identity == null || !identity.IsAuthenticated) return principal;

        if (identity.HasClaim(c => c.Type == Constants.TeakKeyCookie)) return principal;

        // Get tenant/team ID from a cookie or session
        var teamKey = context?.Request.Cookies[Constants.SelectedTeakKeyCookie];

        var user = await _userService.GetOrCreateAsync(principal);

        if (!string.IsNullOrEmpty(teamKey))
        {
            identity.AddClaim(new Claim(Constants.TeakKeyCookie, teamKey));

            //TODO: Figure out what roles this user have in the team
            //var team = await _teamService.GetTeamsAsync(principal).FirstOrDefaultAsync(x => x.Key == teamId);
            //var member = team?.Members.FirstOrDefault(x => x.Key == principal.GetIdentity().Identity);
            var member = await _teamService.GetTeamMemberAsync(teamKey, user.Key, principal);
            if (member != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, $"Team{member.AccessLevel}"));
                //identity.AddClaim(new Claim(ClaimTypes.Role, $"Status{member.Status}"));
            }
        }

        //TODO: When we use Entra, there should be a developer role to assign there.
        if (identity.GetEmail()?.Contains("daniel.bohlin", StringComparison.InvariantCultureIgnoreCase) ?? false)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, "Developer"));
        }

        return principal;
    }
}