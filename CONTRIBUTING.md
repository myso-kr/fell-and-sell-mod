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

## Code contributions

Describe the problem, resulting behaviour and evidence. Include the game version for gameplay changes, and distinguish a successful build from an in-game check.

Contributor instructions, source conventions and validation commands live in the [development guide](https://github.com/myso-kr/fell-and-sell-mod/blob/main/.spec/DEVELOPMENT.md) and [conventions](https://github.com/myso-kr/fell-and-sell-mod/blob/main/.spec/CONVENTIONS.md).

Keep generated output, local logs, credentials and game binaries out of pull requests.

## Review and licensing

Keep a PR focused and describe any remaining limitations. Be respectful and
address the change rather than its author. By submitting your own contributions,
you agree to provide them under the project's MIT license, subject to underlying
game-content rights. Third-party assets require a documented source and compatible
license; preserve their notices. Report security issues privately as described in
[SECURITY.md](SECURITY.md).
