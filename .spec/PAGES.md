# GitHub Pages maintenance

The public site follows the sibling repositories: static Jekyll under `docs/`,
paper/forest-green styling, system fonts and no JavaScript or remote theme.
Published at https://myso-kr.github.io/fell-and-sell-mod/.
Pages deploys from **main /docs**; the Documentation workflow validates rather
than deploying. CI artifacts, draft releases and Pages deployments are separate.

## Canonical routes

Each route below has a Korean counterpart prefixed with `/ko`:

| English | Purpose |
|---|---|
| `/` | Introduction and preview entry |
| `/guide/` | Player guide directory |
| `/downloads/` | Successful main build, artifact and inner package instructions |
| `/installation/` | Requirements, install, optional checksum, update and removal |
| `/features/` | Controls, map, fairy guide, pickup, movement and availability |
| `/help/` | Symptom navigation, checks and optional logs |
| `/glossary/` | Translation coverage, terms and feedback |

`/404.html` offers English and Korean recovery links. Legacy `/DOWNLOAD.html`,
`/DOWNLOADS.html`, `/INSTALLATION.html`, `/TROUBLESHOOTING.html` and
`/TRANSLATION.html` redirect to the corresponding lowercase English route,
with an explicit fallback link and canonical metadata.

## Source ownership

- `_data/pages.json`: paired routes keyed by page identity. Main navigation and
  language switching use these pairs; switching language preserves the current guide.
- `_data/patch.json`: public compatibility, available preview URLs, translation
  count and scoped feature availability. `check-repository.py` compares it with
  version/catalog and `.spec/release-metadata.json`; raw internal evidence is not published.
- `_data/ui.json`: shared English/Korean layout labels.
- `_layouts/default.html`: one H1, article breadcrumb, responsive main navigation,
  current-page state, paired hreflang/canonical metadata and common footer.
- `_includes/feature-status.html`: shared home/feature availability. Automatic
  movement remains preview; broader map/enemy coverage is not claimed fully verified.
- Markdown: player-only copy, H2/H3 body headings, explicit metadata including
  `page_id` and `page_type`. Long guides use anchor navigation. CLI and logs sit
  in keyboard-operable native disclosures.
- CSS: readable source, responsive layouts, visible focus, light/dark tokens,
  Korean word grouping and bounded code blocks. No packaged game font is fetched.

Use `relative_url` for project navigation/assets and `absolute_url` for metadata.
Source filenames may remain uppercase for repository links; canonical web routes
are lowercase folders. The package includes `_data`, `_includes` and `aliases`
alongside layout, CSS and guides. Technical records and build caches stay out.

## Local checks

```powershell
bundle install --gemfile tools/pages/Gemfile
bundle exec --gemfile tools/pages/Gemfile jekyll build --source docs --destination generated/site
python tools/check-repository.py
python tools/check-site.py generated/site
python -m pip install -r tools/requirements-site.txt
python -m playwright install chromium
python tools/check-site-browser.py generated/site --screenshots generated/pages-preview
```

Use `--channel msedge` with an installed Edge browser instead of downloading
Chromium locally. The browser tool serves the rendered files through an isolated
request fixture and never follows external download links. CI installs pinned
Playwright and Chromium on Ubuntu. Keep generated screenshots ignored.

Ruby development dependencies and their lockfile live under `tools/pages`, outside
the published source. Jekyll 3.10.0, relative-links 0.6.1, GFM parser 1.1.0 and
kramdown 2.4.0 match the [GitHub Pages dependency list](https://pages.github.com/versions/).
The Pages action uses its bundled runtime rather than this Windows lockfile. Keeping
the local Gemfile in the source previously caused `bundle check` to warn about
local transitive gems unavailable in the Pages image; the build still succeeded.

The static checker covers all 20 output pages, fragments/assets, exact canonical
routes, heading hierarchy, seven locale pairs, unique menu destinations, current
states, compatibility/status, preview action and five legacy redirects. The browser
checker covers 15 canonical pages at 320/390/1280px in light/dark mode (90 views),
first-screen download action, skip/main focus, keyboard disclosures, contextual
language switching, two locale user journeys and five actual redirects.

A successful check is not an accessibility certification or exhaustive cross-browser
coverage. Edge is the local browser; bundled Chromium is used in Documentation CI.

## Download and deployment verification

Currently no public release exists. The site links to successful main CI runs;
GitHub sign-in is required for artifacts, retained for 30 days. The outer artifact
contains the installable mod ZIP. Never silently publish an existing draft release
as part of site upkeep. When publishing a release intentionally, change the public
distribution wording and download action in both languages together.

After pushing, inspect CI, Documentation and pages-build-deployment at the exact
head SHA. Check live canonical/legacy URLs and one locale pair's navigation.

```powershell
gh run list --commit <sha>
gh api repos/myso-kr/fell-and-sell-mod/pages/builds/latest
gh api repos/myso-kr/fell-and-sell-mod/actions/runs/<successful-ci-id>/artifacts
```

The original audit is [PAGES-AUDIT.md](PAGES-AUDIT.md); the redesign implementation
and deployment evidence are [PAGES-IMPROVEMENT.md](PAGES-IMPROVEMENT.md).
