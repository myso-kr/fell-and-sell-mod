# Directory conventions

| Directory | Contents |
|---|---|
| src/ | C# mod running under MelonLoader's IL2CPP runtime |
| locale/ | Authored translations and licensed fonts |
| tools/ | Build, verification and packaging commands |
| generated/ | Ignored extraction and inspection output |
| tests/ | Game-independent translation-validator fixtures |
| docs/ | User guides, runtime notes and publication procedure |

Release layout: `Mods/FellAndSellMod.dll` and
`UserData/FellAndSell/locale/ko/strings.json`, with Noto Sans CJK KR and its OFL notice under `UserData/FellAndSell/fonts/`.
MelonLoader is installed separately. Game files, loader binaries and generated
interop assemblies are not redistributed. Keep game-specific hooks separate
from the entry point and verify them against the installed game build.
