# 코드 구조

`combolands-mod/docs/CONVENTIONS.md`의 한 파일 한 책임, 순수 판단과 런타임 경계 분리,
`hell-is-us-mod/.spec/README.md`의 기획 문서 체계를 따른다.
기능 폴더는 소문자, C# 파일·타입은 PascalCase를 사용한다.

| 위치 | 책임 | 게임 쓰기 |
|---|---|---|
| `Plugin.cs` | MelonMod 생명주기와 모듈 연결 | 없음 |
| `Config.cs` | MelonPreferences 바인딩; 신규 기능 기본 꺼짐 | 모드 설정만 |
| `Log.cs` | 기능별 예외 격리와 한 번 오류 보고 | 없음 |
| `Reflect.cs` | 선언 타입부터 멤버 조회; 순수·테스트 가능 | 범용 호출 경계 |
| `Anchors.cs`, `Alive.cs` | 지연 로딩 타입·인스턴스·Unity 생존 확인 | 없음 |
| `Sequence.cs` | 관리형 배열과 IL2CPP Count/Item 컬렉션 경계; 순수·테스트 가능 | 없음 |
| `i18n/` | 기존 번역·TMP 폰트 | 번역 패치만 |
| `autoplay/Snapshot.cs` | 상태·좌표 값; Unity 참조 없음 | 없음 |
| `autoplay/State.cs` | 플레이어·입력·인벤토리·전투 상태 읽기 | 없음 |
| `autoplay/Pickup.cs` | 반경 변경 수명과 원래 값 보관 | `Exec`에 위임 |
| `autoplay/Sight.cs` | 벽과 다른 물체가 줍기를 가리는지 검사 | 없음 |
| `autoplay/Exec.cs` | 정상 이동 입력의 임시 대여·복원, 반경 쓰기, 패널 커서 대여 | 이 파일만 |
| `autoplay/Patches.cs` | 이동 앞뒤·줍기 가시선·패널 행동 판정 | `Exec`에 위임 |
| `autoplay/Supervisor.cs` | 언제 모듈을 호출하고 정리할지 | 없음 |
| `map/Snapshot.cs`, `map/Read.cs` | 던전 레이아웃과 마커를 값으로 복사 | 없음 |
| `map/Enemies.cs` | 로드된 적 위치 읽기; 오류가 지도 전체에 전파되지 않게 격리 | 없음 |
| `map/Overlay.cs` | 별도 지도; 핀·상자·출구·보스 목적지 선택 | 모드 목적지만 |
| `guide/Route.cs` | NavMesh 조회·완전/부분/불가 판별 | 경로 객체만 |
| `guide/Follow.cs` | 순수 이동 판단·도착·막힘·중단 | 없음 |
| `guide/Move.cs` | 수동 입력과 순수 추종기 연결 | 없음 |
| `guide/Overlay.cs` | 월드 경로 화면 투영 | 없음 |
| `panel/` | 한국어 UI, IMGUI 경계와 키 입력 | `Exec`에 위임 |

`panel/Tmp.cs`는 모드 소유 ScreenSpaceOverlay Canvas와 클릭을 가로채지 않는 TMP 레이블만 관리한다.
기본 버튼/배경은 IMGUI로 그리고 글자는 검증된 Noto TMP 자산을 쓴다.
레거시 Font 헬퍼와 게임 공유 GUISkin의 글꼴 변경은 사용하지 않는다.

자동줍기는 `PlayerAutoPickup.Update`를 대체하지 않는다. 일시적으로 반경만 변경하고
`TryPickup` 앞에서 상태와 가시선을 검사한다. 정상 필터·무게·유예·수집 애니메이션은 게임이 소유한다.
지도는 `CurrentMapData`를 읽어 별도 표시한다. `RevealAll`, `isDiscovered`, `currentAlpha`,
원본 텍스처 버퍼, 저장 메서드는 호출하거나 변경하지 않는다.
이동은 `HandleMovement` 전 입력을 주입하고 후/예외 후 원래 값을 복원한다.
추가 `CharacterController.Move`, NavMeshAgent, 순간이동, 중력 처리는 없다.

`tests/FellAndSell.Mod.Tests`는 순수 소스를 링크하고 게임 없이 실행한다.
`tools/check-anchors.py`는 설치된 DLL을 메타데이터로만 읽는다.
CI의 순수 테스트 통과와 실제 게임 기능 확인은 별도 상태로 기록한다.

## Native map integration

`map/Layer.cs` owns child RawImage/marker objects beneath the native M map and minimap. It copies `_fullMapBuffer` into its own Texture2D and leaves discovery data and the original texture unchanged. `Select.cs` converts a click through the native RectTransform and WorldToMapNormalized affine transform; drags over 6 pixels are ignored. `autoplay/Exec.cs` closes the native map through CloseMap before an explicit movement start. Runtime dungeon validation is pending.

## Panel pointer input

`panel/Pointer.cs` is a pure press/release state machine. `Widget.Tick` reads screen-pixel mouse input once per update; shared Hit rectangles also position the TMP labels and visual boxes. Rendering performs no settings writes and saves/restores GUI.matrix. Toggle logs include the preference name and value.

## Route endpoint search

`guide/Search.cs` projects endpoints within a 2.25m bound, deduplicates nine nearby destination samples, and ranks complete routes before partial routes, then endpoint error and native route length. Native NavMesh supplies every connection. No room-graph shortcut bridges walls or disconnected meshes. `map/Height.cs` uses a clicked room world elevation. Right-click clears only the selected target and restores the default floor trigger; movement stops until explicitly started again.
