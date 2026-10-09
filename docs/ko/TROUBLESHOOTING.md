---
layout: "default"
title: "문제 해결"
description: "다운로드, 실행, 한글 표시, 입력, 지도, 경로와 자동줍기 문제를 확인하고 로그와 함께 제보하는 방법입니다."
lang: "ko"
permalink: "/ko/help/"
page_id: "troubleshooting"
page_type: "article"
summary: "증상에 맞는 항목을 확인하고 해결되지 않는 문제를 제보해주세요."
---

<nav class="toc" aria-label="증상 선택"><p>증상 선택</p><ul><li><a href="#download">다운로드</a></li><li><a href="#startup">게임 실행</a></li><li><a href="#text">한글 표시</a></li><li><a href="#input">조작</a></li><li><a href="#map">지도·경로</a></li><li><a href="#pickup">자동줍기</a></li><li><a href="#report">문제 제보</a></li></ul></nav>

## 다운로드·압축 해제 문제 {#download}

GitHub에 로그인하고 성공한 main 빌드를 여세요. 보관 기간이 지난 파일은 받을 수 없으므로 더 최근의 성공한 빌드를 선택하세요. 바깥 artifact ZIP을 푼 뒤 안쪽 패키지 ZIP을 푸세요. 안쪽 패키지에는 `Mods`와 `UserData`가 있습니다. 저장소 소스 ZIP은 설치할 수 없습니다. [다운로드 순서]({{ '/ko/downloads/' | relative_url }})

## 모드가 로드되지 않거나 게임이 실행되지 않음 {#startup}

올바른 게임 실행 파일 옆에 로더가 있고, Mods 바로 아래에 `FellAndSellMod.dll`이 있는지 확인하세요. Steam에서 실행하세요. 원인을 구분하려면 게임을 종료하고 이 DLL을 Mods 밖으로 잠시 옮긴 뒤 패치 없이도 같은 문제가 생기는지 확인하세요. 다른 모드를 따로 확인할 때도 파일을 보존하세요.

로더 설치 문제는 [MelonLoader 프로젝트](https://github.com/LavaGang/MelonLoader)의 안내를 참고하세요. 실행 오류를 제보할 때는 시작 로그의 첫 관련 오류를 포함하세요.

## 한글이 없거나 네모로 표시됨·문구가 잘림 {#text}

게임을 종료하고 **패키지 전체**를 다시 설치하세요. DLL만 복사하면 번역·폰트가 빠집니다. [설치 안내]({{ '/ko/installation/#install' | relative_url }})의 파일 위치를 확인하세요. 네모로 표시된다면 `UserData/FellAndSell/fonts/`의 OTF 폰트를 확인하세요.

한국어는 자동 적용되며 별도의 한국어 언어 메뉴는 없습니다. 크레딧·고유명사·구두점은 그대로 남을 수 있습니다. 범위 밖의 새 문구는 기본 언어로 나올 수 있습니다. 오역이나 잘린 문구는 화면, 표시 문장, 기대한 뜻을 함께 보내주세요. [용어집]({{ '/ko/glossary/' | relative_url }})도 참고하세요.

## F8 또는 게임 조작이 반응하지 않음 {#input}

F8이나 Esc로 설정창을 닫고, 게임의 인벤토리·일시정지 화면도 닫으세요. 게임 창으로 포커스를 돌려주세요. 설정창을 닫으면 이동과 시점 조작이 돌아와야 합니다. 간헐적인 입력 문제를 수정했지만 다시 발생한다면 열린 메뉴와 직전 동작을 함께 제보해주세요.

자동 이동은 직접 입력·전투·피격·메뉴·포커스 상실 시 멈춥니다. 완전 경로에서 F10을 누르면 다시 시작할 수 있습니다.

## 지도나 경로 안내가 표시되지 않음 {#map}

생성된 던전에 들어가 F8에서 원하는 기능을 켜고, F8을 닫은 뒤 M으로 지도를 여세요. 목적지를 고르려면 경로 안내가 켜져 있어야 합니다. 기본 목적지는 다음 층 계단이며 M 지도 우클릭으로 계단 안내에 복귀할 수 있습니다.

층 이동 후에는 지도와 안내가 갱신되어야 합니다. 갱신되지 않으면 이전 층과 새 층을 알려주세요. 부분 경로는 자동 이동할 수 없고, 닫힌 문은 직접 열어야 합니다. 도달할 수 없는 목적지는 해당 위치·F8 상태·화면을 함께 보내주세요.

## 자동줍기가 아이템을 줍지 않음 {#pickup}

F8에서 자동줍기를 켜고 설정 반경 안에 서세요. 기본 반경은 3m입니다. 무게 제한·줍기 필터·게임의 줍기 유예를 확인하세요. 멀리 있는 아이템으로 이동하지 않고, 정상 수집 가능한 주변 전리품만 줍습니다. 같은 아이템을 직접 주울 수 있다면 아이템과 설정값을 제보해주세요.

## 문제 제보 {#report}

[Issues]({{ site.data.patch.issues_url }})에 게임·로더·패치 버전, 다른 모드, 재현 순서, 기대한 동작과 실제 동작을 적어주세요. 필요한 경우 화면도 첨부하세요. 개인 경로와 비공개 정보는 지운 뒤 올려주세요. 보안 문제는 [비공개 제보 절차](https://github.com/myso-kr/fell-and-sell-mod/blob/main/SECURITY.md)를 이용하세요.

<details markdown="1">
<summary>제보에 첨부할 로그 찾기</summary>

- `<게임 폴더>/MelonLoader/Latest.log`: 실행·기능 오류. 다음 실행 시 덮어쓰므로 먼저 복사하세요.
- `%USERPROFILE%/AppData/LocalLow/Art Games Studio SA/Fell _ Sell/Player.log`: 게임 메시지.

토글 문제는 `settings:` 줄, 경로 문제는 `route:` 줄과 화면의 목적지를 함께 보내주세요. 마지막 한 줄만 보내기보다 주변 오류도 포함하세요.
</details>
