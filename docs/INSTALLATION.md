---
layout: default
title: "Installation, updates and removal"
description: "Install the complete Korean patch with MelonLoader, then safely update or remove its files."
lang: "en"
permalink: "/installation/"
page_id: "installation"
page_type: "article"
summary: "Install the loader first, then copy the whole patch package into the game folder."
---

<nav class="toc" aria-label="On this page"><p>On this page</p><ul><li><a href="#requirements">Requirements</a></li><li><a href="#install">Install</a></li><li><a href="#update">Update</a></li><li><a href="#remove">Remove</a></li></ul></nav>

## Requirements {#requirements}

- A separately installed Steam Windows copy of Fell & Sell.
- [MelonLoader {{ site.data.patch.loader_version }} x64]({{ site.data.patch.loader_url }}), with its prerequisites.
- The packaged ZIP from [Downloads]({{ '/downloads/' | relative_url }}).

The checked environment is game {{ site.data.patch.game_version }} / Steam build {{ site.data.patch.steam_build }} on {{ site.data.patch.platform }}. Other game versions, loaders and systems are unverified.

## Install {#install}

1. Close the game. In Steam, choose **Properties → Installed Files → Browse**.
2. Install the x64 loader in the folder containing `Fell & Sell.exe`, following MelonLoader's instructions.
3. Extract the inner mod package ZIP into that folder. Merge its `Mods` and `UserData` folders; avoid an extra nested package folder.
4. Confirm these files exist:
   - `Mods/FellAndSellMod.dll`
   - `UserData/FellAndSell/locale/ko/strings.json`
   - `UserData/FellAndSell/locale/ko/mod-ui.json`
   - `UserData/FellAndSell/fonts/NotoSansCJKkr-Regular.otf` and `OFL-Noto.txt`
5. Start through Steam. The loader's first launch may take longer while preparing required files.
6. Korean text applies automatically. Press <kbd>F8</kbd> to enable optional [exploration tools]({{ '/features/' | relative_url }}), then close the panel to resume play.

There is no separate Korean language-menu entry or font-scale setting. Restart after changing the catalog. If text is missing, see [help]({{ '/help/' | relative_url }}).

<details markdown="1">
<summary>Optional: verify the package checksum</summary>

Keep the `.zip.sha256` beside the package ZIP. From that directory, run this in PowerShell:

```powershell
$archive = 'fell-and-sell-mod-v{{ site.data.patch.version }}.zip'
$expected = (Get-Content ($archive + '.sha256') -Raw).Split()[0]
$actual = (Get-FileHash -LiteralPath $archive -Algorithm SHA256).Hash
if ($actual -ine $expected) { throw 'Checksum mismatch' }
```

A match checks the ZIP against its accompanying checksum. Obtain both from the same project build or release.
</details>

## Update {#update}

Close the game and replace the DLL, catalogs and fonts using the complete newer package. Back up any personal translation edits first. Keep only one copy of `FellAndSellMod.dll` in Mods. Compatibility may change after game or loader updates.

## Remove {#remove}

Close the game and remove `Mods/FellAndSellMod.dll` and `UserData/FellAndSell/`. Leave other mods' files in place. Remove MelonLoader following its own instructions if you no longer need it. The patch does not alter original game binaries or save files.

[Need help?]({{ '/help/' | relative_url }})
