# .spec — 개발 문서

형제 `hell-is-us-mod`의 규칙을 따른다. 설계·기획은 이 폴더가 기준이고,
`docs/`는 사용자 가이드와 Pages 소스다. 코드가 문서보다 우선한다.

| 순서 | 문서 | 내용 |
|---|---|---|
| 1 | [STATUS.md](STATUS.md) | 구현·검증 상태와 남은 런타임 확인 |
| 2 | [PLAN.md](PLAN.md) | 기능별 완료 조건 |
| 3 | [RUNBOOK.md](RUNBOOK.md) | 빌드·테스트·설치·게임 업데이트 대응 |
| 4 | [ARCHITECTURE.md](ARCHITECTURE.md) | 코드 경계와 형제 컨벤션 |
| 5 | [DECISIONS.md](DECISIONS.md) | 채택한 구현과 제한의 이유 |
| 참고 | [EXPANSION.md](EXPANSION.md) | 초기 조사와 확장 설계 |
| 참고 | [ANCHORS.md](ANCHORS.md), [anchors.json](anchors.json) | 실행 앵커와 메타데이터 검사 목록 |
| 참고 | [I18N.md](I18N.md) | 한글 패치 구현 이력 |

생성 데이터는 `generated/`에만 두고 커밋하지 않는다. 게임 DLL을 재배포하지 않는다.

## Technical reference

- [Conventions](CONVENTIONS.md): source structure, public/technical boundary
- [Development](DEVELOPMENT.md): build, extraction and validation
- [Runtime details](RUNTIME-ANCHORS.md): localization anchors
- [Game survey](GAME-SURVEY.md): observed runtime evidence
- [Translation decisions](TRANSLATION.md): terminology and source inconsistencies
- [Pages maintenance](PAGES.md): site build and deployment
- [Releasing](RELEASING.md): maintainer publication procedure
- [Recorded verification](release-metadata.json): machine-readable target and review scope

Only user-facing guides and Pages source belong in `docs/`. Implementation records and release/deployment procedures stay here. The package does not contain `.spec/`.
