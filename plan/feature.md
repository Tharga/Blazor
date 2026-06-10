# Feature: Package icon + docs site + integrated docs CI

## Goal
Bundle three closely-related project-setup polish items into one feature, mirroring the **Tharga.Test** commit `d6b9efb` ("feat: package icon, docs site, and integrated docs CI"):

1. Move `<PackageIconUrl>` from the legacy WordPress URL (`thargelion.se/wp-content/uploads/...`) to the canonical `https://thargelion.net/assets/component-blazor.png`.
2. Publish a DocFX docs site at `blazor.tharga.net` covering the reusable components shipped by Tharga.Blazor.
3. Fold docs build + GitHub Pages deploy into `.github/workflows/build.yml` as `docs` / `docs-deploy` jobs gated on `needs: release`, per the "integrated pattern from day one" guidance.

## Scope
- Update `Tharga.Blazor/Tharga.Blazor.csproj` `<PackageIconUrl>`.
- Add `docs/` tree with DocFX setup:
  - `docs/CNAME` → `blazor.tharga.net`
  - `docs/docfx.json` (modeled on Tharga.Test, project-specific names)
  - `docs/index.md` (landing page)
  - `docs/toc.yml`
  - `docs/articles/index.md`, `articles/toc.yml`
  - `docs/articles/getting-started.md`
  - `docs/articles/buttons.md` (covers `StandardButton` + `CopyButton` including the new `Size` parameter)
  - `docs/articles/breadcrumbs.md`
  - `docs/templates/thg/public/main.css` (navbar logo sizing)
- Add `pages: write` + `id-token: write` to workflow-level `permissions` in `build.yml`.
- Append `docs` job (`needs: release`) and `docs-deploy` job (`needs: docs`) to `build.yml`.
- Update `.gitignore` to exclude `docs/_site/` and DocFX-generated `docs/api/`.
- Update `README.md` to link to `blazor.tharga.net`.

## Out of scope
- Wider workflow-template fixes beyond the docs jobs.
- README rewrite — only add the docs-site link.
- Re-targeting the package icon for other sister projects (each has its own pending request).

## Acceptance criteria
- `dotnet build -c Release` succeeds with 0 warnings; `dotnet test -c Release` passes (no functional change expected).
- `docfx docs/docfx.json` builds locally without errors.
- Workflow YAML validates (`gh workflow view` or merge to PR triggers the new jobs).
- `<PackageIconUrl>` resolves to a 200 PNG (verifiable via curl).
- `blazor.tharga.net` configured in DNS to GitHub Pages **before merging** — flagged for user confirmation.

## Done condition
- All acceptance criteria met.
- README updated.
- PR opened from `feature/icon-and-docs` → `master` (per the GitHub Actions branching strategy now declared in `mission.md`).
- Two corresponding pending requests in `Requests.md` flipped to Done with version reference once the PR is merged:
  - **Move PackageIconUrl to thargelion.net/assets** → Tharga.Blazor row
  - **Documentation sites under tharga.net** → Tharga.Blazor row
