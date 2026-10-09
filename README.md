# Fell & Sell — Korean patch

A MelonLoader language mod for the Steam version of **Fell & Sell**.

[![CI](https://github.com/myso-kr/fell-and-sell-mod/actions/workflows/ci.yml/badge.svg)](https://github.com/myso-kr/fell-and-sell-mod/actions/workflows/ci.yml)
[![License](https://img.shields.io/badge/code-MIT-blue)](LICENSE)

**[한국어 안내](docs/ko/README.md)** · [Documentation](docs/README.md) ·
[Releases](https://github.com/myso-kr/fell-and-sell-mod/releases) · [Changelog](CHANGELOG.md)

A bilingual GitHub Pages site is configured under `docs/`; see
[Pages setup](docs/PAGES.md) for publication and local preview.

**v0.3.0 covers all 1,243 extracted localization entries**: 1,128 Game entries
and 115 UI entries. Items, crafting recipes, effects, tutorials, dialogue and
quests are translated. English provides the meaning; Japanese provides additional
context. Proper names and punctuation-only entries are intentionally retained.

Tested on Windows 11, game **1.7.1 / Steam build 25480096**, Unity 6000.3.10f1
(IL2CPP), and MelonLoader **0.7.3 x64**. Runtime checks confirm translation hooks,
1,243 loaded entries, 699 supported Hangul glyphs and 20 Korean TMP components.
Every gameplay screen's layout has not yet been visually reviewed. Text outside
the extracted tables and future game updates may require additional work.

This is an unofficial fan project by myso-kr, not endorsed by the game developer
or publisher. The game is required separately. See [NOTICE](NOTICE).

## Install

1. In Steam, open **Fell & Sell → Properties → Installed Files → Browse**.
   Use the directory containing `Fell & Sell.exe`.
2. Install [MelonLoader 0.7.3 x64](https://github.com/LavaGang/MelonLoader/releases/tag/v0.7.3)
   into that directory, following the loader's instructions. It is not bundled.
3. Download `fell-and-sell-mod-v0.3.0.zip` from the project's
   [Releases](https://github.com/myso-kr/fell-and-sell-mod/releases), when published,
   and extract it over the game directory. The repository source ZIP is not an
   installable mod. If no packaged release exists, follow [the build guide](docs/DEVELOPMENT.md).
4. Start the game **through Steam**. The Korean overlay applies automatically
   to matching localization entries, including when English or Japanese is selected.
   There is no separate Korean option in the game's language menu.

```text
Fell & Sell/
├─ Fell & Sell.exe
├─ MelonLoader/                         installed separately
├─ Mods/FellAndSellMod.dll
└─ UserData/FellAndSell/
   ├─ locale/ko/strings.json
   └─ fonts/
      ├─ NotoSansCJKkr-Regular.otf
      └─ OFL-Noto.txt
```

The archive also carries documentation, notices and license texts. Keep font
license notices with redistributed copies. For checksum verification and a
complete update/removal guide, see [installation](docs/INSTALLATION.md).

## Update or remove

Close the game before replacing the mod DLL, catalog and font with a newer package.
To remove the Korean patch, delete `Mods/FellAndSellMod.dll` and
`UserData/FellAndSell/`. Keep files belonging to other mods. The patch adds files
and changes localization tables in memory; it does not patch the game's original
binaries or implement save editing. Uninstall MelonLoader separately if desired.

## Troubleshooting and feedback

Check `MelonLoader/Latest.log` for `Fell & Sell Korean Patch v0.3.0`,
`i18n: loaded 1243 Korean entries`, installed string hooks, and
`font: verified 699 Hangul glyphs; missing=0`.

See [troubleshooting](docs/TROUBLESHOOTING.md) for missing text, square glyphs,
startup problems and reports after game updates. Report awkward translations,
clipping and crashes through [Issues](https://github.com/myso-kr/fell-and-sell-mod/issues).
Include the game/mod versions, affected screen and relevant log excerpts.
Remove personal paths and credentials before sharing logs.

## Build and contribute

Build with .NET SDK 8 targeting the loader's `net6.0` runtime. Python 3.12 runs
the validator tests; PowerShell 7 runs the build tools.

```powershell
dotnet build FellAndSell.Mod.sln -c Release
python -m unittest discover -s tests
pwsh -NoProfile -File tools/package.ps1
```

The resulting archive and SHA-256 file are under `dist/`. Game files are not
needed for compilation or fixture tests. Source extraction and full translation
coverage checks require a local game installation; see
[development](docs/DEVELOPMENT.md) and [CONTRIBUTING](CONTRIBUTING.md).

## How it works

Harmony hooks intercept Unity.Localization entries by table name and numeric ID,
then replace their templates before formatting. A dynamic **Noto Sans CJK KR**
font provides Hangul through TextMeshPro fallback assets. It registers again as
fonts appear after scene loads. Unknown IDs retain the selected base language.

[Runtime anchors](docs/ANCHORS.md) describe the game APIs this depends on.
[Translation decisions](docs/TRANSLATION.md) record the glossary and source
inconsistencies. There is no translation toggle or font-size setting in this version.

## License

Project code and authored translation contributions are provided under
[MIT](LICENSE), subject to the rights in the underlying game material.
Noto Sans CJK KR is distributed unchanged under the **SIL Open Font License 1.1**,
with its [license text](locale/fonts/OFL-Noto.txt). Game names and original content
remain with their owners. MelonLoader and HarmonyX are installed separately;
game binaries and generated interop assemblies are not distributed.
See [third-party components](THIRD-PARTY.md).
