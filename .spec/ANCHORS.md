# 실행 앵커

기준: Fell & Sell 1.7.1 / Steam 25480096 / Unity 6000.3.10f1 / MelonLoader 0.7.3.

권위 있는 목록은 [anchors.json](anchors.json)이다. `tools/check-anchors.py`는 DLL 메타데이터에서
타입·메서드·프로퍼티 이름을 확인하며 게임 코드를 로드하거나 실행하지 않는다.
이름 존재 검사이며 오버로드·호출 순서·정상 수집·저장 동작의 런타임 검증을 대신하지 않는다.
런타임은 정확한 매개변수 형식으로 메서드를 조회하고 실패한 기능의 이름을 로그에 남긴다.

| 기능 | 핵심 앵커 | 실패 영향 |
|---|---|---|
| 상태 | FirstPersonController, PlayerInputHandler, PlayerStats | 자동화 중단 |
| 줍기 | PlayerAutoPickup.pickupRadius, TryPickup, Physics.RaycastAll | 반경 복원·확장 수집 중단 |
| 지도 | DungeonMinimapManager.CurrentMapData, DungeonMapLayoutData | 모드 지도 비활성 |
| 적 | DungeonMinimapManager.GetActiveEnemies | 지도 기능 오류 보고 |
| 경로 | NavMeshSurface.agentTypeID, NavMesh native filter methods | 이동 중단 |
| 입력 | FirstPersonController.HandleMovement, MovementInput | 게임 기본 이동 유지·모드 이동 중단 |
| UI | GUI, Event, Canvas, TMP, Input, Cursor | 해당 패널/입력 기능 오류 보고 |

업데이트 후 `--assemblies`로 실제 설치 경로를 지정해 확인한다.
경로가 없을 때는 `[SKIP]`이며 성공으로 보고하지 않는다. 실패한 앵커를
다른 이름으로 추측해 대체하지 않고 설치 빌드의 메타데이터를 먼저 조사한다.
