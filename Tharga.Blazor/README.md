# Tharga Blazor

[![GitHub repo](https://img.shields.io/github/repo-size/Tharga/Blazor?style=flat&logo=github&logoColor=red&label=Repo)](https://github.com/Tharga/Blazor)

Razor component library for multi-tenant Blazor applications. Works with both **Blazor Server** and **Blazor WebAssembly**.

## Key features

- **Team management** - TeamSelector, TeamComponent, invite dialogs
- **API key management** - ApiKeyView for team-scoped API keys
- **Claims augmentation** - `TeamClaimsAuthenticationStateProvider` adds `TeamKey`, `AccessLevel`, role, and scope claims from team membership. Compatible with all Blazor hosting models.
- **Roles** - `Roles.TeamMember` (any team member), `Roles.Developer`
- **UI components** - ActionButton, CopyButton, BreadCrumbs, CustomErrorBoundary, Loading, and more

## Hosting model support

| Hosting model | Supported |
|---------------|-----------|
| Blazor Server | Yes |
| Blazor WebAssembly | Yes |
| Blazor Web App (auto/hybrid) | Yes |

## Links

- [GitHub repository](https://github.com/Tharga/Blazor)
- [Report an issue](https://github.com/Tharga/Blazor/issues)
