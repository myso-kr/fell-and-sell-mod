---
title: "한국어 패치"
description: "Fell & Sell 한국어 패치. 전체 1,243개 번역 항목과 설치·문제 해결 안내."
lang: ko
home: true
permalink: /ko/
---

<p class="eyebrow">Fell &amp; Sell · 비공식 한국어 패치</p>

# 모험에서 상점까지,<br>한국어로 만나세요.

<p class="lead">아이템과 제작법, 퀘스트와 대화를 한국어로. Steam판 Fell &amp; Sell을 위한 MelonLoader 기반 언어 모드입니다.</p>

<div class="actions">
<a class="button" href="https://github.com/myso-kr/fell-and-sell-mod/releases">릴리스 확인</a>
<a class="button secondary" href="{{ '/ko/guide/' | relative_url }}">설치 안내</a>
</div>

<dl class="ledger">
<div><dt>번역 항목</dt><dd>1,243 / 1,243</dd></div>
<div><dt>패치 버전</dt><dd>0.3.0</dd></div>
<div><dt>검증한 게임 버전</dt><dd>1.7.1</dd></div>
</dl>

<div class="note" markdown="1">
**전체 번역 테이블을 다루며, 화면 검수는 진행 중입니다.** 추출한 Game·UI 항목을
모두 번역했고 한글 글리프 699개는 누락 없이 확인했습니다. 모든 플레이 화면의
줄바꿈과 배치는 아직 검수하지 못했습니다. 테이블 밖의 문구는 추가 작업이 필요할 수 있습니다.
</div>

## 전리품부터 상점 진열대까지

Game 항목 1,128개와 UI 항목 115개를 번역했습니다. 장비·가구·제작법·전투 효과·
튜토리얼·대화·퀘스트가 포함됩니다. 영어 원문을 기준으로 일본어 번역을 함께 비교해
맥락을 확인했고, 가구와 제작법 이름을 일치시켰습니다.

게임의 기본 언어를 영어 또는 일본어로 선택해도 한국어가 자동 적용됩니다.
언어 메뉴에 한국어 항목이 따로 추가되지는 않습니다. 이후 업데이트로 추가된 미등록
항목은 선택한 원래 언어로 표시될 수 있습니다.

## 세 단계로 설치하기

1. Steam의 설치된 파일 메뉴에서 `Fell & Sell.exe`가 있는 폴더를 엽니다.
2. [MelonLoader 0.7.3 x64](https://github.com/LavaGang/MelonLoader/releases/tag/v0.7.3)를 별도로 설치합니다.
3. 패치 ZIP을 게임 폴더에 풀고 Steam으로 실행합니다.

```text
Fell & Sell/
├─ Mods/FellAndSellMod.dll
└─ UserData/FellAndSell/
   ├─ locale/ko/strings.json
   └─ fonts/NotoSansCJKkr-Regular.otf + OFL-Noto.txt
```

GitHub의 Source code ZIP은 설치용 패치가 아닙니다. 배포본이 아직 없다면
[개발 안내](../DEVELOPMENT.md)에서 빌드 방법을 확인하세요.
[한국어 설치·제거 안내](README.md)와 [체크섬 확인 방법](../INSTALLATION.md)도 제공합니다.

## 확인한 범위

| 항목 | 확인 결과 |
|---|---|
| 게임 | 1.7.1 / Steam 빌드 25480096, Windows 11 |
| 실행 환경 | Unity 6000.3.10f1 IL2CPP / MelonLoader 0.7.3 x64 |
| 번역과 서식 | 1,243개 항목, 누락·토큰 검사 통과 |
| 글꼴 | 한글 글리프 699개, 누락 0개 |
| 남은 검수 | 모든 화면 배치, 플레이 검수, 일본어 전환 |

원본 게임 바이너리는 수정하지 않습니다. 번역은 실행 중 메모리에서 적용되며 세이브
편집 기능은 없습니다. 게임을 종료하고 모드 DLL과 `UserData/FellAndSell/`을 삭제하면
한국어 패치를 제거할 수 있습니다.

## 어색한 번역도 알려주세요

뜻이 틀리거나 버튼 밖으로 문구가 나오는 경우
[이슈](https://github.com/myso-kr/fell-and-sell-mod/issues)로 알려주세요.
화면 이름, 게임·패치 버전, 스크린샷이나 관련 로그를 함께 보내면 확인에 도움이 됩니다.

[한국어 안내](README.md) · [전체 문서](../README.md) ·
[문제 해결](../TROUBLESHOOTING.md) · [번역 용어집](../TRANSLATION.md)
