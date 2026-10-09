---
layout: default
title: "Fell & Sell 한국어 패치"
lang: ko
permalink: /ko/guide/
description: "Fell & Sell 한국어 패치"
---

# Fell & Sell 한국어 패치

Steam판 **Fell & Sell**을 한국어로 표시하는 MelonLoader 기반 비공식 모드입니다.

[한국어 웹사이트](https://myso-kr.github.io/fell-and-sell-mod/ko/) · [영어 README](https://github.com/myso-kr/fell-and-sell-mod/blob/main/README.md) · [다운로드](https://github.com/myso-kr/fell-and-sell-mod/releases) ·
[변경 이력](https://github.com/myso-kr/fell-and-sell-mod/blob/main/CHANGELOG.md) · [문제 신고](https://github.com/myso-kr/fell-and-sell-mod/issues)

## 번역 범위와 검증 상태

v0.4.0은 추출된 번역 테이블 **1,243개 항목 전체**를 다룹니다.
Game 테이블 1,128개와 UI 테이블 115개로, 아이템·제작법·효과·튜토리얼·대화·퀘스트를
포함합니다. 영어 원문을 기준으로 일본어 번역과 항목 ID를 비교해 맥락을 확인했습니다.
고유명사와 기호만 있는 항목은 필요한 경우 그대로 유지했습니다.

Windows 11, 게임 1.7.1 / Steam 빌드 25480096, MelonLoader 0.7.3 x64에서
번역 로딩과 훅 실행을 확인했습니다. 한글 글리프 699개는 누락이 없으며 한국어 TMP
텍스트 컴포넌트 20개가 확인됐습니다. 2026-10-09에 프로젝트 소유자가 인게임 텍스트
표시 검수 완료를 확인했습니다. 전반적인 플레이·일본어 전환·제거 테스트는 별도
확인 항목입니다. 번역 테이블 밖의 문구와 이후 게임 업데이트는 추가 작업이 필요할 수 있습니다.

## 탐험 도우미 프리뷰

v0.4.0에는 주변 자동줍기·기존 지도 확장·경로 표시·자동 이동 코드가 추가됐습니다.
새 기능은 기본 꺼짐이며 지도 표시·목적지 선택·우클릭 후 계단 안내 복귀는 확인됐습니다. 빛 이동·투명도도 확인됐습니다. 자동줍기·자동 이동은 검증 중입니다.
F8 설정, F9 지도, F10 이동/정지를 사용합니다. 게임의 도전과제 처리는 유지합니다.
[상세 조작과 범위](EXPANSION.md)를 확인하세요.

## 설치

1. Steam에서 **Fell & Sell → 속성 → 설치된 파일 → 찾아보기**를 엽니다.
   `Fell & Sell.exe`가 있는 폴더가 설치 대상입니다.
2. [MelonLoader 0.7.3 x64](https://github.com/LavaGang/MelonLoader/releases/tag/v0.7.3)를
   해당 폴더에 설치합니다. 로더는 패치 ZIP에 포함하지 않습니다.
3. [Releases](https://github.com/myso-kr/fell-and-sell-mod/releases)에 배포본이 올라오면
   `fell-and-sell-mod-v0.4.0.zip`을 받아 게임 폴더에 압축을 풉니다.
   GitHub의 Source code ZIP은 설치용 패치가 아닙니다. 배포본이 아직 없다면
   [CI 빌드](https://github.com/myso-kr/fell-and-sell-mod/actions/workflows/ci.yml)의
   성공한 실행에서 아티팩트를 받을 수 있습니다. GitHub 로그인이 필요하며 보관 기간은
   30일입니다. 바깥 아티팩트 ZIP을 먼저 풀고, 안에 있는 패치 ZIP을 게임 폴더에 풉니다.
   [다운로드 안내](../DOWNLOADS.md)에 자세한 방법이 있습니다.
4. **Steam으로 게임을 실행**합니다. 한국어 번역이 자동 적용됩니다.
   게임 언어 메뉴에는 한국어 항목이 따로 추가되지 않습니다. 영어 또는 일본어를 선택해도
   같은 한국어 카탈로그가 적용되며, 새로 추가된 미등록 항목은 선택한 원래 언어로 표시됩니다.

```text
Fell & Sell/
├─ Mods/FellAndSellMod.dll
└─ UserData/FellAndSell/
   ├─ locale/ko/strings.json
   └─ fonts/
      ├─ NotoSansCJKkr-Regular.otf
      └─ OFL-Noto.txt
```

자세한 체크섬 확인·설치·업데이트 방법은 [설치 문서](../INSTALLATION.md)를 참고하세요.

## 업데이트와 제거

게임을 종료한 뒤 새 ZIP의 DLL·번역 파일·글꼴을 덮어씁니다.
제거하려면 `Mods/FellAndSellMod.dll`과 `UserData/FellAndSell/`만 삭제하세요.
다른 모드 파일은 유지합니다. 이 패치는 원본 게임 바이너리를 수정하거나 세이브를 편집하지
않습니다. MelonLoader 제거는 로더의 안내를 따르세요.

## 문제가 생기면

`MelonLoader/Latest.log`에서 다음 내용을 확인하세요.

- `Fell & Sell Korean Patch v0.4.0`
- `i18n: loaded 1243 Korean entries`
- `i18n: installed raw and formatted string hooks`
- `font: verified 699 Hangul glyphs; missing=0`

글자가 네모로 나오거나 영어가 남는 경우 [문제 해결 안내](../TROUBLESHOOTING.md)를
확인하세요. 어색한 번역과 잘리는 문구도 [이슈](https://github.com/myso-kr/fell-and-sell-mod/issues)로
알려주세요. 게임·모드 버전, 화면 이름, 재현 방법, 관련 로그나 스크린샷을 함께 보내면
확인하기 쉽습니다. 로그를 공개하기 전에 개인 경로와 민감한 정보를 가려주세요.

## Actions 빌드와 릴리스

CI는 게임 설치 없이 검사·DLL 빌드·패키징을 수행하고 ZIP·SHA-256·릴리스 노트를
보관합니다. 버전 태그의 Release 워크플로는 같은 빌드를 수행한 뒤 패키지를 릴리스
초안에 첨부합니다. 초안은 일반 사용자에게 공개되지 않으며 검토 후 게시합니다.
CI 성공은 모든 게임 화면의 플레이 검수를 의미하지 않습니다.

## 번역 수정에 참여하기

`locale/ko/strings.json`의 값을 수정하고 PR을 보내주세요. 숫자 ID 키, 자리표시자,
서식 태그와 버튼 토큰을 유지해야 합니다. [번역 기준](../TRANSLATION.md)과
[기여 안내](https://github.com/myso-kr/fell-and-sell-mod/blob/main/CONTRIBUTING.md)에 용어와 검사 방법이 있습니다.

## 저작권과 라이선스

myso-kr의 비공식 팬 프로젝트로, 게임 개발사·배급사의 공식 패치가 아닙니다.
프로젝트 코드와 작성한 번역 기여분은 원본 게임 콘텐츠의 권리를 전제로
[MIT](https://github.com/myso-kr/fell-and-sell-mod/blob/main/LICENSE)로 제공합니다. 게임명과 원본 콘텐츠의 권리는 각 권리자에게 있습니다.
동봉한 Noto Sans CJK KR 글꼴은 SIL OFL 1.1이며 라이선스 문서를 함께 배포합니다.
[고지](https://github.com/myso-kr/fell-and-sell-mod/blob/main/NOTICE) · [외부 구성 요소](https://github.com/myso-kr/fell-and-sell-mod/blob/main/THIRD-PARTY.md)
