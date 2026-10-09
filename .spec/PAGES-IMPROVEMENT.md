# Pages 전체 개선

기준: [전체 검토](PAGES-AUDIT.md). 기존 종이색·녹색 디자인과 정적 Jekyll을 유지한다. 계획·설계·검증은 `.spec`, 공개 페이지는 플레이어의 다운로드·설치·조작·도움만 다룬다.

- [x] 공개 데이터와 언어별 경로를 공통화하고 실제 제공 중인 프리뷰 다운로드 동선을 연결한다.
- [x] 홈·문서 허브·다운로드·설치·기능·문제 해결·용어집을 영어/한국어로 모두 정리하고 404를 개선한다.
- [x] 공통 메뉴·대응 언어 전환·현재 위치·문서/다운로드 화면·목차·모바일 코드 표현을 개선한다.
- [x] 기존 `.html` 주소와 DOWNLOAD.html 별칭을 유지하고 공개 기술 본문 경계를 강화한다.
- [x] 렌더링/핵심 URL/언어/제목/공개 상태 및 모바일·다크 모드·키보드 동선을 CI 검사로 고정한다.
- [x] 로컬 사이트·패키지 검사 후 커밋/푸시하고 Actions·실제 Pages 배포를 확인한다.

공개 릴리스는 현재 없으므로 성공한 main 빌드로 안내한다. 기존 draft 릴리스의 게시 작업은 포함하지 않는다. 게임 코드와 실행 중인 게임 파일 교체는 필요하지 않다.

## 로컬 검증

- Jekyll 빌드 성공, 정적 검사 20페이지 통과. 별칭 5개를 제외한 본문 15개.
- Edge 브라우저 320/390/1280px × 밝은/어두운 모드 = 90뷰 통과.
- 첫 화면 다운로드 버튼, 문서 가로 넘침, 본문 건너뛰기·포커스,
  키보드 disclosure, 대응 언어 전환, 두 언어 사용자 동선, 실제 별칭 이동 확인.
- 한국어 다운로드·홈, 영어 홈, 양 언어 모바일 용어집과 다크 모드 스크린샷 검토.
- `check-repository.py`, Python 번역 검증 5개 통과.
- 패키지 빌드 경고/오류 0, ZIP·체크섬·신규 데이터/템플릿 포함 검사 통과.
- gh로 공개 릴리스 0개와 최신 성공한 CI의 v0.4.0 아티팩트가 만료되지 않았음을 확인.
- 실제 게임 플레이를 다시 검증했다는 의미가 아니며 게임 파일 설치는 수행하지 않음.

## 배포

개선 커밋 `ba653875b71ec9042856b5b7e7ac2778ed849ad0`을 main에 푸시했다.

- [CI 37941290567](https://github.com/myso-kr/fell-and-sell-mod/actions/runs/37941290567): 성공. 패키지·체크섬·44개 C# 테스트·5개 Python 테스트 포함.
- [Documentation 37941290211](https://github.com/myso-kr/fell-and-sell-mod/actions/runs/37941290211): 정적 20페이지·Chromium 90뷰 검사 성공.
- [Pages 37941289448](https://github.com/myso-kr/fell-and-sell-mod/actions/runs/37941289448): 성공. Pages API도 해당 커밋 `built`, 오류 없음.
- 공개 본문 15개와 이전 주소 5개 모두 HTTP 200. canonical·언어·버전·본문 구조 확인.
- 새 v0.4.0 CI 아티팩트가 만료되지 않았음을 확인. 공개 릴리스 게시 작업 없음.

추가 정리: 첫 배포의 Gemfile 경고는 로컬 잠금 의존성을 Pages 이미지에서 찾지 못해서
발생했다. `tools/pages`로 로컬 개발 Gemfile/lockfile을 이동하고 kramdown을
[Pages의 2.4.0](https://pages.github.com/versions/)에 맞췄다. [Pages action](https://github.com/actions/jekyll-build-pages/blob/main/entrypoint.sh)은
소스 폴더의 Gemfile을 `bundle check`한 후 별도의 번들된 런타임으로 빌드하므로,
이동 후에는 개발용 lockfile과 배포 런타임을 혼동하지 않는다. 새 위치에서 로컬
빌드·20페이지 정적 검사·저장소 검사도 통과했다. 이 최종 정리 역시 main에 푸시하고
배포 상태와 경고 제거 여부를 확인한다.
