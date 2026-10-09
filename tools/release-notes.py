"""Compose a draft release description from committed metadata and changelog."""
import argparse
import json
from pathlib import Path
import re
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]


def compose():
    version = ET.parse(ROOT / "Directory.Build.props").findtext("PropertyGroup/Version")
    metadata = json.loads((ROOT / "release-metadata.json").read_text(encoding="utf-8"))
    changelog = (ROOT / "CHANGELOG.md").read_text(encoding="utf-8")
    section = re.search(r"^## " + re.escape(version) + r"[^\n]*\n(.*?)(?=^## |\Z)", changelog, re.M | re.S)
    if not section:
        raise ValueError("Missing versioned changelog entry")
    if metadata.get("visual_review_complete"):
        review = metadata["visual_review"]
        visual = f"In-game text display review was confirmed complete by the {review['reported_by']} on {review['reported_on']}."
        visual_ko = "프로젝트 소유자가 인게임 텍스트 표시 검수 완료를 확인했습니다."
    else:
        visual = "In-game text display review remains pending."
        visual_ko = "인게임 텍스트 표시 검수는 아직 완료하지 않았습니다."
    return f"""# Fell & Sell Korean patch v{version}

{section.group(1).strip()}

## Verified target

Game {metadata['game_version']} / Steam build {metadata['steam_build']},
Unity {metadata['unity_version']} IL2CPP, MelonLoader {metadata['loader_version']} x64,
{metadata['verified_os']}. Coverage: Game {metadata['translation_counts']['Game']},
UI {metadata['translation_counts']['UI']}. Recorded local glyph check:
{metadata['hangul_glyphs']} Hangul glyphs, zero missing.

GitHub Actions builds and verifies the ZIP and SHA-256 without the game installed.
This does not replace in-game testing. {visual}
Broader gameplay, Japanese switching and uninstall checks remain unrecorded;
this is a preview build.

## Install / 설치

Install MelonLoader {metadata['loader_version']} x64 separately. Extract
`fell-and-sell-mod-v{version}.zip` over the directory containing `Fell & Sell.exe`,
then launch through Steam. The attached `.zip.sha256` verifies the mod ZIP.
Do not install GitHub's source-code archive or extract the outer Actions artifact
as if it were the mod package.

MelonLoader를 별도로 설치한 뒤 패치 ZIP을 게임 폴더에 풀고 Steam으로 실행하세요.
{visual_ko} 전반적인 플레이·일본어 전환·제거 테스트는 별도 확인 항목입니다.

[한국어 안내](https://myso-kr.github.io/fell-and-sell-mod/ko/) ·
[Installation](https://myso-kr.github.io/fell-and-sell-mod/INSTALLATION.html)

Unofficial fan patch. Code/authored contributions: MIT; bundled Noto font: SIL OFL 1.1.
Game and loader binaries are not included.
"""


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--output", type=Path, required=True)
    args = parser.parse_args()
    args.output.parent.mkdir(parents=True, exist_ok=True)
    args.output.write_text(compose(), encoding="utf-8", newline="\n")
