param([Parameter(Mandatory)][string]$GameDir)
$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
$gameRoot = (Resolve-Path -LiteralPath $GameDir).Path
if (-not (Test-Path -LiteralPath (Join-Path $gameRoot 'Fell & Sell.exe'))) { throw 'Not a Fell & Sell game directory.' }
if (-not (Test-Path -LiteralPath (Join-Path $gameRoot 'MelonLoader/net6/MelonLoader.dll'))) { throw 'Install MelonLoader separately first.' }
if (Get-CimInstance Win32_Process | Where-Object { $_.Name -eq 'Fell & Sell.exe' }) { throw 'Close the game before deploying.' }
dotnet build (Join-Path $projectRoot 'FellAndSell.Mod.sln') -c Release --nologo
if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }
New-Item -ItemType Directory -Path (Join-Path $gameRoot 'Mods'), (Join-Path $gameRoot 'UserData/FellAndSell/locale/ko'), (Join-Path $gameRoot 'UserData/FellAndSell/fonts') -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $projectRoot 'src/FellAndSell.Mod/bin/Release/net6.0/FellAndSellMod.dll') -Destination (Join-Path $gameRoot 'Mods') -Force
Copy-Item -LiteralPath (Join-Path $projectRoot 'locale/ko/strings.json') -Destination (Join-Path $gameRoot 'UserData/FellAndSell/locale/ko') -Force
foreach ($fontFile in @('NotoSansCJKkr-Regular.otf', 'OFL-Noto.txt')) {
    Copy-Item -LiteralPath (Join-Path $projectRoot "locale/fonts/$fontFile") -Destination (Join-Path $gameRoot 'UserData/FellAndSell/fonts') -Force
}
Write-Output '[OK] Mod, Korean catalog and licensed font deployed.'
