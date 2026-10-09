# Troubleshooting

Start by recording game version, Steam build, MelonLoader version and mod version.
The currently tested combination is 1.7.1 / 25480096 / 0.7.3 x64 / 0.3.0.

## Find the logs

- `<game folder>/MelonLoader/Latest.log`: loader startup, mod initialization,
  catalog, hooks and font messages. Copy it before another launch overwrites it.
- `%USERPROFILE%/AppData/LocalLow/Art Games Studio SA/Fell _ Sell/Player.log`:
  Unity/game messages. The sanitized directory name differs from the executable.

Expected mod messages include 1,243 loaded entries, installed raw/formatted hooks,
and 699 Hangul glyphs with `missing=0`. The number of observed fonts or text
components can vary by scene and launch timing; these are diagnostics, not fixed requirements.

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

## Known limits

- No exhaustive visual/gameplay review, uninstall test or Japanese-switching
  gameplay test has been completed.
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
Security concerns belong in [the private reporting process](../SECURITY.md).
