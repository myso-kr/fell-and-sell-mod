---
title: "Implementation plan"
lang: en
---

# Implementation plan

1. Scaffold: IL2CPP MelonMod entry point, reproducible build, packaging and Git.
2. Install MelonLoader separately and verify it starts the retail game. Record
   the Steam build, Unity version, loader version and generated interop assemblies.
3. Inspect Unity.Localization tables and string retrieval methods. Select hooks
   using actual game metadata rather than copying Combolands' Mono hooks.
4. Extract source strings locally, translate Korean, and preserve format tokens.
5. Implement TMP Hangul font fallback and verify menu and scene transitions.
6. Verify in-game rendering and uninstall behavior, then produce a release archive.

Current status: steps 1–5 implemented, including all 1243 translations,
English/Japanese context, formatting hooks and licensed dynamic Hangul fallback.
GitHub Actions builds verified installable packages, retains CI artifacts for
30 days and prepares version-tag draft prereleases. Pages and mod builds use
separate workflows. Runtime checks are available. The project owner confirmed
in-game text display review on 2026-10-09. Broader gameplay, Japanese switching
and uninstall tests remain separate checks; their completion is not recorded.
See [game survey](GAME-SURVEY.md) for build and extraction details.

## Planned mod expansion

The owner requested nearby auto-pickup, map reveal, route guidance and auto movement,
with achievements preserved. These are planned, not part of v0.3.0.
See [the evidence and design](EXPANSION.md) and
[the implementation checklist](https://github.com/myso-kr/fell-and-sell-mod/blob/main/mod-expansion-plan.md).
