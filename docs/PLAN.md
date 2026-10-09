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
Release packaging and runtime checks are available. Step 6 still needs visual
review of gameplay screens and uninstall testing. Runtime logs and glyph checks
do not establish that every screen's layout fits.
See [game survey](GAME-SURVEY.md) for build and extraction details.
