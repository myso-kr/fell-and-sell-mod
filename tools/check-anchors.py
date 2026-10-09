"""Check authored runtime anchors as metadata only; never load or execute game code."""
import argparse
import json
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]


def check(assemblies):
    import dnfile
    errors = []
    for assembly, types in json.loads((ROOT / ".spec/anchors.json").read_text(encoding="utf-8")).items():
        path = assemblies / (assembly + ".dll")
        if not path.is_file():
            errors.append(f"Missing installed interop assembly: {path}")
            continue
        pe = dnfile.dnPE(str(path))
        try:
            rows = {f"{row.TypeNamespace}.{row.TypeName}": row for row in pe.net.mdtables.TypeDef.rows}
            for name, members in types.items():
                row = rows.get(name)
                if row is None:
                    errors.append(f"Missing type: {name}")
                    continue
                methods = {str(item.row.Name) for item in row.MethodList}
                fields = {str(item.row.Name) for item in row.FieldList}
                for member in members:
                    # Generated IL2CPP fields are managed properties with get_/set_.
                    if member not in methods | fields and "get_" + member not in methods:
                        errors.append(f"Missing anchor: {name}.{member}")
        finally:
            pe.close()
    return errors


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--assemblies", type=Path)
    args = parser.parse_args()
    if not args.assemblies:
        print("[SKIP] No installed interop path supplied; runtime anchors were not checked")
    else:
        errors = check(args.assemblies)
        if errors:
            raise SystemExit("\n".join(errors))
        print("[OK] Installed metadata anchors found; runtime behaviour remains unverified")
