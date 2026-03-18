# Plan: tharga-auth-registration

## Steps

1. [x] **Add `Microsoft.Identity.Web` dependency** to `Tharga.Blazor` package — added v4.5.0 + FrameworkReference to Microsoft.AspNetCore.App
2. [x] **Create `ThargaAuthOptions`** — LoginPath, LogoutPath, ValidateConfiguration with defaults
3. [x] **Implement `AddThargaAuth()`** — Cookie + OIDC, Microsoft Identity Web, authorization, cascading auth state, HttpContextAccessor, config validation
4. [x] **Implement `UseThargaAuth()`** — maps login/logout endpoints using options
5. [x] **Move `AddBlazoredLocalStorage()`** — already in `AddThargaBlazor()` in Tharga.Team.Blazor, no change needed
6. [x] **Write tests** — 13 tests covering service registration, endpoint mapping, config validation, default/custom paths
7. [x] **Update README** — added Authentication section with usage, config, and options docs
8. [~] **Final build & test pass, commit**
