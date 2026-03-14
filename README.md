# Tharga Blazor
[![NuGet](https://img.shields.io/nuget/v/Tharga.Blazor)](https://www.nuget.org/packages/Tharga.Blazor)
![Nuget](https://img.shields.io/nuget/dt/Tharga.Blazor)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![GitHub repo Issues](https://img.shields.io/github/issues/Tharga/Blazor?style=flat&logo=github&logoColor=red&label=Issues)](https://github.com/Tharga/Blazor/issues?q=is%3Aopen)

A component and service library for building multi-tenant Blazor applications. It provides reusable UI components, team management, and common infrastructure so you can focus on your application logic.

## Packages

### Tharga.Blazor

Razor component library (targets .NET 9.0 and 10.0) built on [Radzen.Blazor](https://blazor.radzen.com/). Key features:

- **Breadcrumb navigation** - Automatic breadcrumb generation via `BreadCrumbService` with support for virtual segments and URL query parameter-driven breadcrumbs.
- **Team management UI** - Ready-to-use components for selecting, creating, and managing teams and members (`TeamSelector`, `TeamComponent`, invite dialogs).
- **API key management** - `ApiKeyView` component for administering API keys scoped to teams (uses `IApiKeyAdministrationService` from [Tharga.Api](https://www.nuget.org/packages/Tharga.Api)).
- **Claims transformation** - `ClaimsTransformation` adds `TeamKey`, `AccessLevel`, and role claims (`TeamMember`, `TeamOwner`, etc.) from the selected team cookie and membership data.
- **Reusable buttons** - `ActionButton`, `CopyButton`, `CancelButton`, and `StandardButton` with built-in busy states, variants, and error handling.
- **UI utilities** - `CustomErrorBoundary`, `ExpandableCard`, `DateTimeView`, `TimeSpanView`, `Loading`, `LoginDisplay`, and more.

### Tharga.Team

Base class library providing the domain models and service abstractions used by Tharga.Blazor:

- **Service interfaces** - `ITeamService` (CRUD, members, invitations) and `IUserService` (current user resolution).
- **Data models** - `ITeam`, `ITeamMember`, `IUser`.
- **Base classes** - `TeamServiceBase` and `UserServiceBase` for implementing your own backend.
- **Roles** - `Roles.TeamMember` (assigned to any team member) and `Roles.Developer`.

> **Note:** The `AccessLevel` enum (Owner, Administrator, User, Viewer) has moved to [Tharga.Api](https://www.nuget.org/packages/Tharga.Api). Tharga.Team depends on Tharga.Api.

## Related packages

- [Tharga.Api](https://www.nuget.org/packages/Tharga.Api) - API-key authentication, `AccessLevel` enum, `[RequireAccessLevel]` attribute, and controller/Swagger setup. Tharga.Blazor and Tharga.Team depend on this package.

## Getting started

Register the services in your `Program.cs`:

```csharp
builder.Services.AddThargaBlazor(o =>
{
    o.Title = "My App";
    o.RegisterTeamService<MyTeamService, MyUserService>();
});
```

For API key authentication and access level enforcement:

```csharp
builder.Services.AddThargaControllers();
builder.Services.AddAuthentication()
    .AddThargaApiKeyAuthentication();
builder.Services.AddThargaApiKeys();
```

Protect pages with role-based authorization:

```razor
@using Tharga.Blazor.Framework
@attribute [Authorize(Roles = Roles.TeamMember)]
```

The `ClaimsTransformation` automatically populates `TeamKey` and `AccessLevel` claims from the selected team, enabling service-level authorization via `[RequireAccessLevel]` from Tharga.Api.
