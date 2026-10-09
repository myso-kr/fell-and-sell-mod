
# 모드 확장 설계

2026-10-09 작성. **계획 단계이며 현재 v0.3.0에 구현된 기능이 아닙니다.**
이번 조사에서는 게임 메서드를 호출하거나 세이브·게임 상태를 변경하지 않았습니다.

사용자 선택은 경로 표시와 자동 이동 모두 제공, 현재 위치 주변의 획득 가능 전리품만
자동줍기, 도전과제 유지입니다. 짧은 실행 체크리스트는
[구현 계획](PLAN.md)에 있습니다. 이 문서는 초기 조사·설계 기록입니다.
현재 구현의 차이와 검증 상태는 [DECISIONS](DECISIONS.md)와 [STATUS](STATUS.md)를 참조하세요.

## 1. 형제 모드에서 확인한 구현

아래 상태는 로컬 저장소의 코드와 기록에 근거합니다. 이번 조사에서 형제 게임을 실행해
다시 검증한 결과는 아닙니다. 게임 엔진이 달라 코드 전체를 그대로 이식하지 않습니다.

| 저장소 | 확인한 코드·기록 | 가져올 설계 |
|---|---|---|
| Combolands | `autoplay/Exec.cs`가 게임의 정상 선택·배치 메서드를 호출하며 판정과 실행을 분리 | 수집은 정상 획득 메서드에 연결, 의사결정과 실제 게임 변경을 분리 |
| Hell Is Us | `read/navmesh.rs`의 폴리곤 A*·funnel, `guide/pathfind.rs`의 장애물 격자, `map/minimap.rs`의 지도·안개·경로·핀 | NavMesh 우선, 층 높이·닫힌 문·부분 경로 구분, 지도와 월드 경로 표시 |
| Hell Is Us | `cheat/film/walk.rs`의 경로 추종·감속·도착·정체 판정, `.spec/ROUTES.md`의 문/계단/물/재경로 회귀 기록 | 자동 이동의 입력 추종, 제한된 재시도, 진행 거리 기반 정체 감지 |
| Big Dragon | `web/autoplay/engine/dungeon.js`의 길찾기·목표 선택·타임아웃·행동 실패 처리와 주입 가능한 실행 의존성 | 현재 목표를 유지하는 상태 머신, 실패 이유 표시, 게임 없는 테스트 |
| Dungeons 2 | `src/cheats.rs`의 기능 표·검증 상태·값 범위, `docs/CHEATS.md`의 33개 속성 기능 중 21개 검증 기록 | 기능별 지원/검증 상태와 설정 범위, 일시 변경값의 복원 |

Dungeons 2의 전리품 배수는 자동줍기가 아닙니다. Hell Is Us의 지도·길찾기는 이미
코드가 있으나 Unreal 외부 메모리 접근 방식입니다. 이 프로젝트는 Unity IL2CPP 내부
모드이므로 기존 게임 API를 사용하는 편이 맞습니다. Combolands의 도전과제 차단 정책은
사용자가 선택한 **도전과제 유지**와 다르므로 가져오지 않습니다.

참고 코드:
[Combolands 실행 계층](https://github.com/myso-kr/combolands-mod/blob/main/src/Combolands.Mod/autoplay/Exec.cs),
[Hell Is Us NavMesh](https://github.com/myso-kr/hell-is-us-mod/blob/main/src/read/navmesh.rs),
[경로 추종](https://github.com/myso-kr/hell-is-us-mod/blob/main/src/cheat/film/walk.rs),
[Big Dragon 던전 자동화](https://github.com/myso-kr/big-dragon-mod/blob/main/web/autoplay/engine/dungeon.js),
[Dungeons 2 기능 표](https://github.com/myso-kr/dungeons2-mod/blob/main/src/cheats.rs).

## 2. Fell & Sell에서 확인한 기반

MelonLoader가 생성한 `Assembly-CSharp.dll`의 타입·멤버 메타데이터를 읽었습니다.
아래는 **존재 확인**이며 인스턴스 생성 시점, 정상 호출 조건, 부작용은 런타임에서
추가 검증해야 합니다. 원본 바이너리나 추출 결과는 저장소에 포함하지 않습니다.

| 기능 | 확인한 앵커 후보 | 구현 전 확인할 점 |
|---|---|---|
| 기존 자동줍기 | `Il2Cpp.PlayerAutoPickup`: `pickupRadius`, `scanInterval`, `PerformProximityScan`, `TryPickup`, `PauseAutoPickup` | 현재 기본 반경·활성 조건, 사망/인벤토리 열림 처리, 어떤 아이템 타입을 처리하는지 |
| 획득 대상 | `Il2Cpp.ItemPickup`: `CanAutoPickup`, `IsLooseInScene`, `IsBeingPickedUp`, `PickUpItem`, `SetAutoPickupCooldown` | 수집 애니메이션 중 중복 호출, 놓인 가구와 떨어진 전리품 구분 |
| 동전/골드 | `Il2Cpp.CoinPickup.Interact`, `Il2Cpp.GoldPickup.Interact` | 기존 자동줍기 처리 여부와 별도 수집에 필요한 호출 조건 |
| 인벤토리 | `Il2Cpp.InventoryManager`: `CanCarryItem`, `EffectiveMaxWeight`, `CurrentWeight`, `AddItem` | 초과 무게·실패 알림·퀘스트 이벤트가 정상 경로에서 보존되는지 |
| 지도 | `Il2Cpp.DungeonMinimapManager`: `CurrentMapData`, `MapTexture`, `fogOfWarEnabled`, `showEnemies`, `hideEnemiesInUndiscoveredRooms`, `RevealAll` | 표시 설정만 바꿔도 전체 지도가 보이는지, 발견 상태·저장값에 미치는 영향 |
| 지도 표시 | `Il2Cpp.DungeonMapWindow`, `Il2Cpp.DungeonMinimapHUD`: 마커 생성·적 목록·지도 좌표 변환 | 해상도/줌/회전/층 구분, 열린 상자와 죽은 적 제거 |
| 생성 정보 | `Il2Cpp.CustomDungeonGenerator`: `OnDungeonGeneratedStatic`, `LastGeneratedRooms/Corridors/Stairs`, `navMeshSurface`, `ExportMinimapData` | 생성 및 NavMesh 준비 완료 시점과 층 재생성 시 객체 교체 |
| 탐색 저장 | `Il2CppFellSell.Minimap.SavedMinimapData`: 방·통로·계단·상자 발견 목록, 보스/출구 발견값 | 맵 공개가 세이브에 탐색 완료를 남기지 않는지 |
| 이동 | `Il2Cpp.FirstPersonController`: `CharacterController`, `HandleMovement`, `CalculateWorldDirection`, `IsPlayerBlocked` | 실제 입력 전달 경로, 중력·계단·무게·기력 처리 |
| 입력/상태 | `Il2Cpp.PlayerInputHandler.MovementInput`, `IsInputsBlocked`; `Il2Cpp.PlayerStats.IsDead/IsInCombat` | 직접 입력과 모드 입력 구분, UI·사망·전투 정지 조건 |
| Unity 이동망 | `Unity.AI.Navigation.dll`, `UnityEngine.AIModule.dll` 설치 확인 | 플레이어가 쓸 수 있는 agent type·area mask와 실제 NavMesh 범위 |

`AchievementsManager`도 존재하지만 도전과제 유지 선택에 따라 획득·통계 메서드를
변경하지 않습니다. 자동 해금 기능도 이번 범위에 없습니다.

## 3. 기능별 설계

### 주변 자동줍기

기존 `PlayerAutoPickup`을 확장하는 것이 1순위입니다. 정상 획득 경로를 이용해 수량·
퀘스트·효과음·인벤토리 알림을 함께 유지합니다. 반경·수집 간격·종류·등급 필터를
설정하고, 금화·아이템·제작법은 기본 대상 후보로 둡니다. 정확한 기본값은 기존 컴포넌트의
직렬화 값과 실제 동작을 확인한 뒤 정합니다.

현재 플레이어 주변만 대상으로 하고, 같은 공간·도달 가능한 위치인지 검사합니다.
`IsLooseInScene`·`CanAutoPickup`·수집 중 상태·무게 한도를 존중하며, 놓인 가구·상점
진열품·상자 개봉·퀘스트 장치 조작은 일반 전리품 수집과 분리합니다. 금화 등은 기존
자동줍기가 지원하지 않는 것이 확인됐을 때만 개별 `Interact` 어댑터를 추가합니다.
인벤토리 열림, 사망/부활 유예, 장면 전환에서는 중지합니다. 실패 대상은 짧은 재시도
대기 시간을 적용하고 매 프레임 획득 요청을 반복하지 않습니다.

### 현재 층 맵 공개와 ESP

기존 전체지도·미니맵에 현재 생성된 층의 방·통로·계단·출구·보스·상자·적을 표시합니다.
미탐색 지도 표시와 적 표시를 별도 토글로 제공합니다. 다른 층 높이의 마커는 구분하고
전리품/적 표시에는 종류·거리·등급 필터를 둡니다. 월드 ESP는 현재 로드된 객체만 대상으로
하며, 상자 개봉과 적 사망에 따라 마커를 갱신합니다.

먼저 `fogOfWarEnabled`와 적 표시 설정을 일시 변경하는 방식을 검증합니다.
`RevealAll`은 발견 상태와 저장 데이터에 영향이 없다는 확인 전에는 호출하지 않습니다.
표시 플래그만으로 충분하지 않으면 모드 전용 지도 버퍼와 마커를 만들어 원본
`isDiscovered` 및 `SavedMinimapData`를 그대로 둡니다. 해제·층 변경·모드 종료 시
모드가 변경한 설정만 복원하고, 지도 UI를 정상 갱신합니다.

### 경로 표시

목표는 출구·선택한 상자·보스 방·사용자 핀으로 시작합니다. 생성 완료 후 기존 NavMesh를
사용해 현재 위치에서 도달 가능한 목표 접근점까지 계산합니다. 플레이어 크기·높이·계단
규칙이 적 AI와 같다고 가정하지 않습니다. 현재 층 지형의 정보와 문 상태를 함께 검사합니다.

Unity의 `NavMesh.CalculatePath`는 부분 경로에서도 true를 반환하므로
`PathComplete/Partial/Invalid`를 별도로 구분합니다. 경로 계산은 동기식이므로 매 프레임
재계산하지 않고 목표 변경·문 상태 변경·경로 이탈 시 갱신합니다.
[Unity API](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/AI.NavMesh.CalculatePath.html).

지도 경로와 월드의 바닥 방향 표시를 같은 경로 데이터로 그립니다. 도달 불가 목표는
실패 이유와 마지막 도달 지점까지만 표시합니다. NavMesh가 없는 영역은 자동 이동을
비활성화하며, 필요하면 방/통로 그래프 기반의 **추정 안내**를 별도로 제공합니다.
Hell Is Us의 벽 횡단 추정 경로는 자동 이동에 사용하지 않습니다.

### 자동 이동

경로 표시를 검증한 다음 구현합니다. 플레이어에게 NavMeshAgent를 새로 붙이는 대신
`FirstPersonController`가 사용하는 정상 입력 전달 지점을 찾아 경로 방향을 전달합니다.
주입은 이동 처리 구간에 한정하며 직접 WASD/스틱 입력과 구분하고 종료 시 원래 입력을
복원합니다. 카메라 제어는 기본적으로 플레이어에게 둡니다.

`CharacterController.Move`를 별도로 호출하는 방식은 중력을 자체 적용하지 않으므로
기존 컨트롤러와 이중 이동이 생기지 않는지 확인이 필요합니다. 기본 구현은 게임의 중력·
무게·기력·발소리 처리 경로를 유지합니다.
[Unity API](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/CharacterController.Move.html).

상태는 `Idle → Planning → Following → Arrived`이며 메뉴/전투는 `Paused`, 막힘은
`Blocked`, 수동 취소는 `Cancelled`로 구분합니다. 직접 이동 입력·정지 키·사망·층 변경에
즉시 제어를 반환합니다. 전투 시작이나 피해에서는 기본적으로 정지하고 사용자가 다시
시작하도록 합니다. 진행 거리가 늘지 않으면 한정 횟수만 재경로를 계산한 뒤 멈춥니다.
잠긴 문·점프가 필요한 단절·지형 밖 추정 구간은 자동 통과하지 않습니다.

이동 중에도 **주변** 자동줍기는 작동할 수 있으나, 먼 전리품을 쫓아 경로 목표를 바꾸지
않습니다. 자동 전투·상점 운영·자동 판매·순간이동은 이번 첫 구현 범위에서 분리합니다.

## 4. 구조와 운영 규칙

권장 새 모듈은 `Runtime/GameApi`(버전별 어댑터), `Runtime/Capabilities`(지원 상태),
`Runtime/GameSession`(층/생성/정지 상태), `Automation/AutoPickup`,
`Navigation/RoutePlanner`, `Navigation/AutoMove`, `Map/MapReveal`,
`Overlay/ModPanel`, `Overlay/RouteOverlay`, `Settings/ModSettings`입니다.
게임 의존 어댑터와 순수 필터·목표·추종·정체 판정을 분리합니다.

- 각 기능은 기본 꺼짐이며 자동 이동은 사용자가 매번 시작합니다.
- 기능별 상태는 미지원·API 확인·런타임 확인·플레이 검증으로 나누어 표시합니다.
- 한 기능의 앵커 실패가 한국어 패치나 다른 기능 초기화를 중단하지 않게 합니다.
- 사용자 선택대로 도전과제는 유지하고 게임의 통계·획득 처리를 변경하지 않습니다.
- 매 프레임 전체 객체 검색 대신 생성/삭제 이벤트와 제한된 주기 캐시를 사용합니다.
- 초기 성능 목표는 객체 목록 갱신 5 Hz 이하, 마커 갱신 10 Hz 이하, 경로 재계산 2 Hz
  이하입니다. 이는 측정 결과가 아니라 구현 시 검증·조정할 예산입니다.
- 반경/간격 변경값은 인스턴스별로 원래 값을 기록하고 모드가 소유한 변경만 복원합니다.
- 패널·정지·맵 토글 키는 기존 바인딩을 확인한 후 설정 가능하게 추가합니다.
- 새 패널 문구는 `locale/ko/mod-ui.json` 같은 별도 카탈로그를 사용해 기존
  `Game/<ID>`·`UI/<ID>` 및 1,243개 원문 테이블 카운트와 섞지 않습니다.

## 5. 구현 단계와 검증

| 단계 | 제공 기능 | 핵심 통과 기준 |
|---|---|---|
| M0 | 읽기 전용 앵커·인스턴스·층·NavMesh 진단 | 메뉴/던전/층 전환에서 객체 유효성과 생성 시점 확인 |
| M1 | 패널·설정·주변 자동줍기 | 정상 획득과 수량/알림 일치, 벽 너머·중복·가구 줍기 방지 |
| M2 | 현재 층 지도 공개·마커 | 해제/세이브 재로드 후 발견 기록 보존, 상자/적 마커 갱신 |
| M3 | 목표 선택·지도/월드 경로 | 직선/모퉁이/계단/다른 층/닫힌 문/부분 경로에서 올바른 상태 |
| M4 | 자동 이동·주변 줍기 조합 | 수동 취소·UI·전투·피해·사망·층 변경·정체에서 제어 반환 |
| M5 | 회귀·성능·배포 | 번역/한글 표시 유지, 설정 복원, 도전과제 차단 훅 없음, 검증된 기능만 문서화 |

첫 배포 후보는 **v0.4 자동줍기·맵 공개**, 두 번째는 **v0.5 경로·자동 이동**입니다.
실제 버전 확정·태그·릴리스는 구현과 검증 후 진행합니다.

순수 로직 테스트는 수집 필터/무게/중복 상태, 부분 경로 거부, 층이 다른 목표, 경로 추종·
도착·정체·취소를 다룹니다. CI는 이 테스트와 빌드·패키징을 수행하고, 런타임 앵커와
실제 플레이 결과는 별도 기록합니다. 게임 패치 후에는 기존 번역 및 각 확장 기능의
앵커를 따로 검사합니다.
