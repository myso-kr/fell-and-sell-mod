$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
$catalog = Get-Content -LiteralPath (Join-Path $projectRoot 'locale/ko/strings.json') -Raw | ConvertFrom-Json -AsHashtable
if ($catalog -isnot [System.Collections.IDictionary]) { throw 'Catalog must be a JSON object.' }
foreach ($entry in $catalog.GetEnumerator()) {
    if ($entry.Key -notmatch '^(Game|UI)/[0-9]+$') { throw "Invalid table/ID key: $($entry.Key)" }
    if (($entry.Value -isnot [string]) -or [string]::IsNullOrWhiteSpace($entry.Value)) { throw "Translation must be a non-empty string: $($entry.Key)" }
}
Get-ChildItem -LiteralPath $PSScriptRoot -Filter '*.ps1' | ForEach-Object {
    $parseErrors = $null
    $parseTokens = $null
    [void][System.Management.Automation.Language.Parser]::ParseFile($_.FullName, [ref]$parseTokens, [ref]$parseErrors)
    if ($parseErrors) { throw ($parseErrors | Out-String) }
}
Write-Output '[OK] Translation JSON and PowerShell syntax verified.'
