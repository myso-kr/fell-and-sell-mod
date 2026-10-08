# Directory conventions

| Directory | Contents |
|---|---|
| src/ | C# mod running under MelonLoader's IL2CPP runtime |
| locale/ | Authored translations and licensed fonts |
| tools/ | Build, verification and packaging commands |
| generated/ | Ignored extraction and inspection output |
| tests/ | Game-independent tests when behavior is implemented |
| docs/ | Architecture and verification notes |

Release layout: `Mods/FellAndSellMod.dll` and
`UserData/FellAndSell/locale/ko/strings.json`, with fonts added later.
MelonLoader is installed separately. Game files, loader binaries and generated
interop assemblies are not redistributed. Keep game-specific hooks separate
from the entry point and verify them against the installed game build.
