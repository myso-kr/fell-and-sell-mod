# Fell & Sell — Korean patch

A MelonLoader language mod for the Steam version of **Fell & Sell**.

[![CI](https://github.com/myso-kr/fell-and-sell-mod/actions/workflows/ci.yml/badge.svg)](https://github.com/myso-kr/fell-and-sell-mod/actions/workflows/ci.yml)
[![License](https://img.shields.io/badge/code-MIT-blue)](LICENSE)

**[한국어 안내](docs/ko/README.md)** · [Documentation](docs/README.md) ·
[Releases](https://github.com/myso-kr/fell-and-sell-mod/releases) · [Changelog](CHANGELOG.md)

**[Website](https://myso-kr.github.io/fell-and-sell-mod/)** ·
**[한국어 웹사이트](https://myso-kr.github.io/fell-and-sell-mod/ko/)**


**v0.4.0 covers all 1,243 extracted localization entries**: 1,128 Game entries
and 115 UI entries. Items, crafting recipes, effects, tutorials, dialogue and
quests are translated. English provides the meaning; Japanese provides additional
context. Proper names and punctuation-only entries are intentionally retained.

Tested on Windows 11, game **1.7.1 / Steam build 25480096**, Unity 6000.3.10f1
(IL2CPP), and MelonLoader **0.7.3 x64**. Runtime checks confirm translation hooks,
1,243 loaded entries, 699 supported Hangul glyphs and 20 Korean TMP components.
The project owner confirmed completion of in-game text display review on
2026-10-09. Broader gameplay, Japanese switching and uninstall tests are separate
checks and remain unrecorded. Text outside
the extracted tables and future game updates may require additional work.

This is an unofficial fan project by myso-kr, not endorsed by the game developer
or publisher. The game is required separately. See [NOTICE](NOTICE).

## Install

1. In Steam, open **Fell & Sell → Properties → Installed Files → Browse**.
   Use the directory containing `Fell & Sell.exe`.
2. Install [MelonLoader 0.7.3 x64](https://github.com/LavaGang/MelonLoader/releases/tag/v0.7.3)
   into that directory, following the loader's instructions. It is not bundled.
3. Download `fell-and-sell-mod-v0.4.0.zip` from the project's
   [Releases](https://github.com/myso-kr/fell-and-sell-mod/releases), when published,
   and extract it over the game directory. The repository source ZIP is not an
   installable mod. Before a public release exists, download the ZIP from a successful
   [CI build](https://github.com/myso-kr/fell-and-sell-mod/actions/workflows/ci.yml)
   using [the download guide](docs/DOWNLOADS.md).
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

Check `MelonLoader/Latest.log` for `Fell & Sell Korean Patch v0.4.0`,
`i18n: loaded 1243 Korean entries`, installed string hooks, and
`font: verified 699 Hangul glyphs; missing=0`.

See [troubleshooting](docs/TROUBLESHOOTING.md) for missing text, square glyphs,
startup problems and reports after game updates. Report awkward translations,
clipping and crashes through [Issues](https://github.com/myso-kr/fell-and-sell-mod/issues).
Include the game/mod versions, affected screen and relevant log excerpts.
Remove personal paths and credentials before sharing logs.

## Contributions

Report translation or gameplay issues through [Issues](https://github.com/myso-kr/fell-and-sell-mod/issues). See [CONTRIBUTING](CONTRIBUTING.md) for contribution guidance.

## Exploration helper preview

[v0.4.0 adds an exploration helper](docs/EXPANSION.md): native nearby pickup,
existing-map reveal and markers, next-floor guidance and manually started automatic movement.
New features default off. F8 opens settings, F9 toggles the map and F10 starts/stops movement.
Achievements retain the game's existing behaviour. The owner confirmed map display, destination selection and right-click return to the next-floor route. The moving light and fade were confirmed. Nearby pickup and route/map refresh after changing floors were confirmed. Automatic movement remains under review.

## License

Project code and authored translation contributions are provided under
[MIT](LICENSE), subject to the rights in the underlying game material.
Noto Sans CJK KR is distributed unchanged under the **SIL Open Font License 1.1**,
with its [license text](locale/fonts/OFL-Noto.txt). Game names and original content
remain with their owners. MelonLoader and HarmonyX are installed separately;
game binaries and generated interop assemblies are not distributed.
See [third-party components](THIRD-PARTY.md).
