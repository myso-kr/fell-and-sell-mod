"""Check version, catalog counts, notices and public documentation without game data."""
import argparse
import json
from pathlib import Path
import re
import subprocess
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]


def version():
    return ET.parse(ROOT / "Directory.Build.props").findtext("PropertyGroup/Version")


def check(tag=""):
    errors = []
    current = version()
    plugin = (ROOT / "src/FellAndSell.Mod/Plugin.cs").read_text(encoding="utf-8")
    if f'"{current}", "myso-kr"' not in plugin:
        errors.append("MelonInfo version differs from Directory.Build.props")
    if tag and tag != f"v{current}":
        errors.append(f"Tag {tag!r} differs from v{current}")
    headings = re.findall(r"^## (\d+\.\d+\.\d+)\b", (ROOT / "CHANGELOG.md").read_text(encoding="utf-8"), re.M)
    if not headings or headings[0] != current:
        errors.append("Newest versioned changelog heading differs from code")
    metadata = json.loads((ROOT / "release-metadata.json").read_text(encoding="utf-8"))
    catalog = json.loads((ROOT / "locale/ko/strings.json").read_text(encoding="utf-8"))
    for table, count in metadata["translation_counts"].items():
        if sum(key.startswith(table + "/") for key in catalog) != count:
            errors.append(f"Catalog count differs from release metadata: {table}")
    glyphs = {c for value in catalog.values() for c in value if '\uac00' <= c <= '\ud7a3' or '\u3130' <= c <= '\u318f'}
    if len(glyphs) != metadata["hangul_glyphs"]:
        errors.append("Hangul count differs from release metadata")
    ui = json.loads((ROOT / "locale/ko/mod-ui.json").read_text(encoding="utf-8"))
    if any(not isinstance(value, str) or not value.strip() for value in ui.values()):
        errors.append("Mod UI catalog contains an empty or non-string label")
    ui_glyphs = {c for value in ui.values() for c in value if not c.isspace()}
    if len(ui_glyphs) != metadata["extension_review"]["mod_ui_glyphs"]:
        errors.append("Mod UI glyph count differs from release metadata")
    for name in ("README.md", "docs/index.md", "docs/ko/index.md", "docs/ko/README.md"):
        text = (ROOT / name).read_text(encoding="utf-8")
        if current not in text or str(sum(metadata["translation_counts"].values())) not in text.replace(",", ""):
            errors.append(f"Public version or coverage claim is stale: {name}")
        if "releases" not in text or "0.7.3" not in text:
            errors.append(f"Missing download or loader guidance: {name}")
    for name in ("LICENSE", "NOTICE", "THIRD-PARTY.md", "locale/fonts/OFL-Noto.txt", "locale/fonts/NotoSansCJKkr-Regular.otf"):
        if not (ROOT / name).is_file() or not (ROOT / name).stat().st_size:
            errors.append(f"Missing license or bundled component: {name}")
    for name in subprocess.check_output(["git", "ls-files"], cwd=ROOT, text=True).splitlines():
        if name.startswith(("extracted/", "dist/")) or name.startswith("generated/") and name != "generated/README.md":
            errors.append(f"Generated data is tracked: {name}")
        if Path(name).suffix.lower() in (".dll", ".exe", ".bundle", ".assets", ".ress", ".unity3d", ".log"):
            errors.append(f"Runtime/game artifact is tracked: {name}")
    return errors


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--tag", default="")
    args = parser.parse_args()
    errors = check(args.tag)
    if errors:
        raise SystemExit("\n".join(errors))
    print(f"[OK] v{version()}: version, public claims, catalog counts and redistribution notices agree")
