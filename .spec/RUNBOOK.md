# 개발 실행 절차

1. `python tools/check-repository.py`
2. `python -m unittest discover -s tests`
3. `dotnet test tests/FellAndSell.Mod.Tests -c Release --nologo`
4. 설치 앵커: `python -m pip install -r tools/requirements-anchors.txt` 후
   `python tools/check-anchors.py --assemblies "<game>/MelonLoader/Il2CppAssemblies"`
5. `./tools/package.ps1` → `python tools/check-package.py`
6. 게임 종료 후 `./tools/deploy.ps1 -GameDir "<game>"`. 실행 중에는 DLL을 교체하지 않는다.
7. Steam으로 실행하고 `MelonLoader/Latest.log`의 초기화·기능별 오류를 확인한다.

실행 회귀: 신규 기능 모두 꺼짐 → F8 패널과 커서 복원 → 주변 수집 켬/끔과 반경 복원 →
벽 너머 물체·가구·과적·인벤토리·죽음 유예 → 모드 지도와 원본 탐색 비교 →
상자/출구/보스/핀 경로 → 부분 경로 시작 거부 → F10 이동 → 수동 입력/피격/전투/메뉴/사망/포커스/씬 정지.
모드 지도 사용 전후 저장을 비교해 탐색 기록에 영향이 없는지도 게임에서 확인한다.

Actions는 게임 없는 빌드·순수 테스트·패키지 검사만 수행한다.
태그·릴리스 공개는 별도 요청에서 수행한다. 실제 게임에서 본 결과는 STATUS에 날짜·빌드와 함께 기록한다.
