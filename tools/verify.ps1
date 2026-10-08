$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path $PSScriptRoot -Parent
$catalog = Get-Content -LiteralPath (Join-Path $projectRoot 'locale/ko/strings.json') -Raw | ConvertFrom-Json -AsHashtable
if ($catalog -isnot [System.Collections.IDictionary]) { throw 'Catalog must be a JSON object.' }
foreach ($entry in $catalog.GetEnumerator()) {
    if ($entry.Value -isnot [string]) { throw "Translation must be a string: $($entry.Key)" }
}
Get-ChildItem -LiteralPath $PSScriptRoot -Filter '*.ps1' | ForEach-Object {
    $parseErrors = $null
    $parseTokens = $null
    [void][System.Management.Automation.Language.Parser]::ParseFile($_.FullName, [ref]$parseTokens, [ref]$parseErrors)
    if ($parseErrors) { throw ($parseErrors | Out-String) }
}
Write-Output '[OK] Translation JSON and PowerShell syntax verified.'
