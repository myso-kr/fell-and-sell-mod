"""Extract Unity Localization English tables without modifying game files."""

import argparse
import json
from pathlib import Path

import UnityPy


def read_tables(bundle: Path) -> list[dict]:
    return [
        obj.read_typetree()
        for obj in UnityPy.load(str(bundle)).objects
        if obj.type.name == "MonoBehaviour"
    ]


def extract(game_dir: Path, output: Path) -> dict:
    bundles = game_dir / "Fell & Sell_Data/StreamingAssets/aa/StandaloneWindows64"
    shared_tables = read_tables(bundles / "localization-assets-shared_assets_all.bundle")
    english_tables = read_tables(bundles / "localization-string-tables-english_assets_all.bundle")
    shared = {
        table["m_TableCollectionName"]: {
            entry["m_Id"]: entry["m_Key"] for entry in table["m_Entries"]
        }
        for table in shared_tables
        if "m_TableCollectionName" in table
    }
    strings = {}
    counts = {}
    missing_keys = []
    for table in english_tables:
        if "m_TableData" not in table:
            continue
        if table["m_LocaleId"]["m_Code"] != "en":
            raise ValueError("Expected an English table")
        collection = table["m_Name"].removesuffix("_en")
        counts[collection] = 0
        for entry in table["m_TableData"]:
            entry_id = str(entry["m_Id"])
            stable_key = f"{collection}/{entry_id}"
            if stable_key in strings:
                raise ValueError(f"Duplicate entry: {stable_key}")
            name = shared[collection].get(entry["m_Id"])
            if name is None:
                missing_keys.append(stable_key)
            strings[stable_key] = {"key": name, "text": entry["m_Localized"]}
            counts[collection] += 1
    if not strings:
        raise ValueError("No English entries found")
    output.mkdir(parents=True, exist_ok=True)
    report = {"counts": counts, "total": len(strings), "missing_shared_keys": missing_keys}
    for filename, data in (("strings.en.json", strings), ("extraction-report.json", report)):
        (output / filename).write_text(
            json.dumps(data, ensure_ascii=False, indent=2) + "\n", encoding="utf-8"
        )
    return report


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--game-dir", type=Path, required=True)
    parser.add_argument("--output", type=Path, default=Path(__file__).resolve().parents[1] / "generated")
    arguments = parser.parse_args()
    print(json.dumps(extract(arguments.game_dir, arguments.output), indent=2))
