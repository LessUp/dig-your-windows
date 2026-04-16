# Getting Started

This guide helps you quickly set up your development environment and run DigYourWindows.

## Requirements

### Required Dependencies

| Dependency | Version | Purpose | Download |
|------------|---------|---------|----------|
| Windows | 10/11 (Build 19041+) | Runtime platform | - |
| .NET SDK | 10.0.x | Build and run | [Download](https://dotnet.microsoft.com/download) |
| IDE (optional) | VS 2022 / Rider / VS Code | Development | - |

### Verify Installation

::: code-group

```powershell [PowerShell]
# Check .NET version (should show 10.0.x)
dotnet --version

# Check Windows version
[Environment]::OSVersion.Version
```

```cmd [CMD]
# Check .NET version
dotnet --version

# Check Windows version
ver
```

:::

## Get the Source Code

### Option 1: Clone Repository (Recommended)

```powershell
# Using HTTPS
git clone https://github.com/LessUp/dig-your-windows.git

# Or using SSH
git clone git@github.com:LessUp/dig-your-windows.git

# Enter project directory
cd dig-your-windows
```

### Option 2: Download ZIP

```powershell
# Download latest source
Invoke-WebRequest -Uri "https://github.com/LessUp/dig-your-windows/archive/refs/heads/master.zip" -OutFile "dig-your-windows.zip"

# Extract
Expand-Archive -Path "dig-your-windows.zip" -DestinationPath "."
```

## Build the Project

### 1. Restore Dependencies

```powershell
dotnet restore DigYourWindows.slnx
```

::: tip NuGet Configuration
The project uses the official NuGet source. To configure proxy or private sources, edit `NuGet.Config`.
:::

### 2. Compile

::: code-group

```powershell [Debug Version]
dotnet build DigYourWindows.slnx
```

```powershell [Release Version]
dotnet build DigYourWindows.slnx -c Release
```

:::

Build output locations:
- Debug: `src/DigYourWindows.UI/bin/Debug/net10.0-windows/`
- Release: `src/DigYourWindows.UI/bin/Release/net10.0-windows/`

## Run the Application

### Basic Run

```powershell
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

### Run as Administrator

::: warning Important Note
The following features require administrator privileges:
- GPU temperature/load monitoring
- Disk SMART data reading
- Some hardware information collection
:::

#### Method 1: Right-click Run as Administrator

1. Open File Explorer, navigate to `src/DigYourWindows.UI/bin/Debug/net10.0-windows/`
2. Right-click `DigYourWindows.UI.exe`
3. Select "Run as administrator"

#### Method 2: VS Code Configuration

Add admin configuration in `.vscode/launch.json`:

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "Run as Admin",
      "type": "coreclr",
      "request": "launch",
      "program": "${workspaceFolder}/src/DigYourWindows.UI/bin/Debug/net10.0-windows/DigYourWindows.UI.dll",
      "args": [],
      "cwd": "${workspaceFolder}",
      "stopAtEntry": false,
      "console": "internalConsole",
      "windows": {
        "runAsAdministrator": true
      }
    }
  ]
}
```

#### Method 3: Visual Studio

1. Open the project in Visual Studio
2. Right-click `DigYourWindows.UI` project → "Properties"
3. Go to "Debug" tab
4. Check "Enable native code debugging" (optional)
5. Start Visual Studio as administrator

## Run Tests

### Run All Tests

```powershell
dotnet test DigYourWindows.slnx
```

### Detailed Output

```powershell
dotnet test DigYourWindows.slnx --logger "console;verbosity=detailed"
```

### Filter Tests

```powershell
# Filter by class name
dotnet test --filter "FullyQualifiedName~ReportServiceTests"

# Filter by method name
dotnet test --filter "FullyQualifiedName~SerializeToJson"
```

### Code Coverage

```powershell
# Collect coverage data
dotnet test --collect:"XPlat Code Coverage"

# Generate HTML report with ReportGenerator
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage
```

Coverage report location: `tests/DigYourWindows.Tests/TestResults/{guid}/coverage.cobertura.xml`

## Publish the Application

### Use Publish Scripts (Recommended)

```powershell
# Generate framework-dependent (FDD) and self-contained (SCD) versions
.\scripts\publish.ps1
```

Output directories:
- `artifacts/publish/FDD/` — Framework-dependent (64MB, requires .NET Runtime)
- `artifacts/publish/SCD/` — Self-contained (185MB, no dependencies)

### Build Installer

::: info Prerequisites
Inno Setup 6 must be installed: [Download](https://jrsoftware.org/isinfo.php)
:::

```powershell
.\scripts\build-installer.ps1
```

Output: `artifacts/installer/DigYourWindows_Setup.exe`

### Manual Publishing

#### Framework-Dependent

```powershell
dotnet publish src/DigYourWindows.UI/DigYourWindows.UI.csproj `
  -c Release `
  -o artifacts/publish/FDD
```

#### Self-Contained

```powershell
dotnet publish src/DigYourWindows.UI/DigYourWindows.UI.csproj `
  -c Release `
  -r win-x64 `
  --self-contained true `
  -p:PublishSingleFile=true `
  -o artifacts/publish/SCD
```

## Release a New Version

### Automatic Release Process

Push a `v*` format Git tag to automatically trigger GitHub Actions build and release:

```powershell
# 1. Update version number in Directory.Build.props
# 2. Update CHANGELOG.md

# 3. Create and push tag
git add .
git commit -m "chore(release): prepare for v1.1.0"
git tag v1.1.0
git push origin master
git push origin v1.1.0
```

GitHub Actions will automatically:
1. 🏗️ Build FDD and SCD versions
2. 📦 Create installer
3. 📝 Create GitHub Release
4. ⬆️ Upload build artifacts

### Manual Release

See the [GitHub Releases](https://github.com/LessUp/dig-your-windows/releases) page to manually upload build artifacts.

## Troubleshooting

### Common Issues

#### Q: "Program cannot start because xxx.dll is missing"

**Solution:**
```powershell
# Re-restore dependencies
dotnet restore --force

# Clean and rebuild
dotnet clean
dotnet build
```

#### Q: GPU temperature shows N/A

**Solution:**
1. Run the application as administrator
2. Update graphics drivers
3. Check if LibreHardwareMonitor supports your GPU

#### Q: Warning MSB3270 during build

This is a mixed platform warning and doesn't affect functionality. To eliminate, ensure all projects use the same target platform.

#### Q: Test host not found error

**Solution:**
```powershell
# Reinstall test SDK
dotnet new install Microsoft.NET.Test.Sdk

# Or clean and rebuild
dotnet clean
dotnet build
```

### Log Location

Application logs are saved at this location for troubleshooting:

```
%LOCALAPPDATA%\DigYourWindows\Logs\
```

Log file naming format: `DigYourWindows_YYYYMMDD.log`

## Next Steps

- [Architecture](/en-US/guide/architecture) — Learn about technical architecture and design decisions
- [Testing Guide](/en-US/guide/testing) — Learn how to write and run tests
- [Contributing Guide](/en-US/guide/contributing) — Get involved in project development
- [Data Schema](/en-US/reference/data-schema) — View diagnostic data format
