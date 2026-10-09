# Fell & Sell — Korean patch

MelonLoader-based Korean language patch for the Steam game **Fell & Sell**.

**Status: partial Korean patch, v0.2.0.** 371 of 1243 extracted English entries
are translated: the complete UI table, core controls and initial tutorials,
stats, categories, inventory controls, shop buildings and Dungeon + settings.
English and Japanese are compared by table/entry ID for translation context.
Untranslated entries retain the game's selected base language.

Verified on game 1.7.1 / Steam build 25480096 with MelonLoader 0.7.3:
translation hooks execute, Korean TMP text components are created, and the
dynamic Noto font contains the authored Hangul glyphs. Visual layout, gameplay
screens and the remaining item descriptions and quests still need review.

## Build

Use .NET SDK 8 to build the `net6.0` mod for MelonLoader 0.7.3's IL2CPP runtime:

```powershell
dotnet build FellAndSell.Mod.sln -c Release
pwsh -NoProfile -File tools/verify.ps1
pwsh -NoProfile -File tools/package.ps1
```

The NuGet loader reference is for compilation only; loader binaries are not shipped.
Unity and game APIs are resolved from the installed loader's generated interop
assemblies at runtime; game assemblies are not needed to build the project.

## Development installation

Install [MelonLoader](https://github.com/LavaGang/MelonLoader) separately into the
folder containing `Fell & Sell.exe`. Extract the package over that folder.
It adds `Mods/FellAndSellMod.dll`, `UserData/FellAndSell/locale/ko/strings.json`,
and the licensed font under `UserData/FellAndSell/fonts/`.
Start through Steam. Check `MelonLoader/Latest.log` for `i18n: loaded 371 Korean
entries`, installed string hooks and `font: verified ... missing=0`.
The patch overlays either English or Japanese (or another selected locale);
choose English or Japanese in the game's language options for the untranslated
fallback text. A separate Korean entry in the language menu is not implemented.

For local development, close the game and deploy:

```powershell
pwsh -NoProfile -File tools/deploy.ps1 -GameDir 'C:\Program Files (x86)\Steam\steamapps\common\Fell & Sell'
python -m unittest discover -s tests
.\.venv\Scripts\python.exe tools/check-translations.py
```

The last command requires the local English/Japanese extraction described in
[game survey](docs/GAME-SURVEY.md). Translations preserve placeholders, rich text
tags and button labels; missing entries use the game's original text.

Remove those added mod files to uninstall the mod. Remove the separately installed
loader according to its own instructions if desired.

## Project

The directory and archive conventions follow the sibling `combolands-mod` project,
but this game is IL2CPP, so its Mono `net472` build and hooks are not copied.
The runtime identifies `Art Games Studio SA` / `Fell & Sell`; these values are
used in the MelonGame attribute. The sanitized name in `app.info` differs.

See [game survey and extraction](docs/GAME-SURVEY.md), [plan](docs/PLAN.md), [conventions](docs/CONVENTIONS.md) and
[third-party notes](THIRD-PARTY.md). Code is MIT licensed.
