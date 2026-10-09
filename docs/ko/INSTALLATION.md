---
layout: default
title: "설치·업데이트·제거"
description: "MelonLoader와 한국어 패치 전체를 설치하고, 업데이트하거나 패치 파일만 제거하는 방법입니다."
lang: "ko"
permalink: "/ko/installation/"
page_id: "installation"
page_type: "article"
summary: "로더를 먼저 설치한 뒤 패치 전체를 게임 폴더에 넣으세요."
---

<nav class="toc" aria-label="이 페이지에서"><p>이 페이지에서</p><ul><li><a href="#requirements">준비물</a></li><li><a href="#install">설치</a></li><li><a href="#update">업데이트</a></li><li><a href="#remove">제거</a></li></ul></nav>

## 준비물 {#requirements}

- Steam에 설치한 Windows용 Fell & Sell 게임.
- [MelonLoader {{ site.data.patch.loader_version }} x64]({{ site.data.patch.loader_url }})와 해당 로더의 필수 구성요소.
- [다운로드 안내]({{ '/ko/downloads/' | relative_url }})에서 받은 패키지 ZIP.

확인한 환경은 {{ site.data.patch.platform }}, 게임 {{ site.data.patch.game_version }} / Steam 빌드 {{ site.data.patch.steam_build }}입니다. 다른 게임·로더 버전과 운영체제는 확인하지 않았습니다.

## 설치 {#install}

1. 게임을 종료하세요. Steam에서 **속성 → 설치된 파일 → 찾아보기**를 선택하세요.
2. MelonLoader 안내에 따라 `Fell & Sell.exe`가 있는 폴더에 x64 로더를 설치하세요.
3. 안쪽 패키지 ZIP을 같은 폴더에 푸세요. `Mods`와 `UserData`가 게임 폴더 바로 아래에 있어야 하며, 패키지 폴더가 한 겹 더 생기지 않도록 주의하세요.
4. 다음 파일을 확인하세요.
   - `Mods/FellAndSellMod.dll`
   - `UserData/FellAndSell/locale/ko/strings.json`
   - `UserData/FellAndSell/locale/ko/mod-ui.json`
   - `UserData/FellAndSell/fonts/NotoSansCJKkr-Regular.otf`와 `OFL-Noto.txt`
5. Steam에서 게임을 실행하세요. 로더가 필요한 파일을 준비하는 첫 실행은 시간이 더 걸릴 수 있습니다.
6. 한국어가 자동 적용됩니다. <kbd>F8</kbd>에서 필요한 [탐험 기능]({{ '/ko/features/' | relative_url }})을 켠 뒤 설정창을 닫고 플레이하세요.

별도의 한국어 언어 메뉴나 글자 크기 설정은 없습니다. 번역 파일을 바꿨다면 재실행하세요. 한글이 보이지 않으면 [문제 해결]({{ '/ko/help/' | relative_url }})을 확인하세요.

<details markdown="1">
<summary>선택 사항: 패키지 체크섬 확인</summary>

패키지 ZIP 옆에 `.zip.sha256`을 두고, 해당 폴더의 PowerShell에서 실행하세요.

```powershell
$archive = 'fell-and-sell-mod-v{{ site.data.patch.version }}.zip'
$expected = (Get-Content ($archive + '.sha256') -Raw).Split()[0]
$actual = (Get-FileHash -LiteralPath $archive -Algorithm SHA256).Hash
if ($actual -ine $expected) { throw 'Checksum mismatch' }
```

일치하면 ZIP이 동봉된 체크섬과 맞는 것입니다. 두 파일은 같은 프로젝트 빌드나 릴리스에서 받으세요.
</details>

## 업데이트 {#update}

게임을 종료하고 새 패키지의 DLL·번역 파일·폰트로 교체하세요. 직접 수정한 번역은 먼저 별도로 백업하세요. Mods에는 `FellAndSellMod.dll`을 하나만 남기세요. 게임이나 로더 업데이트 후에는 호환성이 달라질 수 있습니다.

## 제거 {#remove}

게임을 종료하고 `Mods/FellAndSellMod.dll`과 `UserData/FellAndSell/`을 제거하세요. 다른 모드의 파일은 그대로 두세요. MelonLoader가 더 필요 없다면 로더의 안내에 따라 제거하세요. 이 패치는 원본 게임 실행 파일이나 저장 파일을 수정하지 않습니다.

[문제가 있나요?]({{ '/ko/help/' | relative_url }})
