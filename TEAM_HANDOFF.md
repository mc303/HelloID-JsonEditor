# Team Handoff - TOPdesk Permissions Editor

## What Has Been Prepared

A complete planning package for building a C# WPF application to edit TOPdesk permission JSON files.

### Documentation Created

1. **DEVELOPMENT_PLAN.md** - Comprehensive 13-section development specification
   - Complete architecture design
   - Data models
   - UI mockups
   - 6-week phase-by-phase plan
   - Technical implementation details
   - Risk assessment
   - Acceptance criteria

2. **README.md** - Quick-start guide
   - Project overview
   - Technology stack
   - Setup instructions
   - Timeline summary

3. **Starter Code Templates** - Ready-to-use C# classes
   - Models folder with 4 class files
   - Services folder with 3 interface/implementation files

### File Structure Created

```
TopdeskPermissionsEditor/
├── README.md
├── DEVELOPMENT_PLAN.md
├── TEAM_HANDOFF.md (this file)
├── Models/
│   ├── IncidentPermission.cs
│   ├── ChangePermission.cs
│   ├── PermissionType.cs
│   └── ValidationResult.cs
└── Services/
    ├── IFileService.cs
    ├── JsonFileService.cs
    └── IValidationService.cs
```

---

## What the Team Needs to Do Next

### Step 1: Review Documentation (Day 1)
- [ ] Read DEVELOPMENT_PLAN.md thoroughly
- [ ] Understand the data models (check example JSON files online)
- [ ] Review architecture decisions
- [ ] Discuss and approve the plan
- [ ] Raise any questions or concerns

### Step 2: Environment Setup (Day 1-2)
- [ ] Install Visual Studio 2022 (Community, Professional, or Enterprise)
- [ ] Install .NET 8 SDK (https://dotnet.microsoft.com/download/dotnet/8.0)
- [ ] Set up Git repository
- [ ] Configure branches:
  - `main` - production
  - `develop` - integration
  - `feature/*` - feature branches

### Step 3: Create Solution Structure (Day 2-3)

#### Create the Solution and Projects

```powershell
# Navigate to project folder
cd C:\HelloID-Code\vscode-projects\TopdeskPermissionsEditor

# Create solution
dotnet new sln -n TopdeskPermissionsEditor

# Create Core project (class library)
dotnet new classlib -n TopdeskPermissionsEditor.Core -f net8.0
dotnet sln add TopdeskPermissionsEditor.Core

# Create UI project (WPF)
dotnet new wpf -n TopdeskPermissionsEditor.UI -f net8.0
dotnet sln add TopdeskPermissionsEditor.UI

# Create Tests project
dotnet new xunit -n TopdeskPermissionsEditor.Tests -f net8.0
dotnet sln add TopdeskPermissionsEditor.Tests

# Add project references
cd TopdeskPermissionsEditor.UI
dotnet add reference ../TopdeskPermissionsEditor.Core

cd ../TopdeskPermissionsEditor.Tests
dotnet add reference ../TopdeskPermissionsEditor.Core
```

#### Move Template Files to Correct Projects

1. **Move Models to Core project:**
   ```powershell
   Move-Item .\Models\* .\TopdeskPermissionsEditor.Core\Models\
   ```

2. **Move Services to Core project:**
   ```powershell
   Move-Item .\Services\* .\TopdeskPermissionsEditor.Core\Services\
   ```

3. **Add NuGet packages:**
   ```powershell
   # Core project
   cd TopdeskPermissionsEditor.Core
   dotnet add package System.Text.Json

   # UI project
   cd ../TopdeskPermissionsEditor.UI
   dotnet add package CommunityToolkit.Mvvm
   dotnet add package Microsoft.Extensions.DependencyInjection
   dotnet add package MaterialDesignThemes

   # Tests project
   cd ../TopdeskPermissionsEditor.Tests
   dotnet add package FluentAssertions
   dotnet add package Moq
   ```

### Step 4: Assign Phase 1 Tasks (Day 3)

Distribute these tasks among the team:

**Developer 1: Core Models**
- [ ] Finalize data models in Core project
- [ ] Add XML documentation comments
- [ ] Create unit tests for models
- [ ] Validate JSON serialization/deserialization

**Developer 2: File Services**
- [ ] Implement JsonFileService fully
- [ ] Implement ValidationService
- [ ] Add error handling
- [ ] Write unit tests for services

**Developer 3: Project Setup**
- [ ] Configure dependency injection
- [ ] Set up logging (Serilog or NLog)
- [ ] Create app configuration structure
- [ ] Set up CI/CD pipeline (if applicable)

### Step 5: Follow the Development Plan Phases

Refer to **DEVELOPMENT_PLAN.md Section 6** for detailed phase breakdown:

- **Week 1**: Phase 1 & 2 (Foundation + Core Services)
- **Week 2**: Phase 3 (ViewModels)
- **Week 3**: Phase 4 (UI Implementation)
- **Week 3-4**: Phase 5 (Integration & Testing)
- **Week 4**: Phase 6 (Polish & Documentation)

---

## Key Decisions Already Made

### Technology Choices
- **.NET 8** (LTS version)
- **WPF** (best for Windows desktop forms)
- **MVVM Pattern** (clean separation of concerns)
- **System.Text.Json** (built-in, performant)
- **MaterialDesignThemes** (modern UI, recommended)

### Architecture Pattern
- **Three-tier architecture**:
  1. Core (Models + Services)
  2. UI (Views + ViewModels)
  3. Tests

### Data Model Design
- Separate classes for Incident and Change permissions
- Nullable sections (Grant/Revoke can be empty)
- IsEmpty() helper methods
- JSON property name attributes

---

## Critical Success Factors

### Must-Have Features (MVP)
1. Open incident.json and change.json files
2. List all permission entries
3. Add/Edit/Delete entries
4. Save changes back to JSON
5. Validate required fields (ID, DisplayName)
6. Maintain empty object structure in JSON

### Quality Standards
- Minimum 70% test coverage
- All services unit tested
- Proper error handling
- User-friendly error messages
- No data loss (backup files before save)

### Timeline Expectations
- **6 weeks total** for full application
- **2 weeks** for working MVP (CRUD operations)
- **4 weeks** for polish and advanced features

---

## Questions to Address Before Starting

1. **UI Library**: MaterialDesignThemes or ModernWPF?
   - Recommendation: MaterialDesignThemes (more modern, better documentation)

2. **Testing Strategy**: How much test coverage?
   - Recommendation: 70% minimum, focus on services and ViewModels

3. **Deployment**: How will users install?
   - Options: ClickOnce, MSI installer, or portable EXE
   - Recommendation: ClickOnce for easy updates

4. **Source Control**: Git repository location?
   - GitHub, Azure DevOps, or local Git server?

5. **Team Structure**: How many developers?
   - Recommended: 2-3 developers for 6-week timeline

---

## Resources & References

### Documentation to Read
- [WPF Overview](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
- [MVVM Toolkit](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)
- [System.Text.Json](https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-overview)

### Example JSON Files
- [Incident JSON](https://raw.githubusercontent.com/Tools4everBV/HelloID-Conn-Prov-Target-Topdesk/refs/heads/main/permissions/incident/example.incident.json)
- [Change JSON](https://raw.githubusercontent.com/Tools4everBV/HelloID-Conn-Prov-Target-Topdesk/refs/heads/main/permissions/change/example.change.json)

### Recommended Tools
- Visual Studio 2022
- Git for Windows
- Postman or similar (for testing JSON)
- Notepad++ or VS Code (for viewing JSON files)

---

## Communication Plan

### Daily Standup (15 minutes)
- What did you complete yesterday?
- What are you working on today?
- Any blockers?

### Weekly Review (1 hour)
- Demo completed work
- Review code (pull requests)
- Plan next week's tasks
- Address any issues

### Phase Reviews
- End of each phase: demo to stakeholders
- Gather feedback
- Adjust plan if needed

---

## Success Criteria

The project is complete when:

1. ✅ Users can open incident.json and change.json files
2. ✅ Users can view all entries in a list
3. ✅ Users can add new entries
4. ✅ Users can edit existing entries
5. ✅ Users can delete entries
6. ✅ Users can save changes to file
7. ✅ Application validates data before saving
8. ✅ Application creates backups before overwriting
9. ✅ Application has proper error handling
10. ✅ All unit tests pass
11. ✅ Code coverage > 70%
12. ✅ User documentation exists
13. ✅ Installer package created

---

## Getting Help

If you encounter issues or need clarification:

1. **Technical Questions**: Refer to DEVELOPMENT_PLAN.md
2. **Architecture Questions**: Review the "Technical Architecture" section
3. **Implementation Details**: Check the "Technical Implementation Details" section
4. **Stuck on a Task**: Discuss in daily standup

---

## Final Checklist Before You Start

- [ ] All team members have read DEVELOPMENT_PLAN.md
- [ ] Development environment is set up (VS 2022, .NET 8)
- [ ] Git repository is created and configured
- [ ] Team roles and responsibilities are assigned
- [ ] Communication channels are established (Slack, Teams, etc.)
- [ ] First sprint tasks are identified and assigned
- [ ] Kickoff meeting is scheduled

---

## Contact & Support

For questions about this handoff package or the development plan, contact the technical lead or project manager.

**Good luck with the project!**
