# Publishing to NuGet

This document explains how to publish the Uk.Parliament package to NuGet.org.

## Prerequisites

1. **NuGet API Key**
   - Get your API key from https://www.nuget.org/account/apikeys
   - Create a file named `nuget-key.txt` in the root directory
   - Paste your API key into this file (single line, no quotes)
   - This file is `.gitignore`d and will not be committed

2. **PowerShell 7+**
   - The publish script requires PowerShell 7 or later
   - Download from: https://github.com/PowerShell/PowerShell

3. **Clean Git State (REQUIRED)**
   - All changes **must** be committed
   - Working directory **must** be clean
   - This is required for Nerdbank.GitVersioning (nbgv) to work correctly
   - Cannot be bypassed

4. **nbgv (Nerdbank.GitVersioning)**
   - Version is managed automatically by nbgv
   - Based on `version.json` and Git commit height
   - Install: `dotnet tool install -g nbgv`

## Quick Start

### Standard Publish (Recommended)

```powershell
# 1. Commit all changes
git add .
git commit -m "Prepare for release"

# 2. Publish
.\Publish.ps1
```

This will:
1. ? Check for NuGet API key
2. ? Verify Git working directory is clean (REQUIRED)
3. ? Get version from nbgv
4. ? Run all unit tests
5. ? Build in Release mode
6. ? Create NuGet package
7. ? Prompt for confirmation
8. ? Publish to NuGet.org

### Quick Publish (Skip Tests)

```powershell
.\Publish.ps1 -SkipTests
```

**?? Warning:** Only use this if you're confident tests pass or for emergency hotfixes.

### Dry Run (Test Without Publishing)

```powershell
.\Publish.ps1 -DryRun
```

This performs all steps except the actual publish. Useful for:
- Testing the build process
- Verifying package creation
- Checking version numbers

## Command-Line Options

| Parameter | Description | Example |
|-----------|-------------|---------|
| `-SkipTests` | Skip running unit tests | `.\Publish.ps1 -SkipTests` |
| `-DryRun` | Test without publishing | `.\Publish.ps1 -DryRun` |

**Note:** There is no `-SkipGitCheck` option. Clean Git state is **required** for nbgv versioning.

### Combining Options

```powershell
# Quick dry run
.\Publish.ps1 -SkipTests -DryRun
```

## Publishing Checklist

Before publishing, ensure:

- [ ] All changes are committed to Git (**REQUIRED**)
- [ ] Working directory is clean (**REQUIRED**)
- [ ] Version number is correct in `version.json`
- [ ] CHANGELOG.md is updated with release notes
- [ ] All tests pass locally
- [ ] Release notes are prepared
- [ ] You have reviewed the changes since last release

## First-Time Setup

1. **Install nbgv**

   ```powershell
   dotnet tool install -g nbgv
   ```

2. **Create NuGet API Key File**

   ```powershell
   # Copy the example file
   Copy-Item nuget-key.txt.example nuget-key.txt
   
   # Edit nuget-key.txt and replace with your actual key
   notepad nuget-key.txt
   ```

3. **Verify Configuration**

   ```powershell
   # Check current version
   nbgv get-version
   
   # Test with dry run
   .\Publish.ps1 -DryRun
   ```

4. **First Publish**

   ```powershell
   # Commit everything first
   git add .
   git commit -m "Prepare for v10.0.0 release"
   
   # Standard publish
   .\Publish.ps1
   ```

## What Happens During Publish

### 1. Pre-flight Checks
- ? Verifies `nuget-key.txt` exists and contains a key
- ? Checks Git status is clean (**REQUIRED**)
- ? Stops if there are uncommitted changes

### 2. Version Detection
- ? Uses nbgv to get version from Git
- ? Version based on `version.json` + Git commit height
- ? Shows version, assembly version, and Git height

### 3. Testing
- ? Runs all unit tests in Release mode (unless `-SkipTests`)
- ? Stops if any test fails
- ? Shows test summary

### 4. Build & Package
- ? Cleans previous builds
- ? Builds project in Release mode
- ? Creates NuGet package (`.nupkg`)
- ? Creates symbols package (`.snupkg`)
- ? Outputs to `./nupkg/` directory

### 5. Publish
- ? Shows package details (name, version, size)
- ? Prompts for confirmation (unless `-DryRun`)
- ? Pushes to NuGet.org
- ? Displays package URL

### 6. Post-Publish
- ?? Package appears on NuGet.org within minutes
- ?? May take longer to appear in search results
- ?? Symbols are automatically published to SymbolSource

## Troubleshooting

### "NuGet API key file not found"

**Solution:**
```powershell
# Create the key file
Copy-Item nuget-key.txt.example nuget-key.txt

# Add your key
notepad nuget-key.txt
```

### "Git working directory is not clean"

**This is REQUIRED. You MUST commit your changes.**

**Solution:**
```powershell
# Commit your changes
git add .
git commit -m "Prepare for release"

# Then publish
.\Publish.ps1
```

**Alternative (temporary stash):**
```powershell
# Stash changes
git stash

# Publish
.\Publish.ps1

# Restore changes
git stash pop
```

**Why is this required?**
- Nerdbank.GitVersioning (nbgv) needs a clean Git state
- Version numbers are based on Git commits
- Ensures reproducible builds
- Package version matches Git state

### "Tests failed"

**Solutions:**
```powershell
# Run tests to see failures
dotnet test

# Fix the failing tests, then retry
.\Publish.ps1

# Emergency: Skip tests (not recommended)
.\Publish.ps1 -SkipTests
```

### "Package with version X.Y.Z already exists"

**Solution:**
```powershell
# Make another commit to increment version
git commit --allow-empty -m "Bump version"

# Or update version.json for major/minor bump
nbgv set-version 10.1.0
git add version.json
git commit -m "Bump version to 10.1.0"
```

### "401 Unauthorized" Error

**Causes:**
- Invalid or expired API key
- Key doesn't have push permissions

**Solutions:**
1. Generate a new API key at https://www.nuget.org/account/apikeys
2. Ensure key has "Push" permission
3. Update `nuget-key.txt` with new key

### "Could not get version from nbgv"

**Solution:**
```powershell
# Install nbgv globally
dotnet tool install -g nbgv

# Verify installation
nbgv --version

# Check version.json exists
Get-Content version.json
```

## Package Versioning

The package version is managed by **Nerdbank.GitVersioning (nbgv)** using `version.json`.

### How Versioning Works

1. **Base Version** - Defined in `version.json`
   ```json
   {
     "version": "10.0"
   }
   ```

2. **Git Commit Height** - Auto-incremented build number
   - Based on commits since last version change
   - Automatically managed by nbgv

3. **Final Version** - `{major}.{minor}.{gitHeight}`
   - Example: `10.0.42` (42nd commit)

### View Current Version

```powershell
# Simple version
nbgv get-version

# Detailed info (JSON)
nbgv get-version --format json
```

### Update Version

```powershell
# Set new major/minor version
nbgv set-version 10.1

# Commit the change
git add version.json
git commit -m "Bump version to 10.1"

# Next package will be 10.1.0, then 10.1.1, etc.
```

### Version Format

- **10.x.y** - Major version (10) tracks .NET version
- **10.0.x** - Minor version (0) for feature sets
- **10.0.42** - Patch/Build (42) auto-incremented by Git height

## Security Best Practices

1. **Never Commit API Keys**
   - `nuget-key.txt` is in `.gitignore`
   - Double-check before committing
   - Review files: `git status`

2. **Rotate Keys Periodically**
   - Regenerate API key every 6-12 months
   - Update `nuget-key.txt` with new key

3. **Limit Key Permissions**
   - Only grant "Push" permission
   - Scope to specific packages if possible

4. **Use Separate Keys**
   - Different key per developer
   - Different key per project

5. **Commit Everything Before Publishing**
   - Ensures version matches code
   - Enables reproducible builds
   - Required for nbgv

## Advanced Usage

### Testing Locally Before Publishing

```powershell
# Create package only
dotnet pack Uk.Parliament\Uk.Parliament.csproj --configuration Release --output ./nupkg

# Install locally from folder
dotnet add package Uk.Parliament --source ./nupkg
```

### Checking What Will Be Published

```powershell
# Dry run to see version and package
.\Publish.ps1 -DryRun

# Or check with nbgv
nbgv get-version
```

### Publishing After Version Bump

```powershell
# 1. Update version
nbgv set-version 10.1

# 2. Commit
git add version.json
git commit -m "Bump to v10.1"

# 3. Publish
.\Publish.ps1

# Result: Package version will be 10.1.0
```

## Getting Help

- **NuGet Documentation:** https://docs.microsoft.com/en-us/nuget/
- **nbgv Documentation:** https://github.com/dotnet/Nerdbank.GitVersioning
- **Package Page:** https://www.nuget.org/packages/Uk.Parliament/
- **Issues:** https://github.com/panoramicdata/Uk.Parliament/issues

## See Also

- [MASTER_PLAN.md](MASTER_PLAN.md) - Overall project plan
- [CHANGELOG.md](CHANGELOG.md) - Release history
- [version.json](version.json) - Version configuration
