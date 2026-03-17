# Tharga Platform

A suite of NuGet packages for building multi-tenant Blazor applications with team management, authorization, and API infrastructure.

## Packages

| Package | Description | WASM-safe |
|---------|-------------|-----------|
| [Tharga.Team](https://www.nuget.org/packages/Tharga.Team) | Domain models, authorization primitives, service abstractions | Yes |
| [Tharga.Blazor](https://www.nuget.org/packages/Tharga.Blazor) | Generic Blazor UI components (buttons, breadcrumbs, etc.) | Yes |
| [Tharga.Team.Blazor](https://www.nuget.org/packages/Tharga.Team.Blazor) | Team management Blazor components | Yes |
| [Tharga.Team.MongoDB](https://www.nuget.org/packages/Tharga.Team.MongoDB) | MongoDB persistence for teams and users | No |
| [Tharga.Team.Service](https://www.nuget.org/packages/Tharga.Team.Service) | Server-side API key auth, Swagger, audit logging | No |

## Dependency graph

```
Tharga.Team                          (plain .NET)
     |           |            |
Tharga.Blazor  Tharga.Team.MongoDB  Tharga.Team.Service
(generic UI)   (+ Tharga.MongoDB)   (+ Tharga.MongoDB, ASP.NET Core)
     |
Tharga.Team.Blazor
(+ Tharga.Team, Tharga.Team.Service)
```

## Getting started

### WASM project (client-side only)

```csharp
dotnet add package Tharga.Team.Blazor
```

### Server project (full stack)

```csharp
dotnet add package Tharga.Team.Blazor
dotnet add package Tharga.Team.MongoDB
dotnet add package Tharga.Team.Service
```

### Register services

```csharp
builder.Services.AddThargaBlazor(o =>
{
    o.Title = "My App";
    o.RegisterTeamService<MyTeamService, MyUserService>();
});
```

## Links

- [Report an issue](https://github.com/Tharga/Platform/issues)
