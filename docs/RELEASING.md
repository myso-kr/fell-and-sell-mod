---
title: "Maintainer release procedure"
lang: en
---

# Maintainer release procedure

The repository and Pages site are public. Mod CI builds and uploads an installable
package; a version-tag Release workflow prepares a **draft prerelease** for review.
The draft is not a public download until a maintainer publishes it.

## Shared build pipeline

`.github/workflows/build.yml` is called by CI and Release. On a Windows runner it:

1. Checks Directory.Build.props, MelonInfo, newest versioned changelog heading,
   public version/count claims and optional release-tag agreement.
2. Checks committed catalog/glyph counts against `release-metadata.json`, required
   notices and exclusion of tracked game/extraction artifacts.
3. Runs validator fixtures, JSON/PowerShell checks, DLL build and ZIP packaging.
4. Verifies catalog/font payloads, redistribution notices and ZIP SHA-256.
5. Generates notes from committed metadata and the versioned changelog section.
6. Uploads ZIP, `.zip.sha256` and notes as `fell-and-sell-mod-vX.Y.Z` for 30 days.

This needs no game installation. Source-token/coverage checking and game runtime
anchors still need the local extraction and retail game. CI cannot establish
all-screen layout correctness.

## Prepare a version

Update Directory.Build.props and Plugin.cs together, add a versioned changelog
entry and update public version/coverage claims. `release-metadata.json` holds the
verified game/build, loader, table counts and glyph count. Change those values only
with actual local evidence. Keep known limitations visible in both languages.

```powershell
python tools/check-repository.py --tag v0.3.0
python -m unittest discover -s tests
python tools/check-translations.py --require-complete
pwsh -NoProfile -File tools/package.ps1
python tools/check-package.py
python tools/release-notes.py --output generated/release-notes.md
```

Review repository history for credentials, game source tables/binaries and
unlicensed assets. Confirm the font and OFL notice stay together. Inspect gameplay,
formatted text, scene fonts and representative layouts, recording unperformed
checks explicitly. Review the generated notes and package before tagging.

## Build a release draft

Push an existing version tag after the reviewed commit is on main:

```powershell
git tag v0.3.0
git push origin v0.3.0
```

Or rerun the Release workflow for an existing tag using gh:

```powershell
gh workflow run release.yml -f tag=v0.3.0
gh run list --workflow release.yml --limit 5
```

Manual dispatch validates the tag and checks out that tag's code, not arbitrary
main-branch code. The shared build rechecks version agreement. The draft job has
contents-write permission; the build has contents-read permission. An existing
draft may be refreshed; a published release is never overwritten by this workflow.
No game or loader binaries are attached.

## Review and publish

Open the draft in [Releases](https://github.com/myso-kr/fell-and-sell-mod/releases),
verify notes and attached ZIP/hash, download and validate the package, and record
the commit/tag and actual game checks. Keep the prerelease marker while the broad
visual/gameplay review is incomplete. Publishing is a separate maintainer decision.

After publication, verify anonymous downloads, the checksum and the installation
links. Update public docs if a download path or supported build changes. The
[download guide](DOWNLOADS.md) explains temporary CI artifacts and public assets.
Pages continues to publish from `main /docs`; [site validation](PAGES.md) is separate
from the mod build and release pipeline.
