"""Check authored Korean strings against locally extracted English templates."""

import argparse
from collections import Counter
import json
from pathlib import Path
import re


def tokens(text: str) -> Counter:
    return Counter(re.findall(r"\{[^{}]+\}|</?[^<>]+>|\[[A-Za-z0-9_/]+\]", text))


def check(source: dict, translations: dict) -> list[str]:
    errors = []
    for key, translation in translations.items():
        if key not in source:
            errors.append(f"Unknown key: {key}")
        elif not isinstance(translation, str) or not translation.strip():
            errors.append(f"Empty or non-string translation: {key}")
        elif tokens(source[key]["text"]) != tokens(translation):
            errors.append(f"Token mismatch: {key}")
    return errors


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--source", type=Path, default=Path("generated/strings.en.json"))
    parser.add_argument("--catalog", type=Path, default=Path("locale/ko/strings.json"))
    args = parser.parse_args()
    source = json.loads(args.source.read_text(encoding="utf-8"))
    translations = json.loads(args.catalog.read_text(encoding="utf-8"))
    errors = check(source, translations)
    if errors:
        raise SystemExit("\n".join(errors))
    print(f"[OK] {len(translations)}/{len(source)} entries; format tokens preserved")
