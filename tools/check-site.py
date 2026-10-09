"""Validate all rendered player pages, locale pairs, navigation and legacy routes."""
import argparse
import json
from html.parser import HTMLParser
from pathlib import Path
from urllib.parse import unquote, urlsplit

ROOT = Path(__file__).resolve().parents[1]
DATA = ROOT / "docs/_data"
LEGACY = {"DOWNLOAD.html": "/downloads/", "DOWNLOADS.html": "/downloads/",
          "INSTALLATION.html": "/installation/", "TROUBLESHOOTING.html": "/help/",
          "TRANSLATION.html": "/glossary/"}


class Page(HTMLParser):
    def __init__(self):
        super().__init__()
        self.links, self.headings, self.nav, self.languages = [], [], [], []
        self.ids, self.alternates, self.features = set(), {}, {}
        self.lang = self.canonical = self.description = self.page_id = self.refresh = None
        self.main = self.skip = self.cta = False
        self.nav_class = None
        self.version = ""
        self.in_version = False
        self.text = []

    def handle_starttag(self, tag, attrs):
        a = dict(attrs)
        if "id" in a:
            self.ids.add(a["id"])
        if tag == "html":
            self.lang = a.get("lang")
        if tag == "body":
            self.page_id = a.get("data-page-id")
        if tag == "main":
            self.main = a.get("id") == "content" and a.get("tabindex") == "-1"
        if tag == "nav":
            self.nav_class = a.get("class")
        if tag == "meta" and a.get("name") == "description":
            self.description = a.get("content")
        if tag == "meta" and a.get("http-equiv") == "refresh":
            self.refresh = a.get("content", "").split("url=")[-1]
        if tag == "link" and a.get("rel") == "canonical":
            self.canonical = a.get("href")
        if tag == "link" and a.get("hreflang"):
            self.alternates[a["hreflang"]] = a.get("href")
        if tag in ("h1", "h2", "h3", "h4", "h5", "h6"):
            self.headings.append(int(tag[1]))
        if tag == "a":
            if a.get("class") == "skip":
                self.skip = a.get("href") == "#content"
            if "download-cta" in a.get("class", "").split():
                self.cta = a.get("href") == json.loads((DATA / "patch.json").read_text())["builds_url"]
            if self.nav_class == "primary-nav":
                self.nav.append(a)
            if self.nav_class == "languages":
                self.languages.append(a)
        if "data-feature" in a:
            self.features[a["data-feature"]] = a.get("data-status")
        if "data-patch-version" in a:
            self.in_version = True
        if tag in ("a", "link") and "href" in a:
            self.links.append(a["href"])
        if tag in ("img", "script") and "src" in a:
            self.links.append(a["src"])

    def handle_endtag(self, tag):
        if tag == "nav":
            self.nav_class = None
        if tag == "strong":
            self.in_version = False

    def handle_data(self, data):
        self.text.append(data)
        if self.in_version:
            self.version += data.strip()


def check(root: Path, base: str) -> list[str]:
    routes = json.loads((DATA / "pages.json").read_text())
    patch = json.loads((DATA / "patch.json").read_text())
    host = "https://myso-kr.github.io"
    pages, errors = {}, []
    def report(path, message):
        errors.append(f"{path.relative_to(root)}: {message}")
    def resolve(link, path):
        url = urlsplit(link)
        if url.scheme or url.netloc:
            if url.netloc != urlsplit(host).netloc:
                return None
        part = unquote(url.path)
        if part.startswith("/"):
            if not part.startswith(base + "/"):
                report(path, f"link bypasses project base path: {link}")
                return None
            target = root / part[len(base) + 1:]
        else:
            target = path.parent / part if part else path
        if target.is_dir() or part.endswith("/"):
            target /= "index.html"
        return target.resolve()
    for path in root.rglob("*.html"):
        path = path.resolve()
        page = Page()
        page.feed(path.read_text(encoding="utf-8"))
        pages[path] = page
        relative = path.relative_to(root)
        if path.stem.upper() in {"ANCHORS", "ARCHITECTURE", "CONVENTIONS", "DEVELOPMENT", "GAME-SURVEY", "PAGES", "PLAN", "RELEASING", "RUNBOOK", "STATUS"} or ".spec" in relative.parts:
            report(path, "technical record published as player page")
        if page.lang != ("ko" if relative.parts[0] == "ko" else "en"):
            report(path, "language disagrees with public route")
        if not page.description or not page.main or not page.skip:
            report(path, "missing description, focusable main or skip link")
        if page.headings.count(1) != 1 or not page.headings or page.headings[0] != 1:
            report(path, "expected one initial H1")
        if any(b > a + 1 for a, b in zip(page.headings, page.headings[1:])):
            report(path, "heading level skipped")
        canonical_route = LEGACY.get(relative.as_posix())
        if canonical_route is None:
            canonical_route = "/" + relative.as_posix().removesuffix("index.html")
        if page.canonical != host + base + canonical_route:
            report(path, "canonical differs from public route or redirect target")
        if relative.as_posix() in LEGACY and page.refresh != base + canonical_route:
            report(path, "legacy URL does not redirect to canonical target")
        if page.page_id in routes:
            pair = routes[page.page_id]
            expected_alternates = {lang: host + base + route for lang, route in pair.items()}
            expected_alternates["x-default"] = host + base + pair["en"]
            if page.alternates != expected_alternates:
                report(path, "incorrect language alternates")
            if {a.get("lang"): a.get("href") for a in page.languages} != {lang: base + route for lang, route in pair.items()}:
                report(path, "language navigation does not preserve page")
        expected_nav = [base + routes[id][page.lang] for id in ("downloads", "installation", "guide", "features", "troubleshooting")]
        if [a.get("href") for a in page.nav] != expected_nav:
            report(path, "missing, duplicated or misordered main navigation")
        current = [a.get("href") for a in page.nav if a.get("aria-current") == "page"]
        own = base + routes[page.page_id][page.lang] if page.page_id in routes else None
        if current != ([own] if own in expected_nav else []):
            report(path, "incorrect active main navigation")
        if [a.get("lang") for a in page.languages if a.get("aria-current") == "page"] != [page.lang]:
            report(path, "incorrect active language")
        if page.version != patch["version"]:
            report(path, "stale public patch version")
        if relative.as_posix() not in LEGACY:
            if page.page_id == "downloads" and not page.cta:
                report(path, "missing usable preview download action")
            if page.page_id in ("home", "features") and page.features != {f["id"]: f["status"] for f in patch["features"]}:
                report(path, "feature status differs from shared public data")
        public_text = " ".join(page.text)
        for marker in (".spec/", "TMP Canvas", "OnSceneWasLoaded", "BeginGeneration", "699 Hangul", "missing=0"):
            if marker in public_text:
                report(path, f"technical record leaked into public page: {marker}")
    for path, page in pages.items():
        for link in page.links:
            url = urlsplit(link)
            target = resolve(link, path)
            if target is None:
                continue
            if not target.exists():
                report(path, f"missing link target: {link}")
            elif url.fragment and target in pages and unquote(url.fragment) not in pages[target].ids:
                report(path, f"missing fragment: {link}")
    required = [route for pair in routes.values() for route in pair.values()] + ["/404.html"] + ["/" + name for name in LEGACY]
    for route in required:
        target = root / route.lstrip("/")
        if route.endswith("/"):
            target /= "index.html"
        if not target.exists():
            errors.append(f"Missing required page: {route}")
    if len(pages) != len(required):
        errors.append(f"Expected {len(required)} player/redirect pages, found {len(pages)}")
    return errors


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("site", type=Path)
    parser.add_argument("--base", default="/fell-and-sell-mod")
    args = parser.parse_args()
    errors = check(args.site.resolve(), args.base.rstrip("/"))
    if errors:
        raise SystemExit("\n".join(errors))
    print("[OK] All 20 pages: links, headings, locale pairs, navigation, public status and redirects")
