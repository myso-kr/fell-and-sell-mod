# Fell & Sell — Korean patch

MelonLoader-based Korean language patch for the Steam game **Fell & Sell**.

**Status: initial scaffold only.** The project builds a logging-only MelonMod.
Translation hooks, translated strings and Hangul fonts are not implemented.
The mod has not been tested in the game.

## Build

Use .NET SDK 8 to build the `net6.0` mod for MelonLoader 0.7.3's IL2CPP runtime:

```powershell
dotnet build FellAndSell.Mod.sln -c Release
pwsh -NoProfile -File tools/verify.ps1
pwsh -NoProfile -File tools/package.ps1
```

The NuGet loader reference is for compilation only; loader binaries are not shipped.
Unity and game interop references will be added after the first loader startup.

## Development installation

Install [MelonLoader](https://github.com/LavaGang/MelonLoader) separately into the
folder containing `Fell & Sell.exe`. Extract the package over that folder.
It adds `Mods/FellAndSellMod.dll` and `UserData/FellAndSell/locale/ko/strings.json`.
Check `MelonLoader/Latest.log` for the scaffold initialization message.
The empty catalog does not translate the game yet.

Remove those added mod files to uninstall the mod. Remove the separately installed
loader according to its own instructions if desired.

## Project

The directory and archive conventions follow the sibling `combolands-mod` project,
but this game is IL2CPP, so its Mono `net472` build and hooks are not copied.
The game's `app.info` identifies `Art Games Studio SA` / `Fell _ Sell`;
these values are used in the MelonGame attribute pending runtime verification.

See [plan](docs/PLAN.md), [conventions](docs/CONVENTIONS.md) and
[third-party notes](THIRD-PARTY.md). Code is MIT licensed.
