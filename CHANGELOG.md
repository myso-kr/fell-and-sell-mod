# Changelog

## Unreleased

## 0.4.0 — exploration helper preview

- Refresh the map and exit guidance when changing floors.
- Keep nearby pickup working independently of the game’s auto-pickup option.
- Add optional nearby loot pickup and reveal/markers on the game's existing M map and minimap.
- Guide to the next-floor exit by default; choose a destination with left-click and return to exit guidance with right-click.
- Replace the floating route line with a moving light and short trail that fades with distance and terrain occlusion.
- Add manually started automatic movement; stop on manual input, combat, damage, menus, death, loss of focus or no progress.
- Preserve achievement handling and add Korean F8 settings, F9 map reveal and F10 movement controls.
- Confirm settings toggles, map display, destination selection and return to exit guidance with the project owner. Pickup, automatic movement and the new guide-light appearance remain under review.
- Provide English/Korean player guides and verified preview packages through GitHub Actions.

## 0.3.0 — complete extracted-table translation

Verified on game 1.7.1 / Steam build 25480096 with MelonLoader 0.7.3 x64.
Startup checks loaded 1243 entries and verified 699 Hangul glyphs with no missing glyphs.

- Translate all 1243 entries (1128 Game and 115 UI), using Japanese as context.
- Complete items, furniture recipes, combat effects, tutorials, dialogue and quests.
- Align recipe names with their furniture names and preserve all formatting tokens.
- Add strict coverage validation to reject missing translation entries.
- In-game text display review was subsequently confirmed complete by the project owner on 2026-10-09; broader gameplay checks remain separate.

## 0.2.0 — partial Korean patch

- Compare English and Japanese by numeric table/entry ID for translation context.
- Translate 371 entries, including all 115 UI table entries.
- Patch raw and formatted localization retrieval before placeholder formatting.
- Add licensed Noto Sans CJK KR dynamic font fallback and glyph verification.
- Add deployment tool and translation-token validation with fixture tests.
- Item descriptions, remaining tutorials and quests are not fully translated.

## 0.1.0 — scaffold

- Add IL2CPP MelonMod entry point and build configuration.
- Establish translation, font, extraction and documentation directories.
- Add verification and mod-only ZIP packaging.
- Verify loader and mod entry point on game 1.7.1 / Steam build 25480096.
- Add local extraction of 1243 English localization entries with stable table/ID keys.
- No translation functionality or Korean rendering verification yet.
