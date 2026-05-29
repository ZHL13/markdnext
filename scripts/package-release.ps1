[CmdletBinding()]
param(
    [string]$Version
)

$ErrorActionPreference = 'Stop'
$root = Resolve-Path (Join-Path $PSScriptRoot '..')
$project = Join-Path $root 'MarkDNext.csproj'
$dist = Join-Path $root 'dist'
$localDotnet = Join-Path $root '.dotnet\dotnet.exe'
$dotnet = if (Test-Path -LiteralPath $localDotnet) { $localDotnet } else { 'dotnet' }

if (-not $Version) {
    [xml]$projectXml = Get-Content -LiteralPath $project
    $Version = ($projectXml.Project.PropertyGroup | Where-Object { $_.Version } | Select-Object -First 1).Version
}

if (-not $Version) {
    throw 'Could not determine package version from MarkDNext.csproj.'
}

& $dotnet publish $project -c Release -r win-x64 --self-contained true
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

$source = Join-Path $dist 'MarkDNext.exe'
$targetName = "MarkDNext-$Version-win-x64.exe"
$target = Join-Path $dist $targetName
Copy-Item -LiteralPath $source -Destination $target -Force

Get-ChildItem -LiteralPath $dist -File |
    Where-Object { $_.Name -ne $targetName } |
    Remove-Item -Force

Get-Item -LiteralPath $target
