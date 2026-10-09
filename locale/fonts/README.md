# Fonts

Noto Sans CJK KR Regular is bundled under the SIL Open Font License 1.1.
See `OFL-Noto.txt`. The font is unchanged from the Noto CJK project:

https://github.com/notofonts/noto-cjk/blob/main/Sans/OTF/Korean/NotoSansCJKkr-Regular.otf

SHA-256: `6bcb2a0703aa137e874fc2dffa85f6c21ba9a67fa329e81b8c801663af7e992a`.

The mod creates a dynamic, multi-atlas TMP fallback and registers it on newly
loaded fonts. It verifies all Hangul syllables used in the loaded catalog.
