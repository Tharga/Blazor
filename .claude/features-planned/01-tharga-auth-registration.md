# Add auth registration helpers to Tharga.Blazor

**Requested by:** BlazorTemplate project (Tharga Implementation Guide)
**Date:** 2026-03-18
**Context:** While building the BlazorTemplate v4 implementation guide, it became clear that authentication setup requires too many manual steps. These should be encapsulated in the Tharga.Blazor package.

## Goal

Add two extension methods to Tharga.Blazor so that consuming projects can enable Azure AD (CIAM) authentication with minimal boilerplate:

```csharp
builder.AddThargaAuth();   // registers auth services
app.UseThargaAuth();       // maps /login and /logout endpoints
```

## What to implement

### `AddThargaAuth(this WebApplicationBuilder builder)`
- Register authentication with Cookie default scheme + OIDC challenge scheme
- Call `AddMicrosoftIdentityWebApp` with `AzureAd` config section
- Register authorization, cascading auth state, and HttpContextAccessor
- Package dependency: `Microsoft.Identity.Web`

### `UseThargaAuth(this WebApplication app)`
- Map `/login` endpoint (OIDC challenge, redirect to `/`)
- Map `/logout` endpoint (sign out of OIDC + cookies, redirect to `/`)

### Also consider
- Register `AddBlazoredLocalStorage()` inside `AddThargaBlazor()` rather than `AddThargaAuth()`, since it is a general Tharga.Blazor dependency (used by `ExpandableCard` and others)

## Open questions (decide during implementation)

### Package placement
Adding `Microsoft.Identity.Web` as a dependency to `Tharga.Blazor` means every consumer gets it, even without auth. Consider whether this should live in a separate package (e.g. `Tharga.Blazor.Auth`) to keep the base package lightweight.

### Configuration requirements
The consuming project must provide an `AzureAd` section in `appsettings.json` with these keys:
- `Authority` — e.g. `https://<tenant-id>.ciamlogin.com/<domain>` or `https://login.microsoftonline.com/<tenant-id>/v2.0`
- `ClientId`
- `TenantId`
- `CallbackPath` — e.g. `/signin-oidc`
- `ValidateAuthority` (optional, defaults to true)

Consider validating that the config section exists and throwing a clear error if missing.

### Relationship to existing UI components
`LoginDisplay` (in `Tharga.Blazor.Features.Authentication`) and `UserProfileView` (in `Tharga.Blazor.Features.User`) already exist in the package. These are the UI counterparts to the new registration helpers. The consuming project uses them via `@using Tharga.Blazor.Features.Authentication` in `_Imports.razor`. Ensure the registration helpers and these components are documented as a cohesive feature.

### Endpoint configurability
Should `/login` and `/logout` paths be hardcoded or configurable via an options object? Hardcoded is simpler and matches the current convention, but an options pattern (e.g. `builder.AddThargaAuth(o => o.LoginPath = "/sign-in")`) would be more flexible.

## Tests

- Verify `AddThargaAuth()` registers expected services (authentication, authorization, cascading auth state, HttpContextAccessor)
- Verify `UseThargaAuth()` maps `/login` and `/logout` endpoints
- Verify default scheme is Cookie and challenge scheme is OIDC

## Update documentation

- Update the Tharga.Blazor README.md with usage instructions for `AddThargaAuth` / `UseThargaAuth`
- Update the Tharga Implementation Guide (BlazorTemplate `modules.md`) to reference these helpers instead of manual setup
