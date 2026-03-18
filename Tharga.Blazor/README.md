# Tharga Blazor
[![NuGet](https://img.shields.io/nuget/v/Tharga.Blazor)](https://www.nuget.org/packages/Tharga.Blazor)
![Nuget](https://img.shields.io/nuget/dt/Tharga.Blazor)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

Generic reusable Blazor UI components. Works with both **Blazor Server** and **Blazor WebAssembly**. Built on [Radzen.Blazor](https://blazor.radzen.com/).

## Authentication

Built-in Azure AD (CIAM) authentication helpers. Two calls to set up Cookie + OIDC authentication:

```csharp
// Program.cs
builder.AddThargaAuth();   // registers auth services
app.UseThargaAuth();       // maps /login and /logout endpoints
```

### Configuration

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

### Options

Customize behavior via `ThargaAuthOptions`:

```csharp
builder.AddThargaAuth(o =>
{
    o.LoginPath = "/sign-in";              // default: "/login"
    o.LogoutPath = "/sign-out";            // default: "/logout"
    o.ValidateConfiguration = false;       // default: true — validates AzureAd config at startup
});
```

### UI components

Use the existing `LoginDisplay` component (from `Tharga.Team.Blazor`) for the login/logout UI, or build your own against the mapped endpoints.

## Components

- **Buttons** - `ActionButton`, `CancelButton`, `CopyButton`, `StandardButton` with built-in busy states and error handling.
- **Breadcrumbs** - `BreadCrumbService` with virtual segments, query parameter segments, and segment relinking.
- **Error handling** - `CustomErrorBoundary` with correlation ID logging.
- **Layout** - `ExpandableCard`, `Loading`, `Title`, `DateTimeView`, `TimeSpanView`.

## Related packages

| Package | Description |
|---------|-------------|
| [Tharga.Team.Blazor](https://www.nuget.org/packages/Tharga.Team.Blazor) | Team management components (depends on this package) |
| [Tharga.Team](https://www.nuget.org/packages/Tharga.Team) | Domain models and authorization primitives |
