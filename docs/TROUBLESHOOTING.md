---
title: "Troubleshooting"
lang: en
---

# Troubleshooting

Start by recording game version, Steam build, MelonLoader version and mod version.
Translation startup and the F8 panel have been checked with
1.7.1 / 25480096 / 0.7.3 x64 / 0.4.0. Exploration-helper gameplay is a preview.

## Find the logs

- `<game folder>/MelonLoader/Latest.log`: loader startup, mod initialization,
  catalog, hooks and font messages. Copy it before another launch overwrites it.
- `%USERPROFILE%/AppData/LocalLow/Art Games Studio SA/Fell _ Sell/Player.log`:
  Unity/game messages. The sanitized directory name differs from the executable.

Expected mod messages include 1,243 loaded entries, installed raw/formatted hooks,
and 699 Hangul glyphs with `missing=0`. The number of observed fonts or text
components can vary by scene and launch timing; these are diagnostics, not fixed requirements.

## Downloaded files do not contain Mods or UserData

GitHub Actions wraps the package in an artifact download. Extract that outer ZIP
first, then install the inner `fell-and-sell-mod-vX.Y.Z.zip`. Do not install a
source-code archive, release-notes file or outer artifact as the mod.
An expired CI artifact requires a newer successful run or a public release.
[Downloads](DOWNLOADS.md) explains the two ZIP layers.

## Mod does not load or game fails to start

Confirm the DLL is directly under Mods and the loader is installed beside the
correct executable. Start from Steam: direct executable launch can request a
Steam restart and exit. Review the first error in Latest.log, rather than only
its final line. For loader installation failures, consult the
[MelonLoader project](https://github.com/LavaGang/MelonLoader).

To isolate a startup regression, close the game and move this mod DLL outside
Mods temporarily, then test again. If needed, test without other mods one at a
time, preserving their files. Report whether the same problem occurs without
this patch. Do not replace game binaries or grant broad filesystem permissions
as a translation fix.

## English or Japanese remains

Check `i18n: loaded 1243 Korean entries` and the hook-installation message.
A missing catalog or invalid JSON produces a translation initialization error.
Confirm the catalog is under UserData/FellAndSell, not beside the DLL.
New IDs or text outside the extracted tables may remain in the selected base
language. Include the screen and original text in a report. Credits containing
proper names and punctuation-only entries may be unchanged intentionally.

## Hangul appears as squares

Confirm both the OTF and OFL notice were extracted to the fonts directory.
Look for font creation, glyph verification or font error messages. Reinstall the
complete package with the game closed; copying only the DLL is insufficient.
Report the affected scene if a late-loaded font has no fallback.

## Text overflows or a translation is misleading

Full table coverage does not guarantee every layout fits. Send a screenshot,
the displayed sentence, expected meaning and reproduction steps. For effect
errors, include the numerical value and item/effect name. Translations are keyed
by stable table/entry IDs rather than English phrases.

## F8 panel or movement problems

The panel uses the bundled Noto font and its own TMP Canvas. Check
`panel: Noto glyph check 98; missing=0`. The panel temporarily suppresses player
actions while open; closing it with F8 or Esc returns control. It does not change
the game's persistent input-block flag. Native inventory/pause screens can still
block movement according to the game's rules.

The map appears only for a generated dungeon, not the town or main menu. Enable
route guidance and choose a map target before starting automatic movement.
Partial/invalid routes cannot start movement. Any feature failure reports its
name in Latest.log; report that entry and the screen state.

## Known limits

- The project owner confirmed in-game text display review on 2026-10-09. Broader
  gameplay, uninstall and Japanese-switching tests remain unrecorded.
- The loader logged a `Class::Init` signature fallback warning during successful
  test launches. That observation does not establish that every similar warning
  is harmless; include surrounding errors when reporting a crash.
- The source quest `quest.comfort_level_4.name` names level 9, while its objective
  says level 10. The patch preserves that discrepancy.
- Future game updates may change API signatures, table IDs or fonts.

## Submit a report

Use [Issues](https://github.com/myso-kr/fell-and-sell-mod/issues). Include versions,
other installed mods, steps, expected/actual behavior, and relevant log excerpts.
Remove usernames, personal paths, tokens and unrelated private data before posting.
Security concerns belong in [the private reporting process](https://github.com/myso-kr/fell-and-sell-mod/blob/main/SECURITY.md).
