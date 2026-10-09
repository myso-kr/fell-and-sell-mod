# Contributing

Translation corrections, layout reports and focused code fixes are welcome.
Use [Issues](https://github.com/myso-kr/fell-and-sell-mod/issues) or a pull request.
Korean and English reports are both welcome.

## Translation changes

Edit values in `locale/ko/strings.json`; keep `Game/<numeric ID>` and
`UI/<numeric ID>` keys unchanged. Read [the glossary](docs/TRANSLATION.md).
Use English as the semantic baseline and Japanese as additional context, not as
an unquestioned source. Keep numbers, signs, percentages, line breaks where
meaningful, rich text tags, key labels and case-sensitive placeholders intact.
Furniture recipes should match their furniture names.

Describe the affected screen, old meaning, new meaning and any screenshot evidence.
If you cannot extract the source locally, submit the correction and say so; the
maintainer can run the full token/coverage check. Do not add game source strings,
game binaries, generated interop assemblies or unlicensed screenshots/assets to PRs.

## Code changes and validation

Follow [development](docs/DEVELOPMENT.md) and [directory conventions](docs/CONVENTIONS.md).
Keep hooks separate from the entry point. Explain the problem, resulting behavior
and validation. For a gameplay-sensitive hook change, report the exact game build
and in-game evidence, distinguishing log checks from visual review.

```powershell
python tools/check-repository.py
python -m unittest discover -s tests
pwsh -NoProfile -File tools/verify.ps1
pwsh -NoProfile -File tools/package.ps1
python tools/check-package.py
```

With a local extraction, also run:

```powershell
python tools/check-translations.py --require-complete
```

Fixture tests and compilation do not require the game. CI cannot verify source
coverage without locally extracted tables or exercise a retail game session.
CI uploads a verified package for review on each successful run; see
[downloads](docs/DOWNLOADS.md). Downloading Actions artifacts requires GitHub sign-in.
The Release workflow prepares a draft from a version tag; publishing the draft is
a separate maintainer action. Update `release-metadata.json` when verified game
metadata, translation counts or glyph counts change.

Do not commit `generated/`, `dist/`, local logs, credentials or game files.

## Review and licensing

Keep a PR focused and describe any remaining limitations. Be respectful and
address the change rather than its author. By submitting your own contributions,
you agree to provide them under the project's MIT license, subject to underlying
game-content rights. Third-party assets require a documented source and compatible
license; preserve their notices. Report security issues privately as described in
[SECURITY.md](SECURITY.md).
