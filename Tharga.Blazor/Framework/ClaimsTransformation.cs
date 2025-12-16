using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Tharga.Team;
using Tharga.Toolkit;

namespace Tharga.Blazor.Framework;

public class ClaimsTransformation : IClaimsTransformation
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITeamService _teamService;

    public ClaimsTransformation(IHttpContextAccessor httpContextAccessor, ITeamService teamService)
    {
        _httpContextAccessor = httpContextAccessor;
        _teamService = teamService;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var context = _httpContextAccessor.HttpContext;
        var identity = (ClaimsIdentity)principal.Identity;

        if (identity == null || !identity.IsAuthenticated)
            return principal;

        // Don't duplicate roles
        if (identity.HasClaim(c => c.Type == "team_id"))
            return principal;

        //// Get tenant/team ID from a cookie or session
        var teamId = context?.Request.Cookies["selected_team_id"];
        //if (teamId == null)
        //    return Task.FromResult(principal);

        //var roles = teamId switch
        //{
        //    "team1" => new[] { "TenantOwner" },
        //    "team2" => new[] { "TenantAdmin" },
        //    _ => new[] { "TenantUser" }
        //};

        //identity.AddClaim(new Claim("team_id", teamId));
        //foreach (var role in roles)
        //    identity.AddClaim(new Claim(ClaimTypes.Role, role));

        if (!string.IsNullOrEmpty(teamId))
        {
            identity.AddClaim(new Claim("team_id", teamId));

            //TODO: Figure out what roles this user have in the team
            //var team = (TeamEntity)await _teamService.GetTeamsAsync(principal).FirstOrDefaultAsync(x => x.Key == teamId);
            //var member = team?.Members.FirstOrDefault(x => x.Key == principal.GetIdentity().Identity);
            //if (member != null)
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, $"Team{member.Role}"));
            //    //identity.AddClaim(new Claim(ClaimTypes.Role, $"Status{member.Status}"));
            //}
        }

        //TODO: When we use Entra, there should be a developer role to assign there.
        if (identity.GetEmail()?.Contains("daniel.bohlin", StringComparison.InvariantCultureIgnoreCase) ?? false)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, "Developer"));
        }

        return principal;
    }
}