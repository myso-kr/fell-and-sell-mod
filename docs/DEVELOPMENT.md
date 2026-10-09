---
title: "Development"
lang: en
---

# Development

## Toolchain

Use Windows, .NET SDK 8, PowerShell 7 and Python 3.12. The mod targets net6.0 for
MelonLoader 0.7.3 IL2CPP. NuGet downloads MelonLoader 0.7.3 and HarmonyX 2.10.2
as compilation references; loader libraries are not included in the package.
Runtime Unity and game types are resolved from loader-generated interop assemblies.

```powershell
git clone https://github.com/myso-kr/fell-and-sell-mod.git
cd fell-and-sell-mod
dotnet build FellAndSell.Mod.sln -c Release
python -m unittest discover -s tests
pwsh -NoProfile -File tools/verify.ps1
pwsh -NoProfile -File tools/package.ps1
```

`dist/fell-and-sell-mod-v0.3.0.zip` and its `.sha256` contain the installable package.
The version comes from Directory.Build.props; the MelonInfo version in Plugin.cs
must be kept in sync. Builds and fixture tests need no game installation.

## Extract translation context locally

Install the game separately and run from the repository root:

```powershell
python -m venv .venv
.\.venv\Scripts\python.exe -m pip install -r tools/requirements.txt
.\.venv\Scripts\python.exe tools/extract-strings.py --game-dir 'C:\Program Files (x86)\Steam\steamapps\common\Fell & Sell'
.\.venv\Scripts\python.exe tools/check-translations.py --require-complete
```

The extractor reads English, Japanese and shared-key bundles with UnityPy.
`generated/strings.en.json` joins English and Japanese by table/entry ID;
missing Japanese/shared keys do not discard English entries. Generated context
is ignored and must not be published. See [the survey](GAME-SURVEY.md).

The checker rejects unknown keys, empty/non-string values, changed formatting
tokens and, with `--require-complete`, missing source entries. Numeric meaning and
translation quality still require review. A new game extraction may change coverage.

## Deploy and verify

Close the game, install MelonLoader separately, then run:

```powershell
pwsh -NoProfile -File tools/deploy.ps1 -GameDir 'C:\Program Files (x86)\Steam\steamapps\common\Fell & Sell'
```

The deploy tool refuses to overwrite a running game and copies the built DLL,
catalog and licensed font. Launch through Steam. Inspect Latest.log for catalog,
hook and font success, then check menu layout, item descriptions, formatted strings,
scene changes and language switching in play. Record actual results and game build;
do not infer visual correctness from logs alone.

## CI

GitHub Actions builds on windows-latest with .NET SDK 8 and Python 3.12, runs
repository/version/document consistency checks and validator fixtures, builds the
DLL, verifies packaged payloads and checksum, then uploads ZIP, SHA-256 and release
notes for 30 days. CI and Release share `.github/workflows/build.yml`.

The CI workflow runs on main pushes, PRs and manual dispatch. Release runs for
`v*` tags or a manually selected existing tag, validates the version and creates
a draft prerelease with the same package. It does not overwrite published releases.
[Downloads](DOWNLOADS.md) explains artifact retrieval.

Source tables are deliberately absent,
so full extracted-source coverage is a local maintainer check. CI does not install
or run the game. See [release procedure](RELEASING.md).
