# Changelog

## Unreleased

- Share an Actions package build between CI and tag releases, and retain verified CI artifacts for 30 days.
- Check version, public claims, catalog counts, package payloads and checksum; generate release notes from recorded metadata.
- Build version-tag packages into draft prereleases and document artifact downloads in both languages.

- Add a bilingual Jekyll Pages site, custom responsive layout, dark mode and site-link checks.

- Add English and Korean public-facing guides, installation/troubleshooting and contribution instructions.
- Document runtime anchors, release checks and private security reporting.
- Add bilingual issue forms and a pull request template.
- Include the linked documentation in mod archives.

## 0.3.0 — complete extracted-table translation

Verified on game 1.7.1 / Steam build 25480096 with MelonLoader 0.7.3 x64.
Startup checks loaded 1243 entries and verified 699 Hangul glyphs with no missing glyphs.

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
