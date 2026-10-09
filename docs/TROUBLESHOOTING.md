---
layout: "default"
title: "Troubleshooting"
description: "Fix download, startup, Korean text, input, map, guidance and nearby pickup problems; find logs and report an issue."
lang: "en"
permalink: "/help/"
page_id: "troubleshooting"
page_type: "article"
summary: "Choose the symptom, try the relevant checks, then report what remains."
---

<nav class="toc" aria-label="Choose a symptom"><p>Choose a symptom</p><ul><li><a href="#download">Download</a></li><li><a href="#startup">Startup</a></li><li><a href="#text">Korean text</a></li><li><a href="#input">Controls</a></li><li><a href="#map">Map and route</a></li><li><a href="#pickup">Pickup</a></li><li><a href="#report">Report</a></li></ul></nav>

## Download or extraction problems {#download}

Sign in to GitHub and open a successful main build. Expired artifacts require a newer successful run. Extract the outer artifact ZIP, then the mod package ZIP inside it. The inner package contains `Mods` and `UserData`. A repository source ZIP cannot be installed. [Download steps]({{ '/downloads/' | relative_url }})

## Mod does not load or the game fails to start {#startup}

Check that the loader is beside the correct executable and `FellAndSellMod.dll` is directly inside Mods. Start through Steam. If needed, close the game and temporarily move this DLL out of Mods to check whether the problem occurs without the patch. Preserve other mods' files when testing them separately.

Loader installation problems are covered by the [MelonLoader project](https://github.com/LavaGang/MelonLoader). Include the first relevant startup error when reporting a crash.

## Missing, square or clipped Korean text {#text}

Reinstall the **complete package** with the game closed. Copying only the DLL leaves out translations and fonts. Check the file locations in [Installation]({{ '/installation/#install' | relative_url }}). For square labels, check the OTF font under `UserData/FellAndSell/fonts/`.

Korean applies automatically; there is no added Korean language-menu entry. Credits, proper names and punctuation-only text may remain unchanged. New text outside covered entries may appear in the selected base language. For incorrect or clipped text, include a screenshot, the displayed sentence and the expected meaning. See the [glossary]({{ '/glossary/' | relative_url }}).

## F8 or gameplay controls stop responding {#input}

Close F8 with F8 or Esc, and close the game's inventory or pause screen. Return focus to the game. Normal movement and camera controls should resume after the panel closes. Intermittent input problems have been addressed, but report any recurrence with the open menus and your previous action.

Automatic movement stops on manual input, combat, damage, menus or loss of focus. F10 starts it again only on a complete route.

## Map or guidance does not appear {#map}

Enter a generated dungeon, enable the desired features in F8, close F8 and open M. Enable route guidance before selecting a destination. Guidance defaults to the next-floor stairs; right-click the M map to return to that target.

The map and guide should refresh after changing floors. Report the old and new floor if they do not. Partial routes cannot start automatic movement. Closed doors require interaction. For an unreachable destination, include its location, the F8 status and a screenshot.

## Nearby pickup does not collect an item {#pickup}

Enable nearby pickup in F8 and stand within the configured radius (3m by default). Check weight limits, pickup filters and the game's pickup delay. It collects eligible nearby loot rather than moving to distant items. If manually collecting the same item works, report the item and your settings.

## Send a useful report {#report}

[Open an issue]({{ site.data.patch.issues_url }}) with game, loader and patch versions, other mods, reproduction steps, expected and actual behaviour, and a screenshot when useful. Remove personal paths and private information before posting. Use [private reporting](https://github.com/myso-kr/fell-and-sell-mod/blob/main/SECURITY.md) for security concerns.

<details markdown="1">
<summary>Find logs to attach to your report</summary>

- `<game folder>/MelonLoader/Latest.log`: startup and feature errors. Copy it before another launch overwrites it.
- `%USERPROFILE%/AppData/LocalLow/Art Games Studio SA/Fell _ Sell/Player.log`: game messages.

For toggle issues, include `settings:` lines. For route issues, include `route:` lines and the destination. Include surrounding errors rather than only the final line.
</details>
