# TOPdesk Permissions Editor

A Windows desktop application for managing TOPdesk incident and change request permission JSON files.

## Overview

This application provides a user-friendly GUI editor for HelloID-Topdesk integration permission files, eliminating the need to manually edit JSON files in text editors.

**Supported File Types:**
- `incident.json` - Incident request permissions
- `change.json` - Change request permissions

## Features

- Open and edit JSON permission files
- CRUD operations (Create, Read, Update, Delete)
- Form-based editing with validation
- Search and filter entries
- Duplicate entries as templates
- Maintain proper JSON structure

## Technology Stack

- **.NET 8** (WPF Desktop Application)
- **MVVM Pattern** (Model-View-ViewModel)
- **System.Text.Json** for serialization
- **MaterialDesign** or **ModernWPF** for UI

## Getting Started

### Prerequisites

- Visual Studio 2022 or later
- .NET 8 SDK
- Windows 10/11

### Development Setup

1. Clone the repository
2. Open `TopdeskPermissionsEditor.sln` in Visual Studio
3. Restore NuGet packages
4. Build and run

## Project Structure

```
TopdeskPermissionsEditor/
├── TopdeskPermissionsEditor.Core/      # Business logic and models
├── TopdeskPermissionsEditor.UI/        # WPF application
└── TopdeskPermissionsEditor.Tests/     # Unit tests
```

## Quick Links

- **Example incident.json**: https://raw.githubusercontent.com/Tools4everBV/HelloID-Conn-Prov-Target-Topdesk/refs/heads/main/permissions/incident/example.incident.json
- **Example change.json**: https://raw.githubusercontent.com/Tools4everBV/HelloID-Conn-Prov-Target-Topdesk/refs/heads/main/permissions/change/example.change.json

## License

[Your License Here]
