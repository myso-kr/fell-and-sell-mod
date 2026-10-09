param([string]$Configuration = 'Release')
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
& (Join-Path $PSScriptRoot 'verify.ps1')
dotnet build (Join-Path $projectRoot 'FellAndSell.Mod.sln') -c $Configuration --nologo
if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }
[xml]$props = Get-Content -LiteralPath (Join-Path $projectRoot 'Directory.Build.props')
$version = $props.Project.PropertyGroup.Version
$dist = Join-Path $projectRoot 'dist'
$stage = Join-Path $dist ('stage-' + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path (Join-Path $stage 'Mods'), (Join-Path $stage 'UserData/FellAndSell/locale/ko') -Force | Out-Null
New-Item -ItemType Directory -Path (Join-Path $stage 'UserData/FellAndSell/fonts') -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $projectRoot "src/FellAndSell.Mod/bin/$Configuration/net6.0/FellAndSellMod.dll") -Destination (Join-Path $stage 'Mods')
Copy-Item -LiteralPath (Join-Path $projectRoot 'locale/ko/strings.json') -Destination (Join-Path $stage 'UserData/FellAndSell/locale/ko')
foreach ($fontFile in @('NotoSansCJKkr-Regular.otf', 'OFL-Noto.txt')) {
    Copy-Item -LiteralPath (Join-Path $projectRoot "locale/fonts/$fontFile") -Destination (Join-Path $stage 'UserData/FellAndSell/fonts')
}
foreach ($document in @('README.md', 'CHANGELOG.md', 'LICENSE', 'NOTICE', 'THIRD-PARTY.md')) {
    Copy-Item -LiteralPath (Join-Path $projectRoot $document) -Destination $stage
}
$archive = Join-Path $dist "fell-and-sell-mod-v$version.zip"
Compress-Archive -Path (Join-Path $stage '*') -DestinationPath $archive -Force
$hash = (Get-FileHash -LiteralPath $archive -Algorithm SHA256).Hash.ToLowerInvariant()
Set-Content -LiteralPath ($archive + '.sha256') -Value ($hash + '  ' + (Split-Path $archive -Leaf)) -Encoding utf8NoBOM
Write-Output "[OK] $archive"
