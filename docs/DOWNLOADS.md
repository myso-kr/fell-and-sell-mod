---
title: "Downloads / 다운로드"
lang: en
---

# Downloads / 다운로드

Public [Releases](https://github.com/myso-kr/fell-and-sell-mod/releases) are the
stable download location once a maintainer publishes a package. Until then, use
an artifact from a successful [CI run](https://github.com/myso-kr/fell-and-sell-mod/actions/workflows/ci.yml).
A successful build checks the package; it does not guarantee every gameplay screen
has been reviewed. The project owner confirmed in-game text display review on
2026-10-09; broader gameplay, Japanese switching and uninstall checks remain
separate. Current builds are previews with complete extracted-table coverage.

## Download a CI build

1. Sign in to GitHub and open the CI workflow above.
2. Select a **successful main-branch run** and confirm its commit/version.
3. Under **Artifacts**, download `fell-and-sell-mod-v0.4.0`.
4. Extract the outer artifact ZIP. It contains the installable mod ZIP, its
   `.zip.sha256` and `release-notes.md`.
5. Check the SHA-256 and extract the **inner mod ZIP** into the game directory.
   Install MelonLoader separately and launch through Steam.

Artifacts are retained for **30 days**. PR artifacts may contain unreviewed changes;
use a main-branch build unless you specifically intend to test a contribution.
GitHub sign-in and repository read access are required for artifact downloads.
See [GitHub's artifact download guide](https://docs.github.com/en/actions/how-tos/manage-workflow-runs/download-workflow-artifacts).

```text
Downloaded Actions artifact/
├─ fell-and-sell-mod-v0.4.0.zip        install this inner ZIP
├─ fell-and-sell-mod-v0.4.0.zip.sha256
└─ release-notes.md
```

With an authenticated gh CLI, replace `RUN_ID` with the successful run's ID:

```powershell
gh run list --repo myso-kr/fell-and-sell-mod --workflow ci.yml --branch main --status success --limit 5
gh run download RUN_ID --repo myso-kr/fell-and-sell-mod -n fell-and-sell-mod-v0.4.0 --dir download
```

## CI 아티팩트 받기

GitHub에 로그인한 뒤 **Actions → CI → main 브랜치의 성공한 실행 → Artifacts**에서
`fell-and-sell-mod-v0.4.0`을 받으세요. 보관 기간은 30일입니다.
바깥 ZIP을 풀면 패치 ZIP·SHA-256·릴리스 노트가 나옵니다. 체크섬을 확인한 뒤
**안쪽 패치 ZIP**을 게임 폴더에 풉니다. Source code ZIP은 설치용이 아닙니다.
MelonLoader는 별도로 설치하고 Steam으로 실행하세요.

## Releases and build automation

CI runs on main pushes, PRs and manual dispatch. Release runs for existing
`vX.Y.Z` tags, validates tag/code/changelog agreement and builds through the same
reusable workflow. It attaches the ZIP and checksum to a **draft prerelease**.
Drafts are visible only to repository collaborators until a maintainer publishes
them; the workflow does not automatically publish or overwrite published assets.

There is no public release merely because a tag or green CI run exists.
[Release procedure](RELEASING.md) describes maintainer review and publication.
[Installation](INSTALLATION.md) covers hashes, updates and removal;
[한국어 안내](ko/README.md) provides the user guide in Korean.
