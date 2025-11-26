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

## Documentation

📋 **[DEVELOPMENT_PLAN.md](DEVELOPMENT_PLAN.md)** - Complete development specification with:
- Detailed architecture design
- Data models and structure
- UI/UX mockups
- Phase-by-phase implementation guide
- Technical implementation details
- Development standards
- Timeline and milestones

## Quick Links

- **Example incident.json**: https://raw.githubusercontent.com/Tools4everBV/HelloID-Conn-Prov-Target-Topdesk/refs/heads/main/permissions/incident/example.incident.json
- **Example change.json**: https://raw.githubusercontent.com/Tools4everBV/HelloID-Conn-Prov-Target-Topdesk/refs/heads/main/permissions/change/example.change.json

## Development Timeline

- **Phase 1**: Foundation (Week 1) - Project setup, data models
- **Phase 2**: Core Services (Week 1-2) - File I/O, validation
- **Phase 3**: ViewModels (Week 2) - MVVM implementation
- **Phase 4**: UI Implementation (Week 3) - WPF views
- **Phase 5**: Integration & Testing (Week 3-4) - End-to-end testing
- **Phase 6**: Polish & Documentation (Week 4) - Final release

**Estimated Total Time**: 6 weeks

## Team Setup Checklist

- [ ] Read DEVELOPMENT_PLAN.md thoroughly
- [ ] Set up development environment (VS 2022, .NET 8)
- [ ] Create Git repository and branches (main, develop, feature/*)
- [ ] Assign Phase 1 tasks to developers
- [ ] Schedule daily standup meetings
- [ ] Review and approve architecture decisions

## License

[Your License Here]
