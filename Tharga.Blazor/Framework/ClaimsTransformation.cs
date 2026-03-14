using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Tharga.Api;
using Tharga.Team;
using Tharga.Toolkit;

namespace Tharga.Blazor.Framework;

public class ClaimsTransformation : IClaimsTransformation
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITeamService _teamService;
    private readonly IUserService _userService;
    private readonly IScopeRegistry _scopeRegistry;

    public ClaimsTransformation(
        IHttpContextAccessor httpContextAccessor,
        ITeamService teamService,
        IUserService userService,
        IScopeRegistry scopeRegistry = null)
    {
        _httpContextAccessor = httpContextAccessor;
        _teamService = teamService;
        _userService = userService;
        _scopeRegistry = scopeRegistry;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var context = _httpContextAccessor.HttpContext;
        var identity = (ClaimsIdentity)principal.Identity;

        if (identity == null || !identity.IsAuthenticated) return principal;

        if (identity.HasClaim(c => c.Type == Constants.TeamKeyCookie)) return principal;

        var teamKey = context?.Request.Cookies[Constants.SelectedTeamKeyCookie];

        var user = await _userService.GetCurrentUserAsync(principal);

        if (!string.IsNullOrEmpty(teamKey))
        {
            identity.AddClaim(new Claim(Constants.TeamKeyCookie, teamKey));
            identity.AddClaim(new Claim(TeamClaimTypes.TeamKey, teamKey));

            var member = await _teamService.GetTeamMemberAsync(teamKey, user?.Key);
            if (member != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, Roles.TeamMember));
                identity.AddClaim(new Claim(ClaimTypes.Role, $"Team{member.AccessLevel}"));
                identity.AddClaim(new Claim(TeamClaimTypes.AccessLevel, member.AccessLevel.ToString()));

                if (_scopeRegistry != null)
                {
                    foreach (var scope in _scopeRegistry.GetEffectiveScopes(member.AccessLevel, member.TenantRoles))
                    {
                        identity.AddClaim(new Claim(TeamClaimTypes.Scope, scope));
                    }
                }
            }
        }

        return principal;
    }
}
