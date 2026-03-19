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
Tharga.Team ── plain .NET, no external dependencies
├── Tharga.Blazor ── generic Blazor UI components
│   └── Tharga.Team.Blazor ── team management UI
│       └── + Tharga.Team.Service
├── Tharga.Team.MongoDB ── persistence layer
│   └── + Tharga.MongoDB
└── Tharga.Team.Service ── server-side API + auth
    └── + Tharga.MongoDB, ASP.NET Core
```

## Getting started

See the **[Implementation Guide](docs/implementation-guide.md)** for step-by-step instructions on adding each feature to your Blazor application. Each step covers packages, Program.cs changes, _Imports.razor, configuration, and what becomes available.

### Quick start

```
dotnet add package Tharga.Blazor              # Step 1: UI components
dotnet add package Tharga.Team.Blazor         # Step 2: Authentication + Step 4: Team management UI
dotnet add package Tharga.Team.Service        # Step 3: API controllers + Step 5-8: Server features
dotnet add package Tharga.Team.MongoDB        # Step 4: Team persistence
```

## Links

- [Implementation Guide](docs/implementation-guide.md)
- [Report an issue](https://github.com/Tharga/Platform/issues)
