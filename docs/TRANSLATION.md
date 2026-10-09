---
title: "Korean translation decisions"
lang: en
---

# Korean translation decisions

English is the semantic baseline; Japanese is a context reference joined by
table/entry ID. A Japanese translation may be stale or wrong, so it is never
copied mechanically. Shared key names also clarify the screen and function.
Only authored Korean strings are committed; English/Japanese context remains
in ignored local extraction output.

| Term | Korean | Context |
|---|---|---|
| Comfort | 쾌적도 | Shop furniture stat, affects customer traffic |
| Prestige | 명성 | Shop furniture stat, affects sale prices |
| Tip | 팁 | Shop stat, not a gameplay hint |
| Stamina | 기력 | Combat resource |
| Recipe | 제작법 | Crafting unlock, not food |
| Chest | 보관함 / 보물 상자 | Storage furniture / dungeon reward |
| Table | 진열대 | Sale furniture; distinct from an ordinary decorative table |
| Holy / Unholy / Blasphemous | 신성 / 불경한 / 불경 | Blessings, corrupted effects and rarity |
| Prayer / Boon | 기도 / 은총 | Acquired effect / selection prompt |
| Affix | 특성 | Extra enemy modifier |
| Common / Uncommon / Rare / Epic / Legendary | 일반 / 고급 / 희귀 / 영웅 / 전설 | Item rarity |

Two observed Japanese issues are corrected in Korean:

- `UI` key `Location.Forest` refers to the forest, but Japanese says FPS limit.
  Korean uses 숲, consistent with English and the location key.
- `Game` key `Stat_Tip` uses the Japanese word for hint. Its shop-stat context
  indicates gratuities, so Korean uses 팁.

Menu labels are concise nouns or actions. Tutorial instructions use polite,
direct language and preserve all button substitutions. Modifier descriptions
keep the numeric effects and probability distinctions. Placeholders such as
`{0}`, `{Inventory}` and `{inventory}` are case-sensitive and unchanged.
Rich text tags and `[ESC]`-style key labels remain intact.

Coverage in v0.3.0: 1243/1243 entries (100%): Game 1128 and UI 115.
Furniture recipe names are derived from their corresponding Korean furniture
names. Japanese mushroom-cap names and the Moon decoration recipe are resolved
using English and item context. Proper names in credits and punctuation-only
entries remain unchanged intentionally.

The source quest `quest.comfort_level_4.name` names level 9 while its objective
requires level 10. Korean preserves this source discrepancy instead of changing
the gameplay requirement. Death-save descriptions distinguish fatal blows from
critical hits. Numeric effects and formatting tokens are preserved.

All extracted entries are covered. The project owner confirmed completion of
in-game text display review on 2026-10-09. Broader gameplay and language-switching
checks remain separate.
CI checks the committed counts against `release-metadata.json` and validates fixtures
and catalog shape. Full source-token/coverage checks still run locally against
ignored game extraction; a green Actions run does not prove translation semantics.
