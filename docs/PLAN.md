# Implementation plan

1. Scaffold: IL2CPP MelonMod entry point, reproducible build, packaging and Git.
2. Install MelonLoader separately and verify it starts the retail game. Record
   the Steam build, Unity version, loader version and generated interop assemblies.
3. Inspect Unity.Localization tables and string retrieval methods. Select hooks
   using actual game metadata rather than copying Combolands' Mono hooks.
4. Extract source strings locally, translate Korean, and preserve format tokens.
5. Implement TMP Hangul font fallback and verify menu and scene transitions.
6. Verify in-game rendering and uninstall behavior, then produce a release archive.

Current status: steps 1–3 complete. Steps 4 and 5 have a working first pass:
371 translated entries, English/Japanese context, runtime formatting hooks,
and a licensed dynamic Hangul font. Remaining translations and visual/gameplay
review are still outstanding. Runtime logs verify hooks and glyph coverage;
they do not establish that every screen's layout fits.
See [game survey](GAME-SURVEY.md) for build and extraction details.
