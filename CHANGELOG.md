# Changelog

## 0.3.0 — complete extracted-table translation

- Translate all 1243 entries (1128 Game and 115 UI), using Japanese as context.
- Complete items, furniture recipes, combat effects, tutorials, dialogue and quests.
- Align recipe names with their furniture names and preserve all formatting tokens.
- Add strict coverage validation to reject missing translation entries.
- Full visual and gameplay review remains pending.

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
