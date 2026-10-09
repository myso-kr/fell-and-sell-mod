# Game survey — 2026-10-09

## Verified runtime

| Item | Observed value |
|---|---|
| Steam AppID | 4627110 |
| Steam build | 25480096 |
| Game version | 1.7.1 |
| Unity | 6000.3.10f1 |
| Backend | IL2CPP, metadata version 39 |
| Loader | MelonLoader 0.7.3, net6 |
| Runtime game identity | Art Games Studio SA / Fell & Sell |

The official x64 loader archive was installed locally. Its first run generated
the IL2CPP interop assemblies successfully. Launch through Steam (`-applaunch
4627110`): direct executable launch requests a Steam restart and exits.

The loader reported one mod loaded and the scaffold initialization message on
2026-10-09 at 08:58:14 local time. This verifies entry-point execution, not Korean
rendering. It also logged a Class::Init signature fallback warning; further scene
and gameplay compatibility remains to be tested.

`app.info` contains the sanitized `Fell _ Sell`, whereas MelonLoader uses the
runtime `Fell & Sell`. The mod filter now uses the runtime name.

## Localization

Addressables bundles live in
`Fell & Sell_Data/StreamingAssets/aa/StandaloneWindows64`.
There are 17 language bundles and no Korean bundle. English contains:

| Table | Entries | Entries without shared key names |
|---|---:|---:|
| Game | 1128 | 9 |
| UI | 115 | 1 |
| Total | 1243 | 10 |

Shared data contains 1119 Game keys and 114 UI keys. English tables contain
additional entries with no shared name, so name-only translation lookup would
lose entries. Use `Game/<numeric entry ID>` and `UI/<numeric entry ID>` in the
authored catalog. Preserve IDs as strings to avoid JSON number precision loss.
The extraction output retains shared names when available for translator context.

Extract locally:

```powershell
python -m venv .venv
.\.venv\Scripts\python.exe -m pip install -r tools/requirements.txt
.\.venv\Scripts\python.exe tools/extract-strings.py --game-dir 'C:\Program Files (x86)\Steam\steamapps\common\Fell & Sell'
```

`generated/strings.en.json` and `generated/extraction-report.json` are ignored.
No source strings or game assemblies are committed.

## Next investigation

The generated localization API exposes `TableEntry.Table`, `KeyId`,
`LocalizedValue` and `StringTableEntry.GetLocalizedString`. v0.2.0 patches raw
retrieval and replaces the source template before formatting, clearing its
SmartFormat cache when changed. Runtime logs confirm translation hook hits.

TMP types are generated under `Il2CppTMPro`, unlike the Mono sibling project's
`TMPro` namespace. Runtime resolution accounts for both namespaces. A dynamic
Noto Sans CJK KR font is registered on five loaded font assets. The 371-entry
catalog uses 402 distinct Hangul glyphs; the latest run reports zero missing.
An earlier 263-entry run confirmed seven Korean TMP text components.

Next: finish translations, inspect UI layout and gameplay screens visually,
verify Japanese base-language switching, and exercise formatted strings and
late-loaded scene fonts in actual gameplay.
