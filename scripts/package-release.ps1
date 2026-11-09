[CmdletBinding()]
param(
    [string]$Version
)

$ErrorActionPreference = 'Stop'
$root = Resolve-Path (Join-Path $PSScriptRoot '..')
$project = Join-Path $root 'MarkDNext.csproj'
$dist = Join-Path $root 'dist'
$fddDist = Join-Path $root 'dist-fdd'
$localDotnet = Join-Path $root '.dotnet\dotnet.exe'
$dotnet = if (Test-Path -LiteralPath $localDotnet) { $localDotnet } else { 'dotnet' }

if (-not $Version) {
    [xml]$projectXml = Get-Content -LiteralPath $project
    $Version = ($projectXml.Project.PropertyGroup | Where-Object { $_.Version } | Select-Object -First 1).Version
}

if (-not $Version) {
    throw 'Could not determine package version from MarkDNext.csproj.'
}

# 1) Framework-dependent build (default): small + fast, requires the .NET 8 Desktop Runtime.
#    ReadyToRun is intentionally disabled here; R2R + single-file + framework-dependent crashes at startup.
if (Test-Path -LiteralPath $fddDist) {
    Remove-Item -LiteralPath $fddDist -Recurse -Force
}

& $dotnet publish $project -c Release -r win-x64 --self-contained false -o $fddDist
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

# 2) Portable build: self-contained single file (no .NET runtime required).
& $dotnet publish $project -c Release -r win-x64 --self-contained true -p:PublishReadyToRun=true
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

$fddName = "MarkDNext-$Version-win-x64.exe"
$fddLatest = 'MarkDNext-latest-win-x64.exe'
$scName = "MarkDNext-$Version-portable-win-x64.exe"
$scLatest = 'MarkDNext-latest-portable-win-x64.exe'

Copy-Item -LiteralPath (Join-Path $fddDist 'MarkDNext.exe') -Destination (Join-Path $dist $fddName) -Force
Copy-Item -LiteralPath (Join-Path $fddDist 'MarkDNext.exe') -Destination (Join-Path $dist $fddLatest) -Force
Copy-Item -LiteralPath (Join-Path $dist 'MarkDNext.exe') -Destination (Join-Path $dist $scName) -Force
Copy-Item -LiteralPath (Join-Path $dist 'MarkDNext.exe') -Destination (Join-Path $dist $scLatest) -Force

$keptNames = @($fddName, $fddLatest, $scName, $scLatest)
Get-ChildItem -LiteralPath $dist -File |
    Where-Object { $keptNames -notcontains $_.Name } |
    Remove-Item -Force

Get-Item -LiteralPath (Join-Path $dist $fddName), (Join-Path $dist $scName)
