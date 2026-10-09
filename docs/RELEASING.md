---
title: "Maintainer release procedure"
lang: en
---

# Maintainer release procedure

This document prepares a reviewable public repository and release. It does not
claim that repository visibility, GitHub Pages or a release has already been changed.

## Before publication

- Review tracked files and Git history for personal paths, credentials, generated
  source tables, game binaries and unlicensed assets. Ignore rules do not remove
  files already committed. Keep font source, license and notices together.
- Confirm README and Korean guide describe the actual package and known limits.
- Confirm Directory.Build.props and Plugin.cs have the same version, and add an
  accurate changelog entry with the tested game build.
- Run validator fixtures, `tools/verify.ps1`, full source coverage checking against
  a fresh local extraction, and `tools/package.ps1`.
- Verify archive contents and SHA-256; include DLL, catalog, font and OFL text,
  user guides and notices. Exclude loader/game binaries and generated assemblies.
- Test startup, formatted strings, Hangul, scene changes and representative screens.
  Record unperformed checks explicitly instead of marking them passed.

## Repository settings

After the owner authorizes public publication, change repository visibility to
Public and confirm anonymous access. Enable Issues and, if desired, GitHub private
vulnerability reporting. The CI badge will then be readable publicly. These docs
are also configured as a Jekyll site under `docs/`. Enable Pages from `main /docs`
using [the Pages guide](PAGES.md). Do not claim the site is live before deployment
and anonymous access are verified.

## Package release

Create a version tag for the reviewed commit and a GitHub Release with that tag.
Attach `dist/fell-and-sell-mod-vX.Y.Z.zip` and its `.zip.sha256`; source-code archives
alone are not installable. Use the release-note template below and identify preview
builds as prereleases when gameplay/layout review is incomplete. Verify that the
release links and checksum instructions work after publication.

```markdown
## Fell & Sell Korean patch vX.Y.Z

- Changes: [concrete changes from CHANGELOG.md]
- Coverage: [Game/UI counts from the current local extraction]
- Tested: [game version, Steam build, loader version and OS]
- Checks: [tests, build, coverage, startup, glyph and visual results]
- Known limits: [unperformed review and observed issues]

Install MelonLoader separately, then extract the attached mod ZIP into the folder
containing Fell & Sell.exe. Launch through Steam. See README and docs/INSTALLATION.md.
The SHA-256 checksum is attached alongside the ZIP. This is an unofficial fan patch.
```

Do not claim broader game compatibility or completed visual review from the CI
result. Check [the support guide](TROUBLESHOOTING.md) and license notices before releasing.
