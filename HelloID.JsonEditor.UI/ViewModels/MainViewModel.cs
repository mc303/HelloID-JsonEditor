using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.Input;
using HelloID.JsonEditor.Models;
using HelloID.JsonEditor.Services;

namespace HelloID.JsonEditor.UI.ViewModels;

/// <summary>
/// Main ViewModel for the application
/// Handles file operations, permission list management, and coordination between components
/// </summary>
public partial class MainViewModel : ViewModelBase
{
    private readonly IFileService _fileService;
    private readonly IValidationService _validationService;

    private string? _currentFilePath;
    private PermissionType? _currentPermissionType;
    private object? _selectedPermission;
    private bool _hasUnsavedChanges;
    private int _selectedTabIndex;

    /// <summary>
    /// Editor ViewModel for incident permissions
    /// </summary>
    public IncidentEditorViewModel IncidentEditor { get; }

    /// <summary>
    /// Editor ViewModel for change permissions
    /// </summary>
    public ChangeEditorViewModel ChangeEditor { get; }

    /// <summary>
    /// Collection of incident permissions (when working with incident file)
    /// </summary>
    public ObservableCollection<IncidentPermission> IncidentPermissions { get; } = new();

    /// <summary>
    /// Collection of change permissions (when working with change file)
    /// </summary>
    public ObservableCollection<ChangePermission> ChangePermissions { get; } = new();

    /// <summary>
    /// Path to the currently opened file
    /// </summary>
    public string? CurrentFilePath
    {
        get => _currentFilePath;
        private set
        {
            if (SetProperty(ref _currentFilePath, value))
            {
                OnPropertyChanged(nameof(CurrentFileName));
                OnPropertyChanged(nameof(WindowTitle));
            }
        }
    }

    /// <summary>
    /// Name of the currently opened file
    /// </summary>
    public string CurrentFileName =>
        string.IsNullOrEmpty(CurrentFilePath) ? "Untitled" : Path.GetFileName(CurrentFilePath);

    /// <summary>
    /// Window title including file name and unsaved indicator
    /// </summary>
    public string WindowTitle
    {
        get
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly()
                .GetName().Version?.ToString(3) ?? "0.1.23";
            return $"HelloID - JSON Editor for TOPdesk Permissions v{version} - {CurrentFileName}{(HasUnsavedChanges ? "*" : "")}";
        }
    }

    /// <summary>
    /// Type of permission file currently loaded
    /// </summary>
    public PermissionType? CurrentPermissionType
    {
        get => _currentPermissionType;
        private set
        {
            if (SetProperty(ref _currentPermissionType, value))
            {
                OnPropertyChanged(nameof(IsIncidentFile));
                OnPropertyChanged(nameof(IsChangeFile));
                OnPropertyChanged(nameof(HasOpenFile));
                OnPropertyChanged(nameof(IsIncidentTabEnabled));
                OnPropertyChanged(nameof(IsChangeTabEnabled));
            }
        }
    }

    /// <summary>
    /// True if current file is an incident file
    /// </summary>
    public bool IsIncidentFile => CurrentPermissionType == PermissionType.Incident;

    /// <summary>
    /// True if current file is a change file
    /// </summary>
    public bool IsChangeFile => CurrentPermissionType == PermissionType.Change;

    /// <summary>
    /// True if a file is currently open (either incident or change)
    /// </summary>
    public bool HasOpenFile => CurrentPermissionType.HasValue;

    /// <summary>
    /// True if Incident tab should be enabled (no file open OR incident file is open)
    /// </summary>
    public bool IsIncidentTabEnabled => !HasOpenFile || IsIncidentFile;

    /// <summary>
    /// True if Change tab should be enabled (no file open OR change file is open)
    /// </summary>
    public bool IsChangeTabEnabled => !HasOpenFile || IsChangeFile;

    /// <summary>
    /// Selected tab index (0 = Incident, 1 = Change)
    /// </summary>
    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => SetProperty(ref _selectedTabIndex, value);
    }

    /// <summary>
    /// Currently selected permission (either IncidentPermission or ChangePermission)
    /// </summary>
    public object? SelectedPermission
    {
        get => _selectedPermission;
        set
        {
            if (SetProperty(ref _selectedPermission, value))
            {
                // Wire up the appropriate editor ViewModel
                if (value is IncidentPermission incidentPermission)
                {
                    IncidentEditor.Permission = incidentPermission;
                }
                else if (value is ChangePermission changePermission)
                {
                    ChangeEditor.Permission = changePermission;
                }
                else
                {
                    // Clear both editors if no selection
                    IncidentEditor.Permission = null;
                    ChangeEditor.Permission = null;
                }

                DeleteCommand.NotifyCanExecuteChanged();
                DuplicateCommand.NotifyCanExecuteChanged();
            }
        }
    }

    /// <summary>
    /// Indicates if there are unsaved changes
    /// </summary>
    public bool HasUnsavedChanges
    {
        get => _hasUnsavedChanges;
        set
        {
            if (SetProperty(ref _hasUnsavedChanges, value))
            {
                OnPropertyChanged(nameof(WindowTitle));
                SaveCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public MainViewModel(IFileService fileService, IValidationService validationService)
    {
        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));

        // Initialize editor ViewModels with modification callbacks
        IncidentEditor = new IncidentEditorViewModel(MarkAsModified);
        ChangeEditor = new ChangeEditorViewModel(MarkAsModified);
    }

    /// <summary>
    /// Creates a new empty incident file
    /// </summary>
    [RelayCommand]
    private void NewIncidentFile()
    {
        if (HasUnsavedChanges)
        {
            // TODO: Prompt user to save changes
            // For now, just continue
        }

        ClearAll();
        CurrentPermissionType = PermissionType.Incident;
        CurrentFilePath = null;
        SelectedTabIndex = 0;
        HasUnsavedChanges = false;

        // Automatically add one empty incident permission
        AddNewIncident();

        StatusMessage = "New incident file created";
    }

    /// <summary>
    /// Creates a new empty change file
    /// </summary>
    [RelayCommand]
    private void NewChangeFile()
    {
        if (HasUnsavedChanges)
        {
            // TODO: Prompt user to save changes
            // For now, just continue
        }

        ClearAll();
        CurrentPermissionType = PermissionType.Change;
        CurrentFilePath = null;
        SelectedTabIndex = 1;
        HasUnsavedChanges = false;

        // Automatically add one empty change permission
        AddNewChange();

        StatusMessage = "New change file created";
    }

    /// <summary>
    /// Closes the current file and clears all data
    /// </summary>
    [RelayCommand]
    private void CloseFile()
    {
        if (HasUnsavedChanges)
        {
            // TODO: Prompt user to save changes
            // For now, just continue
        }

        ClearAll();
        CurrentPermissionType = null;
        CurrentFilePath = null;
        HasUnsavedChanges = false;

        StatusMessage = "File closed";
    }

    /// <summary>
    /// Clears all permissions and resets editors
    /// </summary>
    private void ClearAll()
    {
        IncidentPermissions.Clear();
        ChangePermissions.Clear();
        SelectedPermission = null;
        IncidentEditor.Permission = null;
        ChangeEditor.Permission = null;
    }

    /// <summary>
    /// Opens a file from the specified path
    /// </summary>
    [RelayCommand]
    private async Task OpenFileAsync(string? filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            StatusMessage = "No file selected";
            return;
        }

        if (HasUnsavedChanges)
        {
            // TODO: Prompt user to save changes
            // For now, just continue
        }

        try
        {
            SetBusy(true, "Opening file...");

            // Detect file type
            var detectedType = await _fileService.DetectPermissionTypeAsync(filePath);

            if (!detectedType.HasValue)
            {
                SetBusy(false, "Unable to determine file type");
                return;
            }

            // Load based on type
            if (detectedType.Value == PermissionType.Incident)
            {
                var permissions = await _fileService.LoadIncidentFileAsync(filePath);
                var validationResult = _validationService.ValidateIncidentPermissions(permissions);

                if (!validationResult.IsValid)
                {
                    StatusMessage = $"File loaded with {validationResult.Errors.Count} validation error(s)";
                    // Continue loading but show warnings
                }

                IncidentPermissions.Clear();
                foreach (var permission in permissions)
                {
                    IncidentPermissions.Add(permission);
                }
                ChangePermissions.Clear();
            }
            else // Change
            {
                var permissions = await _fileService.LoadChangeFileAsync(filePath);
                var validationResult = _validationService.ValidateChangePermissions(permissions);

                if (!validationResult.IsValid)
                {
                    StatusMessage = $"File loaded with {validationResult.Errors.Count} validation error(s)";
                }

                ChangePermissions.Clear();
                foreach (var permission in permissions)
                {
                    ChangePermissions.Add(permission);
                }
                IncidentPermissions.Clear();
            }

            CurrentFilePath = filePath;
            CurrentPermissionType = detectedType.Value;
            HasUnsavedChanges = false;

            // Auto-switch to the correct tab
            SelectedTabIndex = IsIncidentFile ? 0 : 1;

            // Auto-select first item in the list
            if (IsIncidentFile && IncidentPermissions.Count > 0)
            {
                SelectedPermission = IncidentPermissions[0];
            }
            else if (IsChangeFile && ChangePermissions.Count > 0)
            {
                SelectedPermission = ChangePermissions[0];
            }

            var count = IsIncidentFile ? IncidentPermissions.Count : ChangePermissions.Count;
            StatusMessage = $"Loaded {count} permission(s) from {CurrentFileName}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error opening file: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Saves the current file
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanSave))]
    private async Task SaveAsync()
    {
        // Note: If CurrentFilePath is null, the UI layer will show Save As dialog
        if (string.IsNullOrEmpty(CurrentFilePath))
        {
            StatusMessage = "No file path specified";
            return;
        }

        try
        {
            SetBusy(true, "Saving...");

            if (IsIncidentFile)
            {
                var permissions = IncidentPermissions.ToList();
                var validationResult = _validationService.ValidateIncidentPermissions(permissions);

                if (!validationResult.IsValid)
                {
                    SetBusy(false, $"Validation failed: {string.Join(", ", validationResult.Errors)}");
                    return;
                }

                await _fileService.SaveIncidentFileAsync(CurrentFilePath, permissions);
            }
            else if (IsChangeFile)
            {
                var permissions = ChangePermissions.ToList();
                var validationResult = _validationService.ValidateChangePermissions(permissions);

                if (!validationResult.IsValid)
                {
                    SetBusy(false, $"Validation failed: {string.Join(", ", validationResult.Errors)}");
                    return;
                }

                await _fileService.SaveChangeFileAsync(CurrentFilePath, permissions);
            }

            HasUnsavedChanges = false;
            StatusMessage = $"Saved to {CurrentFileName}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving file: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanSave() => HasUnsavedChanges && CurrentPermissionType.HasValue;

    /// <summary>
    /// Saves the current file to a new location
    /// </summary>
    [RelayCommand]
    private async Task SaveAsAsync()
    {
        // This is called from UI code-behind which shows the dialog
        StatusMessage = "Save As not yet implemented";
        await Task.CompletedTask;
    }

    /// <summary>
    /// Saves the current permissions to a specified file path
    /// </summary>
    public async Task SaveAsAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            StatusMessage = "No file path specified";
            return;
        }

        try
        {
            SetBusy(true, "Saving file...");

            if (IsIncidentFile)
            {
                var permissions = IncidentPermissions.ToList();

                // Set Grant/Revoke to empty objects based on toggle states
                foreach (var permission in permissions)
                {
                    if (!IncidentEditor.IsGrantEnabled)
                    {
                        permission.Grant = new IncidentSection
                        {
                            Caller = null,
                            RequestShort = null,
                            RequestDescription = null,
                            Action = null,
                            Branch = null,
                            OperatorGroup = null,
                            Operator = null,
                            Category = null,
                            SubCategory = null,
                            CallType = null,
                            Status = null,
                            Impact = null,
                            Priority = null,
                            Duration = null,
                            EntryType = null,
                            Urgency = null,
                            ProcessingStatus = null,
                            EnableGetAssets = false,
                            SkipNoAssetsFound = false,
                            AssetsFilter = ""
                        };
                    }
                    if (!IncidentEditor.IsRevokeEnabled)
                    {
                        permission.Revoke = new IncidentSection
                        {
                            Caller = null,
                            RequestShort = null,
                            RequestDescription = null,
                            Action = null,
                            Branch = null,
                            OperatorGroup = null,
                            Operator = null,
                            Category = null,
                            SubCategory = null,
                            CallType = null,
                            Status = null,
                            Impact = null,
                            Priority = null,
                            Duration = null,
                            EntryType = null,
                            Urgency = null,
                            ProcessingStatus = null,
                            EnableGetAssets = false,
                            SkipNoAssetsFound = false,
                            AssetsFilter = ""
                        };
                    }
                }

                var validationResult = _validationService.ValidateIncidentPermissions(permissions);

                if (!validationResult.IsValid)
                {
                    StatusMessage = $"Cannot save: {validationResult.Errors.Count} validation error(s)";
                    return;
                }

                await _fileService.SaveIncidentFileAsync(filePath, permissions);
            }
            else if (IsChangeFile)
            {
                var permissions = ChangePermissions.ToList();

                // Set Grant/Revoke to empty objects based on toggle states
                foreach (var permission in permissions)
                {
                    if (!ChangeEditor.IsGrantEnabled)
                    {
                        permission.Grant = new ChangeSection
                        {
                            Requester = null,
                            Request = null,
                            Action = null,
                            BriefDescription = null,
                            Template = null,
                            Category = null,
                            SubCategory = null,
                            ChangeType = null,
                            Impact = null,
                            Benefit = null,
                            Priority = null,
                            EnableGetAssets = false,
                            SkipNoAssetsFound = false,
                            AssetsFilter = ""
                        };
                    }
                    if (!ChangeEditor.IsRevokeEnabled)
                    {
                        permission.Revoke = new ChangeSection
                        {
                            Requester = null,
                            Request = null,
                            Action = null,
                            BriefDescription = null,
                            Template = null,
                            Category = null,
                            SubCategory = null,
                            ChangeType = null,
                            Impact = null,
                            Benefit = null,
                            Priority = null,
                            EnableGetAssets = false,
                            SkipNoAssetsFound = false,
                            AssetsFilter = ""
                        };
                    }
                }

                var validationResult = _validationService.ValidateChangePermissions(permissions);

                if (!validationResult.IsValid)
                {
                    StatusMessage = $"Cannot save: {validationResult.Errors.Count} validation error(s)";
                    return;
                }

                await _fileService.SaveChangeFileAsync(filePath, permissions);
            }
            else
            {
                StatusMessage = "No file type selected";
                return;
            }

            CurrentFilePath = filePath;
            HasUnsavedChanges = false;
            StatusMessage = $"Saved to {CurrentFileName}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error saving file: {ex.Message}";
        }
        finally
        {
            SetBusy(false);
        }
    }

    /// <summary>
    /// Creates a new incident permission entry
    /// </summary>
    [RelayCommand]
    private void AddNewIncident()
    {
        var newPermission = new IncidentPermission
        {
            Id = GenerateNewId("I", IncidentPermissions.Select(p => p.Id)),
            DisplayName = "New Incident Permission",
            Grant = new IncidentSection(),
            Revoke = new IncidentSection()
        };

        IncidentPermissions.Add(newPermission);
        SelectedPermission = newPermission;
        HasUnsavedChanges = true;

        StatusMessage = $"Added new incident permission: {newPermission.Id}";
    }

    /// <summary>
    /// Creates a new change permission entry
    /// </summary>
    [RelayCommand]
    private void AddNewChange()
    {
        var newPermission = new ChangePermission
        {
            Id = GenerateNewId("C", ChangePermissions.Select(p => p.Id)),
            DisplayName = "New Change Permission",
            Grant = new ChangeSection(),
            Revoke = new ChangeSection()
        };

        ChangePermissions.Add(newPermission);
        SelectedPermission = newPermission;
        HasUnsavedChanges = true;

        StatusMessage = $"Added new change permission: {newPermission.Id}";
    }

    /// <summary>
    /// Deletes the currently selected permission
    /// </summary>
    [RelayCommand(CanExecute = nameof(HasSelection))]
    private void Delete()
    {
        if (SelectedPermission is IncidentPermission incident)
        {
            IncidentPermissions.Remove(incident);
            StatusMessage = $"Deleted incident permission: {incident.Id}";
        }
        else if (SelectedPermission is ChangePermission change)
        {
            ChangePermissions.Remove(change);
            StatusMessage = $"Deleted change permission: {change.Id}";
        }

        SelectedPermission = null;
        HasUnsavedChanges = true;
    }

    /// <summary>
    /// Duplicates the currently selected permission
    /// </summary>
    [RelayCommand(CanExecute = nameof(HasSelection))]
    private void Duplicate()
    {
        if (SelectedPermission is IncidentPermission incident)
        {
            var duplicate = new IncidentPermission
            {
                Id = GenerateNewId("I", IncidentPermissions.Select(p => p.Id)),
                DisplayName = $"{incident.DisplayName} (Copy)",
                Grant = CloneIncidentSection(incident.Grant),
                Revoke = CloneIncidentSection(incident.Revoke)
            };

            IncidentPermissions.Add(duplicate);
            SelectedPermission = duplicate;
            StatusMessage = $"Duplicated as {duplicate.Id}";
        }
        else if (SelectedPermission is ChangePermission change)
        {
            var duplicate = new ChangePermission
            {
                Id = GenerateNewId("C", ChangePermissions.Select(p => p.Id)),
                DisplayName = $"{change.DisplayName} (Copy)",
                Grant = CloneChangeSection(change.Grant),
                Revoke = CloneChangeSection(change.Revoke)
            };

            ChangePermissions.Add(duplicate);
            SelectedPermission = duplicate;
            StatusMessage = $"Duplicated as {duplicate.Id}";
        }

        HasUnsavedChanges = true;
    }

    private bool HasSelection() => SelectedPermission != null;

    /// <summary>
    /// Generates a new unique ID for a permission
    /// </summary>
    private string GenerateNewId(string prefix, IEnumerable<string> existingIds)
    {
        var usedNumbers = existingIds
            .Where(id => id.StartsWith(prefix))
            .Select(id => int.TryParse(id.Substring(prefix.Length), out var num) ? num : 0)
            .ToHashSet();

        int newNumber = 1;
        while (usedNumbers.Contains(newNumber))
        {
            newNumber++;
        }

        return $"{prefix}{newNumber:D3}";
    }

    /// <summary>
    /// Creates a deep copy of an incident section
    /// </summary>
    private IncidentSection? CloneIncidentSection(IncidentSection? source)
    {
        if (source == null) return null;

        return new IncidentSection
        {
            Caller = source.Caller,
            RequestShort = source.RequestShort,
            RequestDescription = source.RequestDescription,
            Action = source.Action,
            Branch = source.Branch,
            OperatorGroup = source.OperatorGroup,
            Operator = source.Operator,
            Category = source.Category,
            SubCategory = source.SubCategory,
            CallType = source.CallType,
            Status = source.Status,
            Impact = source.Impact,
            Priority = source.Priority,
            Duration = source.Duration,
            EntryType = source.EntryType,
            Urgency = source.Urgency,
            ProcessingStatus = source.ProcessingStatus,
            EnableGetAssets = source.EnableGetAssets,
            SkipNoAssetsFound = source.SkipNoAssetsFound,
            AssetsFilter = source.AssetsFilter
        };
    }

    /// <summary>
    /// Creates a deep copy of a change section
    /// </summary>
    private ChangeSection? CloneChangeSection(ChangeSection? source)
    {
        if (source == null) return null;

        return new ChangeSection
        {
            Requester = source.Requester,
            Request = source.Request,
            Action = source.Action,
            BriefDescription = source.BriefDescription,
            Template = source.Template,
            Category = source.Category,
            SubCategory = source.SubCategory,
            ChangeType = source.ChangeType,
            Impact = source.Impact,
            Benefit = source.Benefit,
            Priority = source.Priority,
            EnableGetAssets = source.EnableGetAssets,
            SkipNoAssetsFound = source.SkipNoAssetsFound,
            AssetsFilter = source.AssetsFilter
        };
    }

    /// <summary>
    /// Marks the data as modified
    /// </summary>
    public void MarkAsModified()
    {
        HasUnsavedChanges = true;
    }
}
