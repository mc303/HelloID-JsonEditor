# TOPdesk Permissions Editor - Development Plan

## 1. Application Overview

### Purpose
A Windows desktop application to manage TOPdesk incident and change request permission JSON files through a user-friendly GUI interface, replacing manual text editor usage.

### Target Users
- System administrators
- IT support staff
- Configuration managers working with HelloID-Topdesk integrations

### Core Functionality
- Open incident.json and change.json files
- Visual editor for permission configurations
- CRUD operations on permission entries
- Save changes back to JSON format
- Validation of required fields

---

## 2. Technical Architecture

### Application Type
**WPF (Windows Presentation Foundation) Desktop Application**

**Rationale:**
- Rich UI capabilities for form-based editing
- Excellent data binding support (MVVM pattern)
- Native Windows experience
- Strong support for complex layouts

### Technology Stack
- **.NET Version**: .NET 8 (LTS - Long Term Support)
- **UI Framework**: WPF
- **Architecture Pattern**: MVVM (Model-View-ViewModel)
- **JSON Serialization**: System.Text.Json
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection
- **UI Toolkit**: MaterialDesignThemes (recommended) or ModernWPF for modern UI

### Project Structure
```
TopdeskPermissionsEditor.sln
│
├── TopdeskPermissionsEditor.Core/          # Domain models and business logic
│   ├── Models/
│   │   ├── IncidentPermission.cs
│   │   ├── ChangePermission.cs
│   │   ├── GrantSection.cs
│   │   └── RevokeSection.cs
│   ├── Services/
│   │   ├── IFileService.cs
│   │   ├── JsonFileService.cs
│   │   └── ValidationService.cs
│   └── Enums/
│       ├── PermissionType.cs
│       ├── Priority.cs
│       └── Status.cs
│
├── TopdeskPermissionsEditor.UI/            # WPF Application
│   ├── ViewModels/
│   │   ├── MainViewModel.cs
│   │   ├── IncidentEditorViewModel.cs
│   │   ├── ChangeEditorViewModel.cs
│   │   └── PermissionItemViewModel.cs
│   ├── Views/
│   │   ├── MainWindow.xaml
│   │   ├── IncidentEditorView.xaml
│   │   ├── ChangeEditorView.xaml
│   │   └── PermissionItemView.xaml
│   ├── Converters/
│   │   └── BoolToVisibilityConverter.cs
│   └── App.xaml
│
└── TopdeskPermissionsEditor.Tests/         # Unit tests
    ├── Services/
    └── ViewModels/
```

---

## 3. Data Models

### Incident Permission Model

```csharp
public class IncidentPermission
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public IncidentGrantSection Grant { get; set; }
    public IncidentRevokeSection Revoke { get; set; }
}

public class IncidentGrantSection
{
    public string Caller { get; set; }
    public string RequestShort { get; set; }
    public string RequestDescription { get; set; }
    public string Action { get; set; }
    public string Branch { get; set; }
    public string OperatorGroup { get; set; }
    public string Operator { get; set; }
    public string Category { get; set; }
    public string SubCategory { get; set; }
    public string CallType { get; set; }
    public string Status { get; set; }
    public string Impact { get; set; }
    public string Priority { get; set; }
    public string Duration { get; set; }
    public string EntryType { get; set; }
    public string Urgency { get; set; }
    public string ProcessingStatus { get; set; }
    public bool EnableGetAssets { get; set; }
    public bool SkipNoAssetsFound { get; set; }
    public string AssetsFilter { get; set; }
}

// IncidentRevokeSection has identical structure
```

### Change Permission Model

```csharp
public class ChangePermission
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public ChangeGrantSection Grant { get; set; }
    public ChangeRevokeSection Revoke { get; set; }
}

public class ChangeGrantSection
{
    public string Requester { get; set; }
    public string Request { get; set; }
    public string Action { get; set; }
    public string BriefDescription { get; set; }
    public string Template { get; set; }
    public string Category { get; set; }
    public string SubCategory { get; set; }
    public string ChangeType { get; set; }
    public string Impact { get; set; }
    public string Benefit { get; set; }
    public string Priority { get; set; }
    public bool EnableGetAssets { get; set; }
    public bool SkipNoAssetsFound { get; set; }
    public string AssetsFilter { get; set; }
}

// ChangeRevokeSection has identical structure
```

---

## 4. Key Features & User Stories

### 4.1 File Operations
- **Open File**: Browse and select incident.json or change.json files
- **Save File**: Save modifications to the current file
- **Save As**: Save to a new location
- **Recent Files**: Quick access to recently opened files

### 4.2 CRUD Operations
- **Create**: Add new permission entry to the array
- **Read**: Display all entries in a list/grid view
- **Update**: Edit selected permission entry in a form
- **Delete**: Remove permission entry with confirmation

### 4.3 Editor Features
- **Dual-pane interface**: List view + detail editor
- **Form validation**: Required fields, format validation
- **Search/Filter**: Find specific entries by ID or DisplayName
- **Copy/Duplicate**: Clone existing entries as templates
- **Undo/Redo**: Track changes (nice-to-have)

### 4.4 Validation Rules
- **ID must be unique** within the file
- **DisplayName is required**
- **Grant or Revoke section** must have at least one field populated
- **Boolean fields** properly handled
- **JSON structure** maintained on save

---

## 5. UI/UX Design

### Main Window Layout

```
┌─────────────────────────────────────────────────────────────┐
│  File  Edit  View  Help                        [- □ ×]      │
├─────────────────────────────────────────────────────────────┤
│  📁 Open  💾 Save  ➕ New Entry                              │
├─────────────────────────────────────────────────────────────┤
│                                                               │
│  ┌─────────────────┐ ┌─────────────────────────────────────┐│
│  │ Permission List │ │  Permission Editor                  ││
│  ├─────────────────┤ ├─────────────────────────────────────┤│
│  │                 │ │  ID: [____________]                 ││
│  │ ☑ I001         │ │  DisplayName: [___________________] ││
│  │   Account...    │ │                                     ││
│  │                 │ │  ┌──────────────────────────────┐  ││
│  │ ☐ I002         │ │  │ ⚡ Grant Section            │  ││
│  │   Manager...    │ │  ├──────────────────────────────┤  ││
│  │                 │ │  │ Caller: [__________________] │  ││
│  │ ☐ I003         │ │  │ RequestShort: [____________] │  ││
│  │   Application...│ │  │ ... (scrollable fields)      │  ││
│  │                 │ │  └──────────────────────────────┘  ││
│  │ [➕][✏][🗑]     │ │                                     ││
│  │                 │ │  ┌──────────────────────────────┐  ││
│  │                 │ │  │ 🔄 Revoke Section           │  ││
│  │                 │ │  ├──────────────────────────────┤  ││
│  │                 │ │  │ ... (same fields)            │  ││
│  └─────────────────┘ └─────────────────────────────────────┘│
│                                                               │
│  Status: incident.json loaded (4 entries)        [💾 Save]  │
└─────────────────────────────────────────────────────────────┘
```

### View Design Details

1. **MainWindow**:
   - Menu bar (File, Edit, View, Help)
   - Toolbar (quick actions)
   - Split view: List + Editor
   - Status bar

2. **List View**:
   - Checkboxes for multi-select
   - ID and DisplayName columns
   - Search/filter textbox
   - Add/Edit/Delete buttons

3. **Editor Panel**:
   - Collapsible sections for Grant/Revoke
   - Grouped fields with labels
   - Checkboxes for boolean fields
   - Multi-line textbox for descriptions
   - Save button (enabled when changes detected)

---

## 6. Development Phases

### Phase 1: Foundation (Week 1)
**Goal**: Set up project structure and core data models

**Tasks**:
1. Create solution with three projects (Core, UI, Tests)
2. Install NuGet packages:
   - CommunityToolkit.Mvvm
   - Microsoft.Extensions.DependencyInjection
   - System.Text.Json
3. Implement data models (IncidentPermission, ChangePermission)
4. Create enums for common values
5. Set up dependency injection container
6. Write unit tests for models

**Deliverables**:
- Compilable solution
- All data models implemented
- Basic unit test coverage

### Phase 2: Core Services (Week 1-2)
**Goal**: Implement business logic and file operations

**Tasks**:
1. Implement `JsonFileService`:
   - LoadIncidentFile(string path)
   - LoadChangeFile(string path)
   - SaveFile(string path, object data)
2. Implement `ValidationService`:
   - ValidateIncidentPermission(IncidentPermission)
   - ValidateChangePermission(ChangePermission)
   - ValidateUniqueIds(List<T>)
3. Add error handling and logging
4. Write comprehensive unit tests

**Deliverables**:
- Fully tested service layer
- File I/O functionality working
- Validation rules implemented

### Phase 3: ViewModels (Week 2)
**Goal**: Implement MVVM ViewModels

**Tasks**:
1. Create `MainViewModel`:
   - Commands: OpenFile, SaveFile, NewEntry, DeleteEntry
   - Properties: CurrentFile, Permissions, SelectedPermission
2. Create `IncidentEditorViewModel`:
   - Two-way binding properties for all fields
   - INotifyPropertyChanged implementation
3. Create `ChangeEditorViewModel`:
   - Similar to IncidentEditorViewModel
4. Implement `RelayCommand` for button actions
5. Unit test ViewModels

**Deliverables**:
- All ViewModels implemented
- Command pattern working
- Data binding ready

### Phase 4: UI Implementation (Week 3)
**Goal**: Build WPF user interface

**Tasks**:
1. Design MainWindow.xaml:
   - Menu bar
   - Toolbar
   - Split panel layout
2. Create IncidentEditorView.xaml:
   - Form with all fields
   - Collapsible sections
3. Create ChangeEditorView.xaml:
   - Similar to IncidentEditorView
4. Implement data bindings
5. Add styling (MaterialDesign or ModernWPF)
6. Create value converters if needed

**Deliverables**:
- Functional UI
- All views implemented
- Proper data binding

### Phase 5: Integration & Testing (Week 3-4)
**Goal**: Wire everything together and test

**Tasks**:
1. Connect ViewModels to Views
2. Test CRUD operations end-to-end
3. Test with actual JSON files
4. Handle edge cases:
   - Empty Grant/Revoke sections
   - Invalid JSON
   - File access errors
5. Add user feedback (dialogs, status messages)
6. Integration testing

**Deliverables**:
- Fully functional application
- All features working
- Bug-free core functionality

### Phase 6: Polish & Documentation (Week 4)
**Goal**: Finalize application for release

**Tasks**:
1. Add keyboard shortcuts
2. Implement recent files list
3. Add application icon
4. Create user manual
5. Performance optimization
6. Final testing
7. Create installer (ClickOnce or WiX)

**Deliverables**:
- Polished application
- User documentation
- Installation package

---

## 7. Technical Implementation Details

### 7.1 JSON Serialization

**Handling null vs empty objects**:
```csharp
// Use JsonSerializerOptions to handle empty sections
var options = new JsonSerializerOptions
{
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.Never,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

// Custom converter for Grant/Revoke sections
// If all properties are null/empty, serialize as empty object {}
```

### 7.2 MVVM Pattern Implementation

**ViewModel base class**:
```csharp
public abstract class ViewModelBase : ObservableObject
{
    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }
}
```

**Using CommunityToolkit.Mvvm**:
```csharp
[ObservableProperty]
private string _displayName;

[RelayCommand]
private async Task SaveAsync()
{
    // Save logic
}
```

### 7.3 File Operations

**File Service Interface**:
```csharp
public interface IFileService
{
    Task<List<IncidentPermission>> LoadIncidentFileAsync(string path);
    Task<List<ChangePermission>> LoadChangeFileAsync(string path);
    Task SaveFileAsync<T>(string path, List<T> data);
    bool ValidateJsonStructure(string path, out string errorMessage);
}
```

### 7.4 Validation Strategy

**Fluent validation or custom validator**:
```csharp
public class IncidentPermissionValidator
{
    public ValidationResult Validate(IncidentPermission permission)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(permission.Id))
            errors.Add("ID is required");

        if (string.IsNullOrWhiteSpace(permission.DisplayName))
            errors.Add("DisplayName is required");

        if (IsEmpty(permission.Grant) && IsEmpty(permission.Revoke))
            errors.Add("At least Grant or Revoke section must be populated");

        return new ValidationResult(errors);
    }

    private bool IsEmpty(object section)
    {
        // Check if all properties are null/empty
    }
}
```

### 7.5 Error Handling

**Global exception handling**:
```csharp
// In App.xaml.cs
protected override void OnStartup(StartupEventArgs e)
{
    AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
    DispatcherUnhandledException += OnDispatcherUnhandledException;

    base.OnStartup(e);
}
```

**User-friendly error messages**:
- File not found → "Unable to open file. Please check the path and try again."
- Invalid JSON → "The file is not a valid JSON format. Please check the file structure."
- Validation error → Show specific field errors in the UI

---

## 8. Development Standards

### 8.1 Naming Conventions
- **Classes**: PascalCase (e.g., `IncidentPermission`)
- **Methods**: PascalCase (e.g., `LoadFileAsync`)
- **Private fields**: _camelCase (e.g., `_fileService`)
- **Properties**: PascalCase (e.g., `DisplayName`)
- **Constants**: UPPER_CASE (e.g., `MAX_FILE_SIZE`)

### 8.2 Code Organization
- One class per file
- Group related functionality in folders
- Keep ViewModels thin, move logic to services
- Use async/await for I/O operations

### 8.3 Git Workflow
- **main**: Production-ready code
- **develop**: Integration branch
- **feature/xxx**: Feature branches
- **bugfix/xxx**: Bug fix branches

**Commit message format**:
```
[TYPE] Short description

Detailed description if needed

TYPE: feat, fix, refactor, test, docs
```

### 8.4 Testing Requirements
- Minimum 70% code coverage
- Unit tests for all services
- Unit tests for ViewModels
- Integration tests for file operations
- Test with real JSON files

---

## 9. Risk Assessment & Mitigation

### Risk 1: JSON Structure Changes
**Impact**: High
**Likelihood**: Medium
**Mitigation**:
- Version the data models
- Implement schema validation
- Provide migration tools

### Risk 2: Large File Performance
**Impact**: Medium
**Likelihood**: Low
**Mitigation**:
- Implement pagination if needed
- Use virtualization in list views
- Load files asynchronously

### Risk 3: Concurrent File Access
**Impact**: Low
**Likelihood**: Low
**Mitigation**:
- File locking mechanism
- Warning if file is modified externally
- Auto-reload detection

### Risk 4: Data Loss
**Impact**: High
**Likelihood**: Low
**Mitigation**:
- Auto-save functionality
- Backup original file before saving
- Unsaved changes warning

---

## 10. Acceptance Criteria

### Must Have (MVP)
- ✅ Open incident.json and change.json files
- ✅ Display all permission entries in a list
- ✅ Add new permission entry
- ✅ Edit existing permission entry
- ✅ Delete permission entry
- ✅ Save changes back to JSON file
- ✅ Validate required fields (ID, DisplayName)
- ✅ Maintain JSON structure (empty objects for Grant/Revoke)

### Should Have
- ✅ Search/filter entries
- ✅ Duplicate entry functionality
- ✅ Recent files list
- ✅ Keyboard shortcuts
- ✅ Confirmation dialogs for destructive actions

### Nice to Have
- ⭕ Undo/Redo functionality
- ⭕ Diff view (compare with saved version)
- ⭕ Export to other formats
- ⭕ Dark mode theme
- ⭕ Drag-and-drop reordering

---

## 11. Deployment

### Build Configuration
- **Debug**: Development with full logging
- **Release**: Optimized, minimal logging

### Installation Options
1. **ClickOnce Deployment**: Easy updates, auto-install
2. **MSI Installer**: Traditional Windows installer
3. **Portable**: Single executable with dependencies

### System Requirements
- Windows 10/11 (64-bit)
- .NET 8 Runtime
- 50 MB disk space
- 4 GB RAM (recommended)

---

## 12. Timeline Summary

| Phase | Duration | Key Deliverables |
|-------|----------|------------------|
| Phase 1: Foundation | 1 week | Project structure, data models |
| Phase 2: Core Services | 1 week | File I/O, validation |
| Phase 3: ViewModels | 1 week | MVVM implementation |
| Phase 4: UI Implementation | 1 week | WPF views and styling |
| Phase 5: Integration & Testing | 1 week | End-to-end testing |
| Phase 6: Polish & Documentation | 1 week | Final release |
| **Total** | **6 weeks** | Production-ready application |

---

## 13. Next Steps for Development Team

1. **Review this plan** and provide feedback
2. **Set up development environment**:
   - Install Visual Studio 2022
   - Install .NET 8 SDK
   - Set up Git repository
3. **Create initial project structure**
4. **Assign tasks** from Phase 1
5. **Schedule daily standups** for coordination
6. **Set up CI/CD pipeline** (if applicable)

---

## Appendix A: Useful Resources

- [WPF Documentation](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
- [MVVM Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/maui/mvvm)
- [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)
- [System.Text.Json](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-overview)
- [MaterialDesignInXAML](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)

---

## Appendix B: Sample File Paths

**Example incident.json location**:
```
C:\Projects\HelloID-Config\permissions\incident\example.incident.json
```

**Example change.json location**:
```
C:\Projects\HelloID-Config\permissions\change\example.change.json
```
