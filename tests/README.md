# Tests

Run `python -m unittest discover -s tests` for token preservation checks.
Fixtures verify reordered placeholders, repeated tokens, rich-text tags,
button labels, unknown IDs and empty entries. No game data is required.

CI also runs `tools/check-repository.py` and checks the built ZIP with
`tools/check-package.py`. Full source coverage/token checks need the ignored
local extraction and are not replaced by these fixtures.
