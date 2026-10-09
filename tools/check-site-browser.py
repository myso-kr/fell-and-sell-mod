"""Exercise every canonical page at mobile/desktop widths in both color schemes."""
import argparse
import json
import mimetypes
from pathlib import Path
from urllib.parse import urlsplit

from playwright.sync_api import sync_playwright

ROOT = Path(__file__).resolve().parents[1]
BASE = "/fell-and-sell-mod"


def check(site, channel=None, screenshots=None):
    routes = json.loads((ROOT / "docs/_data/pages.json").read_text())
    pages = [(id, lang, route) for id, pair in routes.items() for lang, route in pair.items()]
    pages.append(("not_found", "en", "/404.html"))
    failures, views = [], 0
    if screenshots:
        screenshots.mkdir(parents=True, exist_ok=True)
    with sync_playwright() as p:
        browser = p.chromium.launch(**({"channel": channel} if channel else {}))

        def serve(route):
            request = urlsplit(route.request.url)
            if request.netloc != "preview.test" or not request.path.startswith(BASE + "/"):
                route.abort()
                return
            target = site / request.path[len(BASE) + 1:]
            if target.is_dir():
                target /= "index.html"
            if target.is_file():
                route.fulfill(body=target.read_bytes(), content_type=mimetypes.guess_type(target.name)[0] or "application/octet-stream")
            else:
                route.fulfill(status=404, body="Not found")

        for id, lang, route in pages:
            for scheme in ("light", "dark"):
                for width in (320, 390, 1280):
                    views += 1
                    label = f"{route} {width}px {scheme}"
                    page = browser.new_page(viewport={"width": width, "height": 844}, color_scheme=scheme)
                    page.route("**/*", serve)
                    errors = []
                    page.on("pageerror", lambda error: errors.append(str(error)))
                    page.on("response", lambda response: errors.append(f"HTTP {response.status}: {response.url}") if response.status >= 400 else None)
                    page.goto("http://preview.test" + BASE + route)
                    if page.locator("html").get_attribute("lang") != lang:
                        errors.append("wrong document language")
                    if page.locator("main h1").count() != 1:
                        errors.append("missing or duplicate H1")
                    if page.evaluate("document.documentElement.scrollWidth > innerWidth"):
                        errors.append("horizontal page overflow")
                    if page.locator("a").evaluate_all("xs => xs.some(x => !x.textContent.trim() && !x.getAttribute('aria-label'))"):
                        errors.append("unnamed link")
                    if id == "downloads":
                        cta = page.locator(".download-cta").bounding_box()
                        if not cta or cta["y"] + cta["height"] > 844:
                            errors.append("download action below first screen")
                    if screenshots and id in ("home", "downloads", "troubleshooting", "glossary") and width in (390, 1280):
                        page.screenshot(path=str(screenshots / f"{id}-{lang}-{width}-{scheme}.png"), full_page=True)
                    page.keyboard.press("Tab")
                    if not page.locator(".skip").evaluate("x => x === document.activeElement"):
                        errors.append("first Tab does not reach skip link")
                    page.keyboard.press("Enter")
                    if not page.locator("main").evaluate("x => x === document.activeElement"):
                        errors.append("skip link does not focus main")
                    # Test disclosure using only the keyboard, including overflow while open.
                    for disclosure in page.locator("details").all():
                        disclosure.locator("summary").focus()
                        page.keyboard.press("Enter")
                        if disclosure.get_attribute("open") is None:
                            errors.append("keyboard disclosure failed")
                        if page.evaluate("document.documentElement.scrollWidth > innerWidth"):
                            errors.append("expanded disclosure overflows page")
                    # Exercise counterpart navigation rather than just inspecting hrefs.
                    if id in routes and scheme == "light" and width == 390:
                        other = "ko" if lang == "en" else "en"
                        page.locator(f".languages a[lang={other}]").click()
                        if page.locator("body").get_attribute("data-page-id") != id or page.locator("html").get_attribute("lang") != other:
                            errors.append("language switch loses current guide")
                    failures.extend(f"{label}: {error}" for error in errors)
                    page.close()
        # User path: language home -> download -> install -> features -> help.
        for lang in ("en", "ko"):
            page = browser.new_page(viewport={"width": 390, "height": 844})
            page.route("**/*", serve)
            page.goto("http://preview.test" + BASE + routes["home"][lang])
            for id in ("downloads", "installation", "features", "troubleshooting"):
                page.locator(f'.primary-nav a[href="{BASE}{routes[id][lang]}"]').click()
                if page.locator("body").get_attribute("data-page-id") != id:
                    failures.append(f"{lang}: user path failed at {id}")
            page.close()
        aliases = {"DOWNLOAD.html": "downloads", "DOWNLOADS.html": "downloads", "INSTALLATION.html": "installation", "TROUBLESHOOTING.html": "troubleshooting", "TRANSLATION.html": "glossary"}
        for alias, id in aliases.items():
            page = browser.new_page()
            page.route("**/*", serve)
            page.goto("http://preview.test" + BASE + "/" + alias)
            page.wait_for_url("**" + BASE + routes[id]["en"])
            if page.locator("body").get_attribute("data-page-id") != id:
                failures.append(f"{alias}: wrong redirect destination")
            page.close()
        browser.close()
    if failures:
        raise SystemExit("\n".join(failures))
    print(f"[OK] {views} browser views: layout, first-screen CTA, keyboard, language switching, two user flows and five redirects")


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("site", type=Path)
    parser.add_argument("--channel", help="Use an installed browser, e.g. msedge")
    parser.add_argument("--screenshots", type=Path)
    args = parser.parse_args()
    check(args.site.resolve(), args.channel, args.screenshots)
