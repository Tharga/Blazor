# Tharga Team Blazor
[![NuGet](https://img.shields.io/nuget/v/Tharga.Team.Blazor)](https://www.nuget.org/packages/Tharga.Team.Blazor)
![Nuget](https://img.shields.io/nuget/dt/Tharga.Team.Blazor)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

Team management Blazor components for multi-tenant applications. Works with both **Blazor Server** and **Blazor WebAssembly**.

## Components

- **Team management** - `TeamSelector`, `TeamComponent`, `TeamDialog`, `InviteUserDialog`, `TeamInviteView`.
- **API key management** - `ApiKeyView` for team-scoped API keys.
- **User management** - `UserProfileView`, `UsersView`.
- **Authentication** - `LoginDisplay` with login/logout and team navigation.
- **Claims augmentation** - `TeamClaimsAuthenticationStateProvider` adds `TeamKey`, `AccessLevel`, role, and scope claims. Compatible with all hosting models.
- **Audit** - `AuditLogView` for viewing audit logs with charts and filtering.

## Authentication

Built-in Azure AD (CIAM) authentication helpers. Two calls to set up Cookie + OIDC authentication:

```csharp
// Program.cs
builder.AddThargaAuth();   // registers auth services
app.UseThargaAuth();       // maps /login and /logout endpoints
```

Add an `AzureAd` section to `appsettings.json`:

```json
{
  "AzureAd": {
    "Authority": "https://<tenant>.ciamlogin.com/<domain>",
    "ClientId": "<client-id>",
    "TenantId": "<tenant-id>",
    "CallbackPath": "/signin-oidc"
  }
}
```

Customize via `ThargaAuthOptions`:

```csharp
builder.AddThargaAuth(o =>
{
    o.LoginPath = "/sign-in";              // default: "/login"
    o.LogoutPath = "/sign-out";            // default: "/logout"
    o.ValidateConfiguration = false;       // default: true — validates AzureAd config at startup
});
```

**UI components:**
- `<LoginDisplay />` — profile menu with Gravatar when authenticated, login button when not. Navigates to `/login`, `/logout`, and profile/team pages.
- `<UserProfileView />` — displays the user's Gravatar, profile info, and authentication claims in an expandable card.

## Team management

```csharp
builder.Services.AddThargaTeamBlazor(o =>
{
    o.Title = "My App";
    o.RegisterTeamService<MyTeamService, MyUserService>();
});
```

## Dependencies

- [Tharga.Blazor](https://www.nuget.org/packages/Tharga.Blazor) - Generic UI components.
- [Tharga.Team](https://www.nuget.org/packages/Tharga.Team) - Domain models and authorization primitives.
- [Tharga.Team.Service](https://www.nuget.org/packages/Tharga.Team.Service) - Audit types for AuditLogView.

## Related packages

| Package | Description |
|---------|-------------|
| [Tharga.Team.MongoDB](https://www.nuget.org/packages/Tharga.Team.MongoDB) | MongoDB persistence for teams and users |
| [Tharga.Team.Service](https://www.nuget.org/packages/Tharga.Team.Service) | Server-side API key auth, Swagger, audit logging |
