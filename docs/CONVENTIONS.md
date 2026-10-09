---
title: "Directory conventions"
lang: en
---

# Directory conventions

| Directory | Contents |
|---|---|
| src/ | C# mod running under MelonLoader's IL2CPP runtime |
| locale/ | Authored translations and licensed fonts |
| tools/ | Build, repository/package checks, notes and packaging commands |
| .github/workflows/ | Shared package build, CI, tag releases and docs validation |
| release-metadata.json | Committed tested game/build, coverage and glyph counts |
| generated/ | Ignored extraction and inspection output |
| tests/ | Game-independent translation, movement policy, reflection and interop-list fixtures |
| docs/ | User guides, runtime notes and publication procedure |
| .spec/ | Internal plans, design, architecture, anchors and verification status |

Release layout: `Mods/FellAndSellMod.dll` and
`UserData/FellAndSell/locale/ko/strings.json`, with Noto Sans CJK KR and its OFL notice under `UserData/FellAndSell/fonts/`.
MelonLoader is installed separately. Game files, loader binaries and generated
interop assemblies are not redistributed. Keep game-specific hooks separate
from the entry point and verify them against the installed game build.
Use lowercase feature directories (`i18n/`, `autoplay/`, `map/`, `guide/`, `panel/`)
and PascalCase C# files/types. Plugin only wires modules. Read game state into plain
snapshots, keep decisions game-independent, and centralize automation writes in
`autoplay/Exec.cs`. UI draws and queues commands; it does not mutate game state.
See the [architecture specification](https://github.com/myso-kr/fell-and-sell-mod/blob/main/.spec/ARCHITECTURE.md).
