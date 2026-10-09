# Pages 전체 검토 — 2026-10-09

검토 기준 커밋: `876efef`. 요청 범위는 개선점 조사다. Pages 소스 변경·재배포는 수행하지 않았다.

이 문서는 개선 전 조사 기록이다. 후속 전체 개선과 검증 결과는
[PAGES-IMPROVEMENT.md](PAGES-IMPROVEMENT.md)에 기록한다.

## 검토 범위와 구조

- `docs/`의 전체 페이지, 공통 Liquid 레이아웃, CSS, Jekyll 설정·의존성, Documentation workflow, 저장소/렌더링 검사 도구를 읽었다.
- 페이지 콘텐츠는 Markdown, 공통 셸은 `_layouts/default.html`, 디자인 토큰·반응형은 `assets/css/site.css`가 소유한다. JavaScript·외부 폰트·이미지 의존성은 없다.
- `jekyll-relative-links`가 `.md` 링크를 각 permalink로 변환한다. Pages는 `main /docs`에서 배포하며 Documentation workflow는 별도로 렌더링을 검사한다.
- 렌더링된 11개 페이지를 320/390/1280px, 총 33개 뷰에서 검사했다. 문서 전체의 가로 넘침 0건, 본문 건너뛰기/본문 포커스 실패 0건. Downloads는 밝은/어두운 모드 스크린샷도 확인했다.
- 검사 기준: [Vercel Web Interface Guidelines](https://raw.githubusercontent.com/vercel-labs/web-interface-guidelines/main/command.md). 일반 UX 스캔 도구는 Liquid 분기를 중복 메뉴로 세고 CSS를 폼으로 오인했다. 이 경고들은 실제 결함으로 채택하지 않았다.
- 로컬 렌더링 검사, 공개 URL HTTP 응답, gh의 릴리스 목록을 별도로 확인했다. 브라우저의 전체 접근성 인증이나 모든 브라우저 호환성 검사를 의미하지 않는다.

| 현재 경로 | 역할 | 검토 결과 |
|---|---|---|
| `/`, `/ko/` | 홈 | 시각적 진입점은 있으나 기술 검수 기록·오래된 기능 상태가 남음 |
| `/guide/` | 영어 문서 허브 | H1→H3 건너뜀, 문서 링크 중복 |
| `/ko/guide/` | 한국어 통합 가이드 | 설치/문서 메뉴가 같은 목적지, 유지보수 정보 혼재 |
| `/DOWNLOADS.html` | 공용 다운로드 | 가장 우선적으로 동선·언어·정보 순서 재설계 필요 |
| `/INSTALLATION.html` | 설치·업데이트·제거 | 한국어 대응 페이지 없음, 긴 명령 블록 |
| `/features/`, `/ko/features/` | 기능 안내 | 확인 상태는 최신, 언어 전환은 대응 페이지 대신 홈으로 이동 |
| `/TROUBLESHOOTING.html` | 문제 해결 | 한국어 대응 페이지 없음, 긴 문서에 빠른 증상 탐색 없음 |
| `/TRANSLATION.html` | 용어집 | 한국어 대응 페이지 없음 |
| `/404.html` | 오류 복구 | 한국어 문구의 부분 언어 표시·다운로드 복구 링크 보완 가능 |

## 우선순위별 발견 사항

### P1 — 다운로드 페이지가 다운로드보다 설명을 먼저 제공

`docs/DOWNLOADS.md:11`, `docs/DOWNLOADS.md:19`, `docs/_layouts/default.html:31`

다운로드 페이지 자체에는 `.button` CTA가 0개다. 가장 먼저 제시하는 링크는 Releases이고, 현재 공개 릴리스는 0개다. gh에 보이는 v0.3.0은 draft prerelease이며 공개 설치 파일이 아니다. 사용자는 CI 목록에서 성공한 main 실행과 아티팩트를 직접 찾아야 한다. 헤더에도 다운로드 항목이 없다.

개선: 상단에 현재 받을 수 있는 v0.4.0 프리뷰, GitHub 로그인 필요 여부, 보관 만료 조건을 짧게 표시하고 **성공한 빌드 보기**를 주 동작으로 둔다. 실제 공개 릴리스가 있을 때는 패키지 ZIP을 주 동작으로 교체한다. 없는 배포본을 다운로드 가능하다고 표시하지 않는다. 최신 빌드 링크를 관리한다면 실행 ID·버전·만료 상태를 함께 검증한다.

### P1 — 한국어 다운로드 동선이 영어 페이지로 이동

`docs/ko/index.md:17`, `docs/DOWNLOADS.md:4`, `docs/DOWNLOADS.md:48`

한국어 홈의 주 버튼도 `lang=en` 다운로드 페이지로 이동한다. 영어 설명, 파일 트리, gh CLI가 먼저 나오고 한국어 섹션은 뒤에 있다. 390px/844px 뷰에서 한국어 섹션 시작점은 문서 Y≈1820px, 페이지 높이는 2864px이다. 영어 페이지 안의 한국어 구간에도 별도 `lang=ko`가 없다.

개선: `/downloads/`와 `/ko/downloads/`를 대응 페이지로 분리한다. 브라우저 다운로드 단계를 먼저, CLI는 선택적인 고급 방법으로 접는다. 한국어 방문자에게 한국어 헤더·동작·절차를 유지한다.

### P1 — 공개 본문에 유지보수/구현 기록이 남음

`docs/ko/README.md:23`, `docs/ko/README.md:86`, `docs/index.md:27`, `docs/index.md:62`, `docs/ko/index.md:60`

TMP 텍스트 컴포넌트 수, 훅 실행 검수, 글리프 수, 태그 빌드와 Release workflow/초안 게시 절차가 공개 안내에 남아 있다. 기술 문서 파일을 `.spec`으로 이동했지만 본문 분리는 완료되지 않았다. 한국어 가이드의 번역 코드/PR 설명도 CONTRIBUTING과 역할이 겹친다.

개선: 구현·검수·릴리스 운영은 `.spec`, 기여 방법은 CONTRIBUTING으로 모은다. 공개 본문에는 사용자가 필요한 호환 버전, 기능 상태, 설치 파일 선택, 조작, 문제 해결만 남긴다. 사용자가 오류를 식별하는 데 필요한 실제 로그 예시는 Troubleshooting에 유지할 수 있다.

### P1 — 기능 검증 상태가 페이지마다 다름

`docs/index.md:86`, `docs/ko/index.md:84`, `docs/ko/README.md:32`

홈 두 언어와 한국어 통합 가이드는 자동줍기가 검증 중이라고 표시한다. 기능 안내와 `.spec/release-metadata.json`에는 소유자의 자동줍기·층 갱신 확인이 반영되어 있다. 같은 배포에서 사용자에게 서로 다른 상태를 보여 준다.

개선: 공개 버전·호환성·기능 상태는 공통 데이터/include에서 렌더링한다. 내부 검증 메타데이터를 그대로 노출하지 않고 공개할 사실만 선택한다. 검사 도구도 버전/번역 개수뿐 아니라 대응 페이지의 사용자 상태 일치를 검증한다.

### P2 — 한국어 설치/문서 메뉴 중복과 현재 위치 표시 누락

`docs/_layouts/default.html:32`, `docs/_layouts/default.html:33`, `docs/_layouts/default.html:37`

한국어 ‘설치’와 ‘문서’는 모두 `/ko/guide/`이다. 영어는 설치 문서와 문서 허브를 구분한다. 본문 메뉴에는 현재 페이지 표시가 없다. 언어 버튼은 항상 홈으로 이동하며, `/features/`↔`/ko/features/`처럼 이미 대응 페이지가 있어도 읽던 문맥을 잃는다. hreflang도 홈에서만 생성된다.

개선: 양 언어에 같은 역할의 다운로드/설치/문서/기능/도움 경로를 마련한다. 페이지 식별자로 대응 번역 URL과 `aria-current`를 계산하고, 대응 문서가 없을 때만 언어 홈으로 대체한다.

### P2 — 문서형 화면과 홈 화면에 같은 타이포그래피 적용

`docs/assets/css/site.css:25`, `docs/assets/css/site.css:26`, `docs/DOWNLOADS.md:11`

모든 H1에 동일한 큰 홈 제목 크기를 적용하고 H2마다 큰 여백과 구분선을 넣는다. Downloads는 긴 검수 설명이 첫 화면의 대부분을 차지한다. 링크·번호 목록·주의사항·고급 명령의 시각적 중요도가 충분히 구분되지 않는다. 한국어 안내가 더 아래로 밀리는 원인이기도 하다.

개선: home/article/download 페이지 타입을 구분한다. 다운로드 화면 순서는 현재 패키지와 버튼 → 브라우저 다운로드 단계 → 안쪽 ZIP 설치 대상 → 설치 안내 → 선택적 CLI/프리뷰 설명으로 정한다. 색상 전면 교체보다 정보 위계와 내용 압축을 먼저 처리한다.

### P2 — 긴 문서의 빠른 탐색과 제목 계층 보완

`docs/README.md:16`, `docs/TROUBLESHOOTING.md:9`, `docs/INSTALLATION.md:11`

영어 문서 허브는 H1 다음에 카드 H3가 나와 H2 수준을 건너뛴다. 390px에서 Troubleshooting은 약 5516px, 한국어 가이드는 4831px인데 증상별 목차가 없다. 설치 페이지도 검증/설치/업데이트/제거까지 빠르게 이동할 수 있는 동선이 없다.

개선: 카드 제목을 H2로 바꾸거나 H2 그룹을 먼저 제공한다. 긴 안내에는 짧은 본문 목차 또는 증상별 링크를 둔다. 고급 내용은 details로 접고 기본 절차를 먼저 표시한다.

### P2 — URL 규칙과 다운로드 별칭

`docs/DOWNLOADS.md:5`, `docs/INSTALLATION.md:5`, `docs/EXPANSION.md:6`

폴더형 `/features/`, `/guide/`와 대문자 `.html` 문서 경로가 섞여 있다. 배포 URL 확인 결과 **`DOWNLOAD.html`은 404, `DOWNLOADS.html`은 200**이다. 현재 내부 링크는 복수형을 사용하므로 전체 사이트의 내부 링크가 깨진 상태는 아니다.

개선: 새 대표 주소는 일관된 소문자 폴더 경로로 정한다. 기존 `DOWNLOADS.html`을 보존하고 필요하면 `DOWNLOAD.html`도 대표 다운로드로 연결하는 별칭을 둔다. URL 변경 시 기존 링크·북마크의 복구를 함께 구현한다.

### P2 — CI 검사 범위가 실제 사용자 흐름을 다루지 않음

`tools/check-site.py:43`, `tools/check-site.py:50`, `tools/check-site.py:67`, `.github/workflows/docs.yml:19`

현재 검사는 내부 링크 대상·fragment·lang 속성·canonical 존재를 확인한다. 필수 경로 목록에는 Downloads/Installation/Troubleshooting/Glossary가 없다. canonical이 실제 페이지 URL과 맞는지, H1 개수/제목 계층, 대응 언어 연결, 중복 메뉴 목적지, 다운로드 가능 상태는 검사하지 않는다. 문서별 모바일 검사는 ignored `generated/preview-check.py`에만 있으며 CI에는 없다. 이번에 확인한 언어 동선·기술 본문·상태 불일치가 있어도 검사는 통과한다.

개선: 핵심 플레이어 페이지·canonical 실제 값·언어 대응·유일한 H1/제목 계층·다운로드 상태 검사로 확장한다. 대표 화면의 320/390/1280px·다크 모드·키보드 동선 검사를 저장소 도구와 CI에 포함한다. GitHub 외부 링크의 접근성/파일 상태는 별도 검증 단계로 다룬다.

### P3 — 설명 메타데이터와 모바일 코드 표현

`docs/DOWNLOADS.md:6`, `docs/INSTALLATION.md:6`, `docs/TROUBLESHOOTING.md:6`, `docs/assets/css/site.css:41`

몇몇 description이 제목을 그대로 반복해 검색/공유 시 페이지 역할을 설명하지 않는다. 모바일 파일명은 anywhere 규칙 때문에 임의 위치에서 줄바꿈되고, Downloads 파일 트리·CLI는 350px 컨테이너에 각각 약 542/887px 폭으로 수평 스크롤된다. 페이지 전체 넘침은 없고, 현재 Edge에서는 PRE에 키보드 포커스도 도달한다. 따라서 키보드 접근 불가로 단정하지 않는다.

개선: 페이지 목적에 맞는 짧은 description을 작성한다. CLI는 선택적으로 접고 명령을 줄로 나눈다. 파일 트리는 짧은 설치 대상 설명으로 대체하거나 스크롤 가능 표시를 제공한다. 파일명을 복사할 수 있는 동작은 필요할 때만 추가한다.

## 권장 개선 순서

1. Downloads를 양 언어의 실제 다운로드 진입점으로 재구성하고 현재 가능한 패키지를 먼저 안내한다.
2. 공개 본문의 기술 기록을 정리하고 사용자 상태 불일치를 수정한다.
3. 대응 언어 경로·한국어 문서 허브·설치/문제 해결을 정리한 뒤 공통 메뉴를 연결한다.
4. 문서형/다운로드형 레이아웃과 제목 계층·목차·모바일 파일 표현을 개선한다.
5. 핵심 경로·언어·본문 경계·렌더링 동선을 CI 회귀 검사로 고정한다.

공개 릴리스 게시 여부는 별도 작업이다. 이번 검토를 이유로 기존 draft를 공개하거나 없는 직접 다운로드 링크를 만들지 않는다.

## 확인된 기반과 산출물

공통 CSS와 레이아웃, 명시적 front matter, 프로젝트 baseurl, canonical/OG, 시스템 폰트, 자동 다크 모드, 본문 skip link는 이미 있다. 33개 뷰에서 문서 전체 가로 넘침은 없었고 skip link가 본문에 도달했다. 페이지별 주 동작·언어 연결·정보 책임을 정리하는 것이 우선이다.

ignored 조사 결과: `generated/pages-audit.py`, `generated/pages-audit.json`, `generated/audit-downloads-first-screen-390.png`, Downloads 데스크톱/다크 스크린샷. 기술 조사 기록은 이 `.spec` 문서에만 저장한다.
