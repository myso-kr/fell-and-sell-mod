---
layout: default
title: "Korean patch"
description: "Play Fell & Sell with Korean localization. 1,243 translated entries, installation and support guides."
lang: en
home: true
permalink: /
---

<p class="eyebrow">Fell &amp; Sell · Unofficial language mod</p>

# A Korean storefront.<br>A familiar adventure.

<p class="lead">Items, recipes, quests and conversations in Korean. A language patch for the Steam game Fell &amp; Sell, built on MelonLoader.</p>

<div class="actions">
<a class="button" href="{{ '/DOWNLOADS.html' | relative_url }}">Download a build</a>
<a class="button secondary" href="{{ '/INSTALLATION.html' | relative_url }}">Installation guide</a>
</div>

<dl class="ledger">
<div><dt>Translated entries</dt><dd>1,243 / 1,243</dd></div>
<div><dt>Patch version</dt><dd>0.4.0</dd></div>
<div><dt>Verified game</dt><dd>1.7.1</dd></div>
</dl>

<div class="note" markdown="1">
**Complete table coverage; text display review completed.** All extracted Game and UI
entries are covered. Runtime checks confirm translation hooks and 699 Hangul
glyphs without missing characters. The project owner confirmed in-game text
display review on 2026-10-09. Text outside these tables may require additional work.
</div>

## From dungeon loot to shop shelves

The patch translates 1,128 Game entries and 115 UI entries, including equipment,
furniture, crafting recipes, combat effects, tutorials, dialogue and quests.
English establishes the meaning; Japanese helps clarify context. Matching item
and recipe names keep crafting menus consistent.

The overlay applies automatically to matching entries regardless of the selected
base language. There is no additional Korean option in the language menu.
New entries absent from the catalog retain the game's selected language.

## Install in three steps

1. Find the folder containing `Fell & Sell.exe` through Steam's Installed Files menu.
2. Install [MelonLoader 0.7.3 x64](https://github.com/LavaGang/MelonLoader/releases/tag/v0.7.3) separately.
3. Extract the packaged mod ZIP over that folder, then start the game through Steam.

```text
Fell & Sell/
├─ Mods/FellAndSellMod.dll
└─ UserData/FellAndSell/
   ├─ locale/ko/strings.json
   └─ fonts/NotoSansCJKkr-Regular.otf + OFL-Noto.txt
```

Use a packaged release ZIP, not GitHub's source-code archive. If a package has
not yet been published, download a successful CI artifact using the
[download guide](DOWNLOADS.md). GitHub sign-in is required; artifacts expire after
30 days. CI builds the DLL, mod ZIP and checksum without a game installation.
Version-tag builds prepare a draft prerelease for review, not a public download.
[Installation and checksums](INSTALLATION.md) cover updating and removal.

## What has been checked

| Check | Result |
|---|---|
| Game | 1.7.1 / Steam build 25480096, Windows 11 |
| Runtime | Unity 6000.3.10f1 IL2CPP / MelonLoader 0.7.3 x64 |
| Catalog and formatting | 1,243 entries, complete coverage and token checks passed |
| Font | 699 Hangul glyphs, zero missing |
| Text display | Reviewed in-game, confirmed by the project owner on 2026-10-09 |
| Remaining checks | Broader gameplay, Japanese switching and uninstall |

The mod adds its own files and changes localization templates in memory. It does
not patch original game binaries or implement save editing. Close the game and
remove its DLL and `UserData/FellAndSell/` to uninstall the Korean patch.

## Exploration helper preview

v0.4.0 adds nearby pickup, a read-only dungeon map, route display and automatic movement.
All new features default off. F8 opens settings, F9 toggles the map and F10 starts/stops movement.
The owner verified the base menu, Korean panel and input return after closing it;
map display, selected destinations and return to the next-floor route are confirmed. The guide light and fade were confirmed. Pickup and automatic movement remain under review. Achievements retain their existing handling.
[Controls and scope](EXPANSION.md)

## Help improve the patch

An awkward sentence or clipped button is useful feedback. Include the screen,
versions and a screenshot or relevant log excerpt in an
[issue](https://github.com/myso-kr/fell-and-sell-mod/issues).
[한국어 안내](ko/README.md) includes the installation and feedback guide in Korean.

[Documentation](README.md) · [Troubleshooting](TROUBLESHOOTING.md) ·
[Translation glossary](TRANSLATION.md)
