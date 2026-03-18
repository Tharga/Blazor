# Feature: tharga-auth-registration

**Originating branch:** develop
**Date started:** 2026-03-18

## Goal

Add auth registration helpers to Tharga.Blazor so consuming projects can enable Azure AD (CIAM) authentication with minimal boilerplate via `builder.AddThargaAuth()` and `app.UseThargaAuth()`.

## Scope

- `AddThargaAuth(this WebApplicationBuilder)` — registers Cookie + OIDC auth, Microsoft Identity Web, authorization, cascading auth state, HttpContextAccessor
- `UseThargaAuth(this WebApplication)` — maps configurable login/logout endpoints
- `ThargaAuthOptions` — options class with configurable `LoginPath` (default `/login`), `LogoutPath` (default `/logout`), and `ValidateConfiguration` (default `true`)
- Config validation: verify `AzureAd` section exists at startup (opt-out via options)
- `Microsoft.Identity.Web` added as dependency to `Tharga.Blazor`
- Move `AddBlazoredLocalStorage()` into `AddThargaBlazor()` if not already there

## Acceptance criteria

- [ ] `AddThargaAuth()` registers all expected services
- [ ] `UseThargaAuth()` maps `/login` and `/logout` endpoints by default
- [ ] Login and logout paths are configurable via `ThargaAuthOptions`
- [ ] Config validation throws clear error when `AzureAd` section is missing (default on, opt-out available)
- [ ] `AddBlazoredLocalStorage()` is called from `AddThargaBlazor()`
- [ ] All tests pass
- [ ] README updated with usage instructions

## Done condition

All acceptance criteria met, all tests pass, user confirms feature is complete.
