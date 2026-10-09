---
title: "Runtime anchors and game-update checks"
lang: en
---

# Runtime anchors and game-update checks

These connections were observed in game 1.7.1 / Steam build 25480096. They are
reflection-based runtime dependencies; a successful build alone cannot validate them.

| Anchor | Purpose and failure symptom | Implementation |
|---|---|---|
| MelonGame identity `Art Games Studio SA` / `Fell & Sell` | Mod selection; mismatch prevents loading | `src/FellAndSell.Mod/Plugin.cs` |
| `TableEntry.Table`, `TableCollectionName`, `KeyId` | Resolve `Game/<ID>` / `UI/<ID>`; mismatch loses translations | `I18n/Catalog.cs` |
| `TableEntry.LocalizedValue` getter | Raw text replacement | `I18n/Patches.cs` |
| `StringTableEntry.GetLocalizedString`, entry Data.Localized and m_FormatCache | Replace template before formatting; mismatch breaks formatted retrieval | `I18n/Patches.cs` |
| Unity.Localization interop assembly | Resolve localization types and hook methods | `I18n/RuntimeTypes.cs` |
| `Il2CppTMPro` types in Unity.TextMeshPro | Resolve TMP assets and components | `I18n/RuntimeTypes.cs` |
| Resources.FindObjectsOfTypeAll and TMP font creation/fallback APIs | Find fonts, create dynamic Noto fallback; failure causes missing Hangul | `I18n/FontFallback.cs` |
| Scene callback and periodic font discovery | Catch fonts loaded through Addressables after scene initialization | `Plugin.cs`, `I18n/FontFallback.cs` |
| English/Japanese/shared Addressables bundles | Local source extraction; renamed or changed bundles break extraction | `tools/extract-strings.py` |

Implementation paths prefixed `I18n/` are under `src/FellAndSell.Mod/`.
The runtime game name contains `&`; the app.info and Player.log folder use the
sanitized `Fell _ Sell`. Do not substitute one for the other.

Actions compiles and packages this mod without game assemblies because it resolves
these APIs at runtime. CI checks do not validate runtime anchors; a game update
still needs local inspection and play.

After an update, record the new build and Unity version, allow the loader to
regenerate interop if needed, re-extract source tables and run complete coverage
validation. Check initialization errors, translation hits, font glyph coverage,
formatted text and late-loaded scenes. Review English/Japanese switching and
visual layout. Update the survey and changelog only with evidence actually observed.
