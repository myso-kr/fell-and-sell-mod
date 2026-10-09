"""Verify install layout, source payload identity and the ZIP checksum."""
import argparse
import hashlib
import json
from pathlib import Path
from zipfile import ZipFile
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]


def check(archive: Path):
    checksum = archive.with_suffix(archive.suffix + ".sha256").read_text(encoding="utf-8").split()[0]
    assert hashlib.sha256(archive.read_bytes()).hexdigest() == checksum.lower(), "ZIP checksum mismatch"
    with ZipFile(archive) as package:
        names = set(package.namelist())
        payloads = {
            "UserData/FellAndSell/locale/ko/mod-ui.json": "locale/ko/mod-ui.json",
            "UserData/FellAndSell/locale/ko/strings.json": "locale/ko/strings.json",
            "UserData/FellAndSell/fonts/NotoSansCJKkr-Regular.otf": "locale/fonts/NotoSansCJKkr-Regular.otf",
            "UserData/FellAndSell/fonts/OFL-Noto.txt": "locale/fonts/OFL-Noto.txt",
        }
        for target, source in payloads.items():
            packaged = package.read(target)
            original = (ROOT / source).read_bytes()
            if target.endswith(".txt"):
                packaged = packaged.replace(b"\r\n", b"\n")
                original = original.replace(b"\r\n", b"\n")
            assert packaged == original, f"Payload mismatch: {target}"
        for name in ("Mods/FellAndSellMod.dll", "README.md", "LICENSE", "NOTICE", "THIRD-PARTY.md", "docs/ko/README.md", "docs/INSTALLATION.md"):
            assert name in names and package.getinfo(name).file_size > 0, f"Missing package file: {name}"
        for source in (ROOT / "docs").rglob("*"):
            relative = source.relative_to(ROOT).as_posix()
            if source.is_file() and source.relative_to(ROOT / "docs").parts[0] in {"_data", "_includes", "aliases"}:
                assert relative in names, f"Missing site dependency: {relative}"
        assert {n for n in names if n.endswith((".dll", ".exe"))} == {"Mods/FellAndSellMod.dll"}, "Unexpected runtime binaries"
        assert not any(".jekyll-cache" in n or n.startswith(("generated/", "extracted/", ".spec/")) for n in names), "Technical/generated output included"
        data = json.loads(package.read("UserData/FellAndSell/locale/ko/strings.json"))
        assert len(data) == sum(json.loads((ROOT / ".spec/release-metadata.json").read_text())["translation_counts"].values()), "Catalog coverage mismatch"


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("archive", nargs="?", type=Path)
    args = parser.parse_args()
    current = ET.parse(ROOT / "Directory.Build.props").findtext("PropertyGroup/Version")
    archive = args.archive or ROOT / "dist" / f"fell-and-sell-mod-v{current}.zip"
    check(archive)
    print(f"[OK] Package payloads, license files and SHA-256 verified: {archive.name}")
