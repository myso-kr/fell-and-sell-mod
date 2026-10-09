---
layout: default
title: "다운로드"
description: "성공한 GitHub Actions 빌드에서 Fell & Sell 패치 프리뷰를 받는 방법과 ZIP 압축 해제 순서를 안내합니다."
lang: "ko"
permalink: "/ko/downloads/"
page_id: "downloads"
page_type: "article"
summary: "현재 패치는 프리뷰 빌드로 받을 수 있습니다."
---

<p class="eyebrow">v{{ site.data.patch.version }} 프리뷰 · Windows · MelonLoader {{ site.data.patch.loader_version }} x64</p>
<div class="actions"><a class="button primary download-cta" href="{{ site.data.patch.builds_url }}">성공한 빌드 찾기</a></div>
<p class="note">GitHub 로그인 필요 · 빌드 파일 보관 기간 30일</p>

## 브라우저에서 받기

1. 위 버튼으로 이동한 뒤 GitHub에 로그인하세요.
2. **초록색 체크가 있는 main 빌드**를 여세요. 최신 실행이 실패했다면 이전의 성공한 빌드를 선택하세요.
3. 실행 페이지 아래 **Artifacts**에서 `fell-and-sell-mod-v{{ site.data.patch.version }}`를 받으세요. 다른 버전이라면 해당 빌드의 버전 안내도 확인하세요.
4. 다운로드한 바깥 ZIP을 먼저 푸세요. 안에 있는 `fell-and-sell-mod-vX.Y.Z.zip`이 설치할 패키지이며, `.zip.sha256`은 확인용 체크섬입니다.
5. **안쪽 패키지 ZIP**을 풀어 `Mods`와 `UserData` 폴더를 게임 폴더에 넣으세요.

<div class="notice" markdown="1">
**어떤 ZIP을 설치하나요?** 바깥 ZIP은 GitHub가 파일을 묶어 전달하는 용도입니다. 설치하는 안쪽 ZIP에는 `Mods/FellAndSellMod.dll`과 `UserData/FellAndSell/`이 있습니다. GitHub의 **Code → Download ZIP**은 소스 코드이므로 게임에 설치할 수 없습니다.
</div>
<div class="actions"><a class="button" href="{{ '/ko/installation/' | relative_url }}">다음: 설치 안내</a></div>

## 파일이 없거나 받을 수 없을 때

Artifacts가 없다면 빌드가 완료됐는지, 성공했는지, GitHub에 로그인했는지 확인하세요. 보관 기간이 지난 파일은 받을 수 없으므로 더 최근의 성공한 빌드를 선택하세요. [문제 해결]({{ '/ko/help/#download' | relative_url }})에서 압축파일 문제도 확인할 수 있습니다.

## 정식 릴리스

현재 공개된 정식 릴리스는 없습니다. 공개 패키지가 준비되면 [Releases]({{ site.data.patch.releases_url }})에 게시됩니다. 지금은 위의 프리뷰 빌드를 이용하세요.

<details markdown="1">
<summary>선택 사항: GitHub CLI로 받기</summary>

`gh auth login`으로 로그인한 뒤 PowerShell에서 실행합니다. 첫 명령의 목록에서 성공한 main 실행의 ID를 선택하세요.

```powershell
gh run list --repo myso-kr/fell-and-sell-mod `
  --workflow ci.yml --branch main --status success --limit 5
$runId = Read-Host '다운로드할 실행 ID'
gh run download $runId --repo myso-kr/fell-and-sell-mod `
  --name fell-and-sell-mod-v{{ site.data.patch.version }} `
  --dir .\patch-download
```

CLI는 바깥 ZIP을 풀어 줍니다. `patch-download` 안의 패키지 ZIP을 [설치 안내]({{ '/ko/installation/' | relative_url }})에 따라 설치하세요. 빌드의 버전이 다르면 artifact 이름도 바꿔야 합니다.
</details>
