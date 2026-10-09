"""Validate a rendered Pages site, including base-path links and metadata."""
import argparse
from html.parser import HTMLParser
from pathlib import Path
from urllib.parse import unquote, urlsplit


class Page(HTMLParser):
    def __init__(self):
        super().__init__()
        self.links = []
        self.ids = set()
        self.lang = None
        self.canonical = None
        self.main = False

    def handle_starttag(self, tag, attrs):
        attrs = dict(attrs)
        if "id" in attrs:
            self.ids.add(attrs["id"])
        if tag == "html":
            self.lang = attrs.get("lang")
        if tag == "main":
            self.main = True
        if tag == "link" and attrs.get("rel") == "canonical":
            self.canonical = attrs.get("href")
        if tag in ("a", "link") and "href" in attrs:
            self.links.append(attrs["href"])
        if tag in ("img", "script") and "src" in attrs:
            self.links.append(attrs["src"])


def check(root: Path, base: str) -> list[str]:
    pages = {}
    errors = []
    for path in root.rglob("*.html"):
        relative = path.relative_to(root)
        if path.stem.upper() in {"ANCHORS", "CONVENTIONS", "DEVELOPMENT", "GAME-SURVEY", "PAGES", "PLAN", "RELEASING", "STATUS"} or ".spec" in relative.parts:
            errors.append(f"Technical record published as a player page: {relative}")
        page = Page()
        page.feed(path.read_text(encoding="utf-8"))
        pages[path.resolve()] = page
        if page.lang not in ("en", "ko") or not page.canonical or not page.main:
            errors.append(f"{path}: missing language, canonical or main element")
        elif page.lang != ("ko" if relative.parts[0] == "ko" else "en"):
            errors.append(f"{relative}: page language disagrees with its public route")
    for path, page in pages.items():
        for link in page.links:
            url = urlsplit(link)
            if url.scheme or url.netloc:
                continue
            part = unquote(url.path)
            if part.startswith("/"):
                if not part.startswith(base + "/"):
                    errors.append(f"{path.name}: link bypasses project base path: {link}")
                    continue
                target = root / part[len(base) + 1:]
            else:
                target = path.parent / part if part else path
            if target.is_dir():
                target = target / "index.html"
            target = target.resolve()
            if not target.exists():
                errors.append(f"{path.name}: missing link target: {link}")
            elif url.fragment and target in pages and unquote(url.fragment) not in pages[target].ids:
                errors.append(f"{path.name}: missing fragment: {link}")
    for required in ("index.html", "ko/index.html", "guide/index.html", "ko/guide/index.html", "features/index.html", "ko/features/index.html", "404.html"):
        if not (root / required).exists():
            errors.append(f"Missing required page: {required}")
    if not pages:
        errors.append("No HTML pages found")
    return errors


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("site", type=Path)
    parser.add_argument("--base", default="/fell-and-sell-mod")
    args = parser.parse_args()
    errors = check(args.site.resolve(), args.base.rstrip("/"))
    if errors:
        raise SystemExit("\n".join(errors))
    print("[OK] Rendered site links, fragments, language and canonical metadata verified")
