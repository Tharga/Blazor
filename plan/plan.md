# Plan: feature/icon-and-docs

Mirrors Tharga.Test commit `d6b9efb`.

## Steps

- [x] 1. Update `Tharga.Blazor/Tharga.Blazor.csproj` `<PackageIconUrl>` to `https://thargelion.net/assets/component-blazor.png`
- [x] 2. Add `docs/CNAME` containing `blazor.tharga.net`
- [x] 3. Add `docs/docfx.json` (mirrors Tharga.Test layout — adjust `src` and `globalMetadata`)
- [x] 4. Add `docs/index.md` (landing page)
- [x] 5. Add `docs/toc.yml` (Home / Articles / API)
- [x] 6. Add `docs/articles/index.md`, `docs/articles/toc.yml`
- [x] 7. Add `docs/articles/getting-started.md`
- [x] 8. Add `docs/articles/buttons.md` (StandardButton + CopyButton.Size)
- [x] 9. Add `docs/articles/breadcrumbs.md`
- [x] 10. Add `docs/templates/thg/public/main.css` for logo sizing + `docs/templates/thg/layout/_master.tmpl` for the absolute-URL logo trap fix (option b, mirroring Tharga.Test)
- [x] 11. Update `.gitignore` — add `docs/_site/`, `docs/api/`, `docs/obj/`
- [x] 12. Update `.github/workflows/build.yml` — add `pages: write` + `id-token: write` permissions
- [x] 13. Update `build.yml` — add `docs` job (`needs: release`)
- [x] 14. Update `build.yml` — add `docs-deploy` job (`needs: docs`)
- [x] 15. Update `README.md` — add link to `blazor.tharga.net`
- [x] 16. Commit settings.json consolidation (separate commit on this branch)
- [x] 17. Build + test locally; verify clean (0 warnings, 24/24 tests, `docfx docs/docfx.json` builds with 0 errors)
- [x] 18. Push branch + open PR `feature/icon-and-docs` → `master` — PR [#7](https://github.com/Tharga/Blazor/pull/7)

## Last session
All implementation steps done. Feature branch pushed and PR #7 opened against master. Waiting on user confirmation + DNS setup (`blazor.tharga.net` → `tharga.github.io`) before closing the feature.
