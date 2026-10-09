---
title: "Installation, updates and removal"
lang: en
---

# Installation, updates and removal

## Requirements

- A separately installed Steam Windows copy of Fell & Sell (AppID 4627110).
- MelonLoader 0.7.3 x64, installed using its own instructions and prerequisites.
- A packaged `fell-and-sell-mod-vX.Y.Z.zip`, rather than the repository source ZIP.

The verified target is game 1.7.1 / Steam build 25480096 on Windows 11.
Other game versions, loaders and operating systems are unverified.
Download the loader from its [official release](https://github.com/LavaGang/MelonLoader/releases/tag/v0.7.3).
Project packages are listed under [Releases](https://github.com/myso-kr/fell-and-sell-mod/releases).
Before a public release exists, use the latest successful
[CI build](https://github.com/myso-kr/fell-and-sell-mod/actions/workflows/ci.yml).
Sign in to GitHub, download the `fell-and-sell-mod-v0.4.0` artifact and extract
the outer artifact archive first. Install the mod ZIP inside it, not the outer
archive. CI artifacts are kept for 30 days. [Download instructions](DOWNLOADS.md)
include a gh command; [building locally](DEVELOPMENT.md) is also supported.

## Verify a package

Download the `.zip.sha256` beside the ZIP. From the download directory in PowerShell:

```powershell
$archive = 'fell-and-sell-mod-v0.4.0.zip'
$expected = ((Get-Content -LiteralPath ($archive + '.sha256') -Raw).Trim() -split '\s+')[0]
$actual = (Get-FileHash -LiteralPath $archive -Algorithm SHA256).Hash
if ($actual -ine $expected) { throw 'Package checksum mismatch.' }
```

A matching hash confirms the ZIP matches the accompanying checksum, not the
identity of its publisher. Obtain both from the same project release or CI run.

## Install

1. Close the game. In Steam, use Properties → Installed Files → Browse.
2. Install the x64 loader into the folder containing `Fell & Sell.exe`.
3. Extract the mod ZIP over that folder. Do not create an extra nested mod folder.
4. Check that `Mods/FellAndSellMod.dll`,
   `UserData/FellAndSell/locale/ko/strings.json`, and the font and OFL notice under
   `UserData/FellAndSell/fonts/` exist.
5. Start via Steam. Allow the loader to generate IL2CPP interop assemblies on its
   first launch. Check [the startup log](TROUBLESHOOTING.md).

The catalog is loaded at startup; restart after editing translations. The Korean
overlay applies automatically to matching entries. There is no language toggle,
font-scale setting or added Korean language-menu entry in v0.4.0.

## Update

Close the game and replace the mod DLL, translation catalog and font files with
the newer package. Preserve any local catalog edits elsewhere before replacing it.
Keep one copy of `FellAndSellMod.dll` under Mods to avoid duplicate loading.
Recheck startup logs after a game or loader update; compatibility may change.

## Uninstall

Close the game, delete `Mods/FellAndSellMod.dll` and `UserData/FellAndSell/`, and
start again. Do not delete the entire Mods or UserData directory if other mods use
it. Remove MelonLoader according to its own instructions if it is no longer needed.
The mod does not patch original game binaries or edit saves. Steam verification
checks game files, but is not a substitute for removing added mod files.

The ZIP's root-level README, changelog and license documents are informational;
they are not loaded by the game. Preserve relevant licenses if redistributing.
