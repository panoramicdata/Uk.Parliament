#Requires -Version 7.0

<#
.SYNOPSIS
    Publishes the Uk.Parliament NuGet package after running validation checks.

.DESCRIPTION
    This script performs the following steps:
    1. Checks for uncommitted changes (git porcelain check) - REQUIRED for nbgv versioning
    2. Runs unit tests (optional - can be skipped)
    3. Builds the project in Release mode
    4. Creates the NuGet package
    5. Publishes to NuGet.org using API key from nuget-key.txt

.PARAMETER SkipTests
    Skip running unit tests before publishing.

.PARAMETER DryRun
    Perform all steps except the actual publish to NuGet.

.EXAMPLE
    .\Publish.ps1
    Standard publish with all checks

.EXAMPLE
    .\Publish.ps1 -SkipTests
    Quick publish without running tests

.EXAMPLE
    .\Publish.ps1 -DryRun
    Test the publish process without actually publishing
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false)]
    [switch]$SkipTests,

    [Parameter(Mandatory = $false)]
    [switch]$DryRun
)

# Set error action preference
$ErrorActionPreference = "Stop"

# Color functions for better output
function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
}

function Write-Error-Message {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor Red
}

function Write-Warning-Message {
    param([string]$Message)
    Write-Host "⚠  $Message" -ForegroundColor Yellow
}

function Write-Info {
    param([string]$Message)
    Write-Host "ℹ  $Message" -ForegroundColor Cyan
}

function Write-Step {
    param([string]$Message)
    Write-Host "`n▶ $Message" -ForegroundColor Blue
}

# Banner
Write-Host "`n========================================" -ForegroundColor Magenta
Write-Host "  UK Parliament .NET Library Publisher" -ForegroundColor Magenta
Write-Host "========================================`n" -ForegroundColor Magenta

# Get script directory
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptPath

# Configuration
$projectPath = "Uk.Parliament\Uk.Parliament.csproj"
$testProjectPath = "Uk.Parliament.Test\Uk.Parliament.Test.csproj"
$nugetKeyPath = "nuget-key.txt"

# Step 1: Check for NuGet API key
Write-Step "Checking for NuGet API key..."

if (-not (Test-Path $nugetKeyPath)) {
    Write-Error-Message "NuGet API key file not found: $nugetKeyPath"
    Write-Info "Create a file named 'nuget-key.txt' in the root directory with your NuGet API key."
    Write-Info "This file is already in .gitignore and will not be committed."
    exit 1
}

$nugetApiKey = Get-Content $nugetKeyPath -Raw | ForEach-Object { $_.Trim() }

if ([string]::IsNullOrWhiteSpace($nugetApiKey)) {
    Write-Error-Message "NuGet API key file is empty: $nugetKeyPath"
    exit 1
}

Write-Success "NuGet API key found"

# Step 2: Check Git status (REQUIRED - no skip option due to nbgv)
Write-Step "Checking Git status (required for nbgv versioning)..."

try {
    $gitStatus = git status --porcelain
    
    if ($gitStatus) {
        Write-Error-Message "Git working directory is not clean. Uncommitted changes detected:"
        Write-Host $gitStatus -ForegroundColor Yellow
        Write-Host ""
        Write-Warning-Message "This project uses Nerdbank.GitVersioning (nbgv) which requires a clean Git state."
        Write-Info "Please commit or stash your changes before publishing:"
        Write-Host ""
        Write-Host "  git add ." -ForegroundColor White
        Write-Host "  git commit -m 'Prepare for v10.0.0 release'" -ForegroundColor White
        Write-Host ""
        Write-Info "Or stash temporarily:"
        Write-Host ""
        Write-Host "  git stash" -ForegroundColor White
        Write-Host "  .\Publish.ps1" -ForegroundColor White
        Write-Host "  git stash pop" -ForegroundColor White
        exit 1
    }
    
    Write-Success "Git working directory is clean"
}
catch {
    Write-Error-Message "Could not check Git status: $_"
    Write-Info "Git is required for nbgv versioning."
    exit 1
}

# Step 3: Get version from nbgv
Write-Step "Getting version from nbgv..."

try {
    $versionOutput = nbgv get-version --format json | ConvertFrom-Json
    $version = $versionOutput.NuGetPackageVersion
    
    Write-Success "Version: $version"
    Write-Info "Assembly Version: $($versionOutput.AssemblyVersion)"
    Write-Info "Git Height: $($versionOutput.VersionHeight)"
}
catch {
    Write-Warning-Message "Could not get version from nbgv: $_"
    Write-Info "Continuing with version detection from package file..."
    $version = $null
}

# Step 4: Run tests
if (-not $SkipTests) {
    Write-Step "Running unit tests..."
    
    $testOutput = dotnet test $testProjectPath --configuration Release --verbosity normal
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error-Message "Tests failed! Please fix failing tests before publishing."
        exit 1
    }
    
    Write-Success "All tests passed"
}
else {
    Write-Warning-Message "Skipping tests (as requested)"
}

# Step 5: Clean previous builds
Write-Step "Cleaning previous builds..."

dotnet clean $projectPath --configuration Release --verbosity quiet

if ($LASTEXITCODE -ne 0) {
    Write-Error-Message "Failed to clean project"
    exit 1
}

Write-Success "Clean completed"

# Step 6: Build in Release mode
Write-Step "Building project in Release mode..."

dotnet build $projectPath --configuration Release --verbosity normal

if ($LASTEXITCODE -ne 0) {
    Write-Error-Message "Build failed!"
    exit 1
}

Write-Success "Build completed successfully"

# Step 7: Create NuGet package
Write-Step "Creating NuGet package..."

dotnet pack $projectPath --configuration Release --no-build --output ./nupkg

if ($LASTEXITCODE -ne 0) {
    Write-Error-Message "Package creation failed!"
    exit 1
}

Write-Success "NuGet package created"

# Step 8: Find the created package
$packageFiles = Get-ChildItem -Path ./nupkg -Filter "Uk.Parliament.*.nupkg" | Where-Object { $_.Name -notlike "*.symbols.nupkg" }

if ($packageFiles.Count -eq 0) {
    Write-Error-Message "No package file found in ./nupkg directory"
    exit 1
}

# Get the most recent package
$packageFile = $packageFiles | Sort-Object LastWriteTime -Descending | Select-Object -First 1

Write-Info "Package: $($packageFile.Name)"
Write-Info "Size: $([math]::Round($packageFile.Length / 1KB, 2)) KB"

# Extract version from filename if nbgv didn't work
if (-not $version) {
    if ($packageFile.Name -match 'Uk\.Parliament\.(\d+\.\d+\.\d+.*?)\.nupkg') {
        $version = $matches[1]
        Write-Info "Version: $version"
    }
}

# Step 9: Publish to NuGet
if (-not $DryRun) {
    Write-Step "Publishing to NuGet.org..."
    
    Write-Host ""
    Write-Host "  Package: Uk.Parliament" -ForegroundColor White
    Write-Host "  Version: $version" -ForegroundColor White
    Write-Host "  File: $($packageFile.Name)" -ForegroundColor White
    Write-Host ""
    
    $confirmation = Read-Host "Are you sure you want to publish to NuGet.org? (y/n)"
    
    if ($confirmation -ne 'y') {
        Write-Warning-Message "Publish cancelled by user"
        exit 0
    }
    
    dotnet nuget push $packageFile.FullName --api-key $nugetApiKey --source https://api.nuget.org/v3/index.json
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error-Message "Package publish failed!"
        exit 1
    }
    
    Write-Success "Package published successfully to NuGet.org!"
    Write-Info "View at: https://www.nuget.org/packages/Uk.Parliament/$version"
}
else {
    Write-Warning-Message "DRY RUN - Package NOT published to NuGet.org"
    Write-Info "Package is ready at: $($packageFile.FullName)"
    Write-Info "To publish, run without -DryRun flag"
}

# Summary
Write-Host "`n========================================" -ForegroundColor Magenta
Write-Host "  Publish Complete!" -ForegroundColor Magenta
Write-Host "========================================`n" -ForegroundColor Magenta

if (-not $DryRun) {
    Write-Success "Package published: Uk.Parliament v$version"
    Write-Info "It may take a few minutes to appear in NuGet search results."
    Write-Host ""
    Write-Info "Next steps:"
    Write-Host "  1. Create GitHub release: https://github.com/panoramicdata/Uk.Parliament/releases/new" -ForegroundColor White
    Write-Host "  2. Tag: v$version" -ForegroundColor White
    Write-Host "  3. Update CHANGELOG.md" -ForegroundColor White
}

exit 0
