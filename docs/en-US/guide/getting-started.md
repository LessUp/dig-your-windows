# Getting Started

This guide will help you set up and run DigYourWindows on your Windows machine.

## Prerequisites

Before you begin, ensure you have:

- **Windows 10/11** (Build 19041 or later)
- **.NET 10.0 SDK** (for building from source)
- **Administrator privileges** (for full feature access)

## Installation Options

### Option 1: Download Release (Recommended)

Download the latest installer from [GitHub Releases](https://github.com/LessUp/dig-your-windows/releases):

| Version | Size | Requirements | Best For |
|---------|------|--------------|----------|
| `DigYourWindows_Setup.exe` | ~5MB | Downloads .NET if needed | Most users |
| FDD (Framework-Dependent) | ~60MB | Requires .NET 10 Runtime | Users with .NET installed |
| SCD (Self-Contained) | ~180MB | No dependencies | Offline use |

#### Installation Steps

1. Download the installer
2. Run `DigYourWindows_Setup.exe`
3. Follow the installation wizard
4. Launch DigYourWindows from Start Menu

### Option 2: Build from Source

```powershell
# Clone repository
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# Restore dependencies
dotnet restore

# Build and run
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

> ⚠️ **Note**: Some features (GPU monitoring, SMART data) require administrator privileges.

## First Run

1. **Launch the application**:
   - From Start Menu, or
   - Run `DigYourWindows.UI.exe`, or
   - Use `dotnet run`

2. **Run diagnostics**:
   - Click "Run Diagnostics" button
   - Wait for data collection (typically 5-15 seconds)

3. **Review results**:
   - View hardware information on dashboard
   - Check event log analysis
   - Review reliability records
   - See health score and recommendations

4. **Export reports** (optional):
   - Click "Export" button
   - Choose JSON or HTML format
   - Select save location

## Administrator Privileges

Some features require administrator privileges:

- ✅ GPU temperature/load monitoring
- ✅ Disk SMART data reading
- ✅ Some hardware information collection

### How to Run as Administrator

**Method 1**: Right-click shortcut
- Right-click `DigYourWindows.UI.exe`
- Select "Run as administrator"

**Method 2**: Configure in Visual Studio
- Open project in Visual Studio
- Edit `.vscode/launch.json`
- Add `"runAsAdministrator": true`

## Troubleshooting

### "Administrator privileges required" message

**Solution**: Run the application as administrator (see above).

### Missing .NET Runtime

**Solution**: 
- Download the self-contained version (SCD), OR
- Install .NET Desktop Runtime 10 from [Microsoft](https://dotnet.microsoft.com/download)

### Windows Defender warning

**Solution**: 
- This is common for unsigned applications
- Add an exception in Windows Defender
- Or build from source yourself

## Next Steps

- 📖 Read the [Architecture Overview](./architecture.md)
- 🧪 Learn about [Testing](./testing.md)
- 🤝 Check out the [Contributing Guide](https://github.com/LessUp/dig-your-windows/blob/main/CONTRIBUTING.md)
- 📋 Review [Specifications](https://github.com/LessUp/dig-your-windows/tree/main/specs)
# Using DigYourWindows

This tutorial walks you through the main features of DigYourWindows.

## Table of Contents

1. [Dashboard Overview](#dashboard-overview)
2. [Running Diagnostics](#running-diagnostics)
3. [Viewing Hardware Information](#viewing-hardware-information)
4. [Analyzing Event Logs](#analyzing-event-logs)
5. [Checking Reliability Records](#checking-reliability-records)
6. [Understanding Health Scores](#understanding-health-scores)
7. [Exporting Reports](#exporting-reports)

## Dashboard Overview

When you launch DigYourWindows, you'll see the main dashboard with:

- **System Info**: Computer name, OS version, processor, memory
- **Quick Stats**: Current CPU/GPU metrics
- **Action Buttons**: Run Diagnostics, Export, Settings

## Running Diagnostics

1. Click **"Run Diagnostics"** button
2. Progress indicator shows collection status:
   - Collecting hardware information
   - Reading event logs
   - Analyzing reliability data
   - Calculating health scores
3. Wait for completion (typically 5-15 seconds)
4. Results display automatically

> 💡 **Tip**: You can cancel collection at any time using the Cancel button.

## Viewing Hardware Information

### CPU Information

The CPU section shows:
- Model name and specifications
- Core and thread count
- Real-time temperature and load
- Current and base frequency

### Memory

Memory information includes:
- Total and available memory
- Usage percentage
- Memory type and speed

### Disks

For each disk, you'll see:
- Model and interface type (NVMe/SATA)
- Health status
- Temperature (if supported)
- SMART data (requires admin)

### GPU

GPU monitoring displays:
- Graphics card model
- Temperature and load
- VRAM usage
- Clock speeds (if supported)

## Analyzing Event Logs

The Event Log Analysis section shows:

- **System Errors**: Critical system failures
- **System Warnings**: Potential issues
- **Application Errors**: Software crashes
- **Application Warnings**: Application issues

### Filtering Events

You can filter by:
- Date range (default: last 7 days)
- Severity (Error/Warning)
- Source application

## Checking Reliability Records

Windows Reliability Monitor data displays:
- Stability index trend (0-10 scale)
- Historical events timeline
- Critical events list

## Understanding Health Scores

### Overall Score

Composite score from 0-100 based on:
- **Stability Score**: System crash frequency
- **Performance Score**: Resource utilization
- **Memory Score**: RAM health
- **Disk Score**: Storage condition

### Recommendations

AI-generated suggestions include:
- Category (CPU/GPU/Memory/Disk/System)
- Priority level (Low/Medium/High/Critical)
- Actionable advice

## Exporting Reports

### JSON Format

Best for:
- Programmatic analysis
- Data archival
- Comparing over time

**Steps**:
1. Click "Export"
2. Select "JSON"
3. Choose save location
4. File named: `DigYourWindows_Report_[date].json`

### HTML Format

Best for:
- Sharing with others
- Offline viewing
- Printing

**Steps**:
1. Click "Export"
2. Select "HTML"
3. Choose save location
4. File named: `DigYourWindows_Report_[date].html`

> 📝 **Note**: HTML reports are self-contained with no external dependencies.

## Theme Toggle

Switch between Dark and Light themes:
- Click theme toggle button (top-right)
- Preference saved for next session

## Next Steps

- 🏗️ Learn about [Architecture](./architecture.md)
- 📋 View [Specifications](https://github.com/LessUp/dig-your-windows/tree/main/specs)
- 🤝 [Contribute](https://github.com/LessUp/dig-your-windows/blob/main/CONTRIBUTING.md)
