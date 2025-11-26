# Increment Version Script
# Usage: .\increment-version.ps1 [major|minor|patch]

param(
    [ValidateSet('major', 'minor', 'patch')]
    [string]$Type = 'patch'
)

$csprojPath = "HelloID.JsonEditor.UI\HelloID.JsonEditor.UI.csproj"

if (-not (Test-Path $csprojPath)) {
    Write-Error "Project file not found: $csprojPath"
    exit 1
}

# Read the project file
$xml = [xml](Get-Content $csprojPath)

# Find the Version element
$versionNode = $xml.Project.PropertyGroup | Where-Object { $_.Version } | Select-Object -First 1

if (-not $versionNode) {
    Write-Error "Version property not found in project file"
    exit 1
}

$currentVersion = $versionNode.Version
Write-Host "Current version: $currentVersion"

# Parse version
if ($currentVersion -match '^(\d+)\.(\d+)\.(\d+)$') {
    $major = [int]$Matches[1]
    $minor = [int]$Matches[2]
    $patch = [int]$Matches[3]
} else {
    Write-Error "Invalid version format: $currentVersion"
    exit 1
}

# Increment based on type
switch ($Type) {
    'major' {
        $major++
        $minor = 0
        $patch = 0
    }
    'minor' {
        $minor++
        $patch = 0
    }
    'patch' {
        $patch++
    }
}

$newVersion = "$major.$minor.$patch"
Write-Host "New version: $newVersion" -ForegroundColor Green

# Update all version fields
$versionNode.Version = $newVersion
$versionNode.AssemblyVersion = "$newVersion.0"
$versionNode.FileVersion = "$newVersion.0"
$versionNode.InformationalVersion = $newVersion

# Save the file
$xml.Save((Resolve-Path $csprojPath))

Write-Host "Version updated successfully!" -ForegroundColor Green
Write-Host "Don't forget to commit this change and create a git tag:"
Write-Host "  git add $csprojPath"
Write-Host "  git commit -m 'Bump version to $newVersion'"
Write-Host "  git tag v$newVersion"
Write-Host "  git push origin main --tags"
