---
layout: default
title: "Download"
description: "Get the packaged Fell & Sell preview from a successful GitHub Actions build. Includes ZIP extraction steps and release availability."
lang: "en"
permalink: "/downloads/"
page_id: "downloads"
page_type: "article"
summary: "The current patch is available as a preview build."
---

<p class="eyebrow">v{{ site.data.patch.version }} preview · Windows · MelonLoader {{ site.data.patch.loader_version }} x64</p>
<div class="actions"><a class="button primary download-cta" href="{{ site.data.patch.builds_url }}">Find a successful build</a></div>
<p class="note">GitHub sign-in required · Build files retained for 30 days</p>

## Download in your browser

1. Follow the button above and sign in to GitHub.
2. Open a **main build with a green check**. If the latest run failed, choose an earlier successful one.
3. Under **Artifacts**, download `fell-and-sell-mod-v{{ site.data.patch.version }}`. Check the build's version information if it provides a different version.
4. Extract the downloaded outer ZIP first. The `fell-and-sell-mod-vX.Y.Z.zip` inside is the installable package; its `.zip.sha256` file is a checksum.
5. Extract the **inner package ZIP** and put its `Mods` and `UserData` folders in the game folder.

<div class="notice" markdown="1">
**Which ZIP should I install?** The outer ZIP is GitHub's delivery wrapper. The inner ZIP contains `Mods/FellAndSellMod.dll` and `UserData/FellAndSell/`. GitHub's **Code → Download ZIP** contains source code and cannot be installed as the mod.
</div>
<div class="actions"><a class="button" href="{{ '/installation/' | relative_url }}">Next: installation</a></div>

## Missing or unavailable files

If Artifacts is missing, check that the run has finished successfully and that you are signed in. Expired files require a more recent successful build. See [download troubleshooting]({{ '/help/#download' | relative_url }}) for extraction problems.

## Public releases

There is no public release yet. Packaged releases will appear under [Releases]({{ site.data.patch.releases_url }}) when available. Use the preview build above for now.

<details markdown="1">
<summary>Optional: download with GitHub CLI</summary>

Sign in with `gh auth login`, then run this in PowerShell. Choose a successful main run ID from the first command's list.

```powershell
gh run list --repo myso-kr/fell-and-sell-mod `
  --workflow ci.yml --branch main --status success --limit 5
$runId = Read-Host 'Run ID to download'
gh run download $runId --repo myso-kr/fell-and-sell-mod `
  --name fell-and-sell-mod-v{{ site.data.patch.version }} `
  --dir .\patch-download
```

The CLI extracts the outer archive for you. Install the package ZIP inside `patch-download` using the [installation guide]({{ '/installation/' | relative_url }}). For another version, change the artifact name to match the build.
</details>
