# Frequently Asked Questions (FAQ)

Find quick solutions. Use <kbd>Ctrl</kbd> + <kbd>F</kbd> to search for keywords.

## Installation & Running {#installation}

### Runtime prompts "Administrator privileges required"?

**A**: The following features require administrator privileges:
- GPU temperature/load monitoring
- Disk SMART data reading
- Some hardware information collection

**Solution**:
1. Right-click the application and select "Run as administrator"
2. Or configure `"runAsAdministrator": true` in `.vscode/launch.json`

### Startup prompts missing .NET Runtime?

**A**: The framework-dependent version (FDD) requires .NET 10 Runtime.

**Solutions**:

| Method | Steps |
|--------|-------|
| **Method 1** | Download the self-contained version (SCD), no .NET required |
| **Method 2** | Install .NET Desktop Runtime 10 from [Microsoft](https://dotnet.microsoft.com/download) |

### Windows Defender reports the app as a virus?

**A**: This is a false positive because the app uses WMI and hardware monitoring APIs.

**Solutions**:
1. Add an exclusion in Windows Defender
2. Verify the installer with [VirusTotal](https://www.virustotal.com)
3. Temporarily disable real-time protection if needed

## Features {#features}

### Why does GPU temperature show N/A?

**A**: Possible causes:

| Cause | Solution |
|-------|----------|
| Not running as administrator | Run the app as administrator |
| GPU driver not supported | Update graphics drivers |
| Virtual machine environment | GPU monitoring may not work in VMs |
| LibreHardwareMonitor doesn't support | Check [compatibility list](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor) |

### SMART data incomplete?

**A**: Some disk controllers or external drives may not support SMART.

**Supported Disk Types**:
- ✅ SATA SSD/HDD
- ✅ NVMe SSD
- ⚠️ Some USB external drives (depends on controller)

### Reliability records are empty?

**A**: Possible causes:
1. Windows Reliability Monitor data has been cleared
2. System recently reinstalled, no history
3. Query time range too small

**Solutions**:
- Adjust "Days back" parameter (default 7 days, max 90)
- Check if Reliability Monitor has data in Control Panel
- Verify Event Log service is running

### Exported HTML report opens slowly?

**A**: Large number of event logs can cause large HTML files.

**Optimization**:
1. Reduce "Max events" setting (default 1000)
2. Use JSON format export (smaller, faster)
3. Use modern browsers (Chrome/Edge/Firefox)
4. Reduce days back range

## Troubleshooting {#troubleshooting}

### App crashes on startup?

**A**: Troubleshooting steps:

**Step 1**: Check logs
```
%LOCALAPPDATA%\DigYourWindows\Logs\DigYourWindows_YYYYMMDD.log
```

**Step 2**: Common issues and fixes
| Symptom | Cause | Solution |
|---------|-------|----------|
| Exits immediately | .NET version mismatch | Reinstall .NET 10 SDK |
| UI freeze on load | Hardware monitor init failed | Run as administrator |
| Out of memory | System memory low | Close other applications |

**Step 3**: Reset app
```powershell
# Delete config file (resets all settings)
Remove-Item -Path "$env:LOCALAPPDATA\DigYourWindows\settings.json" -Force
```

### CPU/GPU monitoring values not updating?

**A**: Monitor service may not be initialized properly.

**Troubleshooting**:
1. Restart app as administrator
2. Check logs for errors (search "HardwareMonitor")
3. Verify LibreHardwareMonitor dependencies loaded correctly
4. Try disabling and re-enabling monitoring (click refresh)

### Import JSON file failed?

**A**: JSON file format may be incorrect or version incompatible.

**Solutions**:
1. Confirm JSON from same or compatible version
2. Validate JSON format with [JSONLint](https://jsonlint.com)
3. Check logs for specific errors
4. Verify file is complete (not truncated)

## Development {#development}

### How to debug hardware monitoring?

**A**: Debug methods:

**Method 1: Visual Studio Debugging**
1. Start Visual Studio as administrator
2. Set breakpoints in `CpuMonitorService.cs` or `GpuMonitorService.cs`
3. Press F5 to start debugging

**Method 2: VS Code Configuration**
```json
{
  "name": "Debug Hardware",
  "type": "coreclr",
  "request": "launch",
  "program": "${workspaceFolder}/src/DigYourWindows.UI/bin/Debug/net10.0-windows/DigYourWindows.UI.dll",
  "windows": { "runAsAdministrator": true }
}
```

### Where is the test coverage report?

**A**: Coverage report generation steps:

```powershell
# 1. Collect coverage
dotnet test --collect:"XPlat Code Coverage"

# 2. Report location
tests/DigYourWindows.Tests/TestResults/{guid}/coverage.cobertura.xml

# 3. Generate HTML (optional)
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage_report
```

### How to add new hardware monitoring metrics?

**Steps**:
1. Enable hardware type in `HardwareMonitorProvider`
2. Add collection logic in corresponding service
3. Update data models
4. Add unit tests

## Other {#other}

### Supported operating systems?

**A**:

| OS | Support Status | Notes |
|----|----------------|-------|
| Windows 11 | ✅ Full Support | Recommended |
| Windows 10 (Build 19041+) | ✅ Full Support | Requires 20H1 or later |
| Windows 10 (Build < 19041) | ⚠️ Limited Support | Some features unavailable |
| Windows 7/8 | ❌ Not Supported | .NET 10 not supported |
| Linux/macOS | ❌ Not Supported | Windows-specific tool |

### Is data uploaded to the cloud?

**A**: **No**. All data is collected, stored, and exported locally:
- ✅ Data saved to local file system
- ✅ Export files controlled by user
- ✅ No remote server connections
- ✅ Open source, auditable

### How to get the latest version?

**A**:

**Method 1**: GitHub Releases (Recommended)
```
https://github.com/LessUp/dig-your-windows/releases/latest
```

**Method 2**: Build from source
```powershell
git pull
dotnet build
dotnet run
```

### How to report issues or suggestions?

| Channel | Purpose | Link |
|---------|---------|------|
| GitHub Issues | Bug reports, feature requests | [Create Issue](https://github.com/LessUp/dig-your-windows/issues/new/choose) |
| GitHub Discussions | General discussion | [Discussions](https://github.com/LessUp/dig-your-windows/discussions) |

**Include in bug reports**:
- OS version
- .NET version (`dotnet --version`)
- App version (check About page)
- Reproduction steps
- Relevant log snippets

## Quick Reference {#reference}

### Keyboard Shortcuts

| Shortcut | Function |
|----------|----------|
| <kbd>F5</kbd> | Refresh diagnostic data |
| <kbd>Ctrl</kbd> + <kbd>S</kbd> | Save report |
| <kbd>Ctrl</kbd> + <kbd>T</kbd> | Toggle theme |
| <kbd>Esc</kbd> | Cancel ongoing collection |

### Health Score Grades

| Score | Grade | Color | Status | Recommendation |
|-------|-------|-------|--------|---------------|
| 90-100 | Excellent | `#28a745` 🟢 | Optimal | Maintain status |
| 75-89 | Good | `#17a2b8` 🔵 | Normal | Regular maintenance |
| 60-74 | Fair | `#ffc107` 🟡 | Some issues | Monitor closely |
| 40-59 | Poor | `#fd7e14` 🟠 | Needs attention | Optimize soon |
| < 40 | Needs Optimization | `#dc3545` 🔴 | Critical | Immediate action |

---

**Can't find an answer?** [Create an Issue](https://github.com/LessUp/dig-your-windows/issues/new) or check the [full documentation](/en-US/guide/).
