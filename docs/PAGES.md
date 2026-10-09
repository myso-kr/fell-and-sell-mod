---
title: "GitHub Pages setup"
lang: en
---

# GitHub Pages setup

The site follows the sibling Combolands repository: Jekyll source in `docs/`, a
custom layout/CSS, and English `/` and Korean `/ko/` home pages. No remote theme,
analytics, client framework, generated art or external webfont is required.
Published on 2026-10-09 at [the English home](https://myso-kr.github.io/fell-and-sell-mod/)
and [한국어 홈](https://myso-kr.github.io/fell-and-sell-mod/ko/). The repository is public
and Pages publishes from `main /docs`. Both homes, the documentation directory,
Korean guide, installation page and CSS returned HTTP 200 in deployment checks.

The Gemfile uses the [GitHub Pages dependency versions](https://pages.github.com/versions/)
for Jekyll, relative links and GFM parsing.

## Routes and content

| Route | Purpose |
|---|---|
| `/` | English introduction and installation entry |
| `/ko/` | Korean introduction and installation entry |
| `/guide/` | Documentation directory |
| `/ko/guide/` | Complete Korean user guide |
| `/INSTALLATION.html` | Installation, checksum, updates and uninstall |
| `/TROUBLESHOOTING.html` | Diagnosis and known limits |
| `/DEVELOPMENT.html` | Build, extraction and checks |
| `/404.html` | Recovery links for a missing page |

Other reference Markdown files render to corresponding `.html` paths. Front
matter supplies titles and language. The `jekyll-relative-links` plugin converts
local Markdown links, including permalink targets. Links to documents outside
`docs/` use their repository URLs. Keep `baseurl` set to `/fell-and-sell-mod` and
use `relative_url` for site navigation and assets. Canonical and home-page
hreflang tags are provided by the layout.

## Build locally

Ruby and Bundler are needed only for documentation development, not to use the mod.
From the repository root:

```powershell
bundle install --gemfile docs/Gemfile
bundle exec --gemfile docs/Gemfile jekyll build --source docs --destination generated/site
python tools/check-site.py generated/site
bundle exec --gemfile docs/Gemfile jekyll serve --source docs --destination generated/site
```

The local preview normally uses `http://127.0.0.1:4000/fell-and-sell-mod/`.
Generated HTML, caches and dependencies are ignored. The Documentation workflow
uses GitHub's Jekyll Pages build action and checks rendered links, fragments,
language and canonical metadata on pushes and pull requests. It validates the
site without publishing a pull request preview.

## Enable deployment with gh

The chosen publishing source is **Deploy from a branch → main → /docs**, matching
the sibling repository. GitHub manages the resulting Pages build/deployment.
The separate Documentation workflow is a validation check, not a deploy job.

```powershell
gh api --method POST repos/myso-kr/fell-and-sell-mod/pages -f 'source[branch]=main' -f 'source[path]=/docs'
gh api repos/myso-kr/fell-and-sell-mod/pages
gh api --method POST repos/myso-kr/fell-and-sell-mod/pages/builds
gh api repos/myso-kr/fell-and-sell-mod/pages/builds/latest
```

If Pages already exists, inspect its settings before using PATCH to change the
source. Confirm the deployment status and anonymously open both home pages and a
reference page. A private repository may require a paid plan; an HTTP 422 saying
the plan does not support Pages requires changing the plan or owner-authorized
repository publication. Making the repository public also exposes its Git history.

See [GitHub's publishing-source documentation](https://docs.github.com/en/pages/getting-started-with-github-pages/configuring-a-publishing-source-for-your-github-pages-site).

## Mod builds and site deployment are separate

CI and Release build the mod through the reusable Build package workflow.
Documentation validates the rendered site; GitHub's pages-build-deployment job
publishes `main /docs`. A CI artifact is not a Pages deployment or a public release.
[Downloads](DOWNLOADS.md) and [release procedure](RELEASING.md) describe those paths.

## Design and maintenance

The original CSS uses a paper-colored background, forest-green links and concise
navigation. System fonts support Korean without downloading the bundled 16 MB
game font. Dark mode follows the user's system setting. Visible focus, skip links,
mobile wrapping and scrollable code/table content support keyboard and small-screen
reading. No screenshot is presented as verified gameplay evidence.

Keep versions, coverage and known limits synchronized across both home pages,
README and the Korean guide. Link to the releases list until a real packaged
release exists. For design changes, check both languages at desktop and mobile
widths, dark mode, keyboard navigation and the rendered link checker.
