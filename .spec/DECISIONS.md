# 구현 결정

- 도전과제 유지: 소유자 선택. AchievementsManager, Steam 통계, 업적 해제 경로는 패치하지 않는다.
- 자동줍기: 최대 4m, 기본 3m의 주변 수집. 원격 아이템 추적·직접 인벤토리 추가는 하지 않는다.
- 지도 공개: 기존 M 지도/미니맵에 모드 소유 자식 레이어를 부착하고 전체 지도 버퍼를 별도 텍스처로 복사한다. 원본 탐색 기록과 저장 데이터를 건드리지 않는 것이 구조적으로 보장되는 경계다.
- 지도 범위: 현재 생성된 던전의 레이아웃. 수직 방도 투영하므로 다른 높이는 경로 판정에서 구분한다.
  목표 위치의 높이를 사용하며 자유 핀은 플레이어 높이를 사용한다. 다른 수직 층의 핀을 자동 추정하지 않는다.
- 경로: 생성기의 NavMeshSurface agentTypeID와 Walkable 영역만 조회. 이 IL2CPP 빌드는
  관리형 QueryFilter 오버로드가 없고 `CalculatePathFilterInternal`과
  `SamplePositionFilter_Injected`가 남아 있어 이를 명시적으로 바인딩한다.
- 자동 이동: 완전 경로만 시작; 부분 경로는 표시만. 수동 입력·피격·전투·사망·UI·포커스 상실 때 중단.
  진행 중 경로는 고정해 재탐색이 막힘 타이머를 초기화하지 않게 한다. 2.5초 동안 진행이 없으면 정지,
  사용자가 다시 시작할 때 새 경로를 사용한다. 자동 재시도는 첫 구현에서 제외했다.
- 설정창: 커서 상태만 보관·복원하고, 패널이 열린 동안 행동 차단 판정에 응답한다.
  `SetInputsBlocked`는 게임의 지속 상태를 잠글 수 있어 제거했다. 게임의 입력 차단 플래그를 쓰지 않는다.
  모드 전용 TMP Canvas와 기존 Noto 폰트를 쓴다.
  모드 UI 98글자 검증은 기존 번역의 699글자 기록과 별개다. 공유 GUISkin에는 쓰지 않는다.
  Font의 OS helper/복사 생성자와 DrawTexture 일부 오버로드는 복원된 스텁이거나 빠져 있어 사용하지 않는다.
- 시작 화면 회귀: 싱글턴 getter 호출로 UI/플레이어가 생성될 수 있으므로 자동화는 backing field만 읽는다.
  신규 기능이 모두 꺼지면 게임 상태 조회를 생략한다. 기본 메뉴 복구는 소유자가 확인했다.
- 배포: v0.4.0 확장 프리뷰. v0.3.0의 번역 검수 확인을 신규 기능의 실행 검증으로 확대 해석하지 않는다.

## Current input and pickup boundary

The earlier panel predicate override is superseded: leave the game's global block queries and release processing untouched. Suppress owned-panel actions at controller methods and lease zero movement input around HandleMovement; native gravity still runs. Never clear unknown game block flags.

Pickup now requests the native PerformProximityScan while enabled. Both that request and native Update use one scan-prefix cadence. Keep native TryPickup eligibility/weight/filter behaviour and pause/death state; no direct inventory writes. Treat sibling colliders owned by the same ItemPickup as one target for visibility.

## Floor recreation and native pickup option scope

Treat scene callbacks, generator recreation, map initialization and progression floor changes as invalidation boundaries. Queue native hook notifications and clean up owned objects in OnUpdate, reacquire existing components, restart default exit planning, and version copied map buffers even when the game reuses its texture. No discovery flags or game map reset methods are invoked.

Native scan and TryPickup both consult the global auto-pickup option. Override its getter only in a permitted scan on the current thread and restore the scope through a Harmony finalizer. This lets the mod toggle work when the global option is disabled, without persisting an options change or bypassing item eligibility. Shared cadence applies to both native and mod scans. Owner confirmed floor refresh and nearby pickup after installing the follow-up build.
