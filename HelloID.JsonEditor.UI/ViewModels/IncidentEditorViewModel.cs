using System.Windows;
using CommunityToolkit.Mvvm.Input;
using HelloID.JsonEditor.Models;

namespace HelloID.JsonEditor.UI.ViewModels;

/// <summary>
/// ViewModel for editing an individual incident permission
/// Provides two-way binding for all properties
/// </summary>
public partial class IncidentEditorViewModel : ViewModelBase
{
    private IncidentPermission? _permission;
    private readonly Action? _onModified;
    private bool _isGrantEnabled;
    private bool _isRevokeEnabled;

    /// <summary>
    /// The incident permission being edited
    /// </summary>
    public IncidentPermission? Permission
    {
        get => _permission;
        set
        {
            if (SetProperty(ref _permission, value))
            {
                LoadPermission();
            }
        }
    }

    // Basic properties
    private string _id = string.Empty;
    private string _displayName = string.Empty;

    // Grant section properties
    private string? _grantCaller;
    private string? _grantRequestShort;
    private string? _grantRequestDescription;
    private string? _grantAction;
    private string? _grantBranch;
    private string? _grantOperatorGroup;
    private string? _grantOperator;
    private string? _grantCategory;
    private string? _grantSubCategory;
    private string? _grantCallType;
    private string? _grantStatus;
    private string? _grantImpact;
    private string? _grantPriority;
    private string? _grantDuration;
    private string? _grantEntryType;
    private string? _grantUrgency;
    private string? _grantProcessingStatus;
    private bool _grantEnableGetAssets;
    private bool _grantSkipNoAssetsFound;
    private string? _grantAssetsFilter;

    // Revoke section properties
    private string? _revokeCaller;
    private string? _revokeRequestShort;
    private string? _revokeRequestDescription;
    private string? _revokeAction;
    private string? _revokeBranch;
    private string? _revokeOperatorGroup;
    private string? _revokeOperator;
    private string? _revokeCategory;
    private string? _revokeSubCategory;
    private string? _revokeCallType;
    private string? _revokeStatus;
    private string? _revokeImpact;
    private string? _revokePriority;
    private string? _revokeDuration;
    private string? _revokeEntryType;
    private string? _revokeUrgency;
    private string? _revokeProcessingStatus;
    private bool _revokeEnableGetAssets;
    private bool _revokeSkipNoAssetsFound;
    private string? _revokeAssetsFilter;

    public IncidentEditorViewModel(Action? onModified = null)
    {
        _onModified = onModified;
    }

    #region Basic Properties

    public string Id
    {
        get => _id;
        set
        {
            if (SetProperty(ref _id, value))
            {
                if (_permission != null)
                {
                    _permission.Id = value;
                    NotifyModified();
                }
            }
        }
    }

    public string DisplayName
    {
        get => _displayName;
        set
        {
            if (SetProperty(ref _displayName, value))
            {
                if (_permission != null)
                {
                    _permission.DisplayName = value;
                    NotifyModified();
                }
            }
        }
    }

    #endregion

    #region Toggle Properties

    /// <summary>
    /// Controls whether Grant section is enabled
    /// </summary>
    public bool IsGrantEnabled
    {
        get => _isGrantEnabled;
        set
        {
            if (SetProperty(ref _isGrantEnabled, value))
            {
                if (value)
                {
                    // Enable: Create Grant object if it doesn't exist
                    EnsureGrantExists();
                }
                else
                {
                    // Disable: Check for data loss
                    if (_permission?.Grant != null && !_permission.Grant.IsEmpty())
                    {
                        var result = MessageBox.Show(
                            "The Grant section contains data. Disabling it will discard this data when saving. Continue?",
                            "Data Loss Warning",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (result == MessageBoxResult.No)
                        {
                            // Revert the toggle
                            _isGrantEnabled = true;
                            OnPropertyChanged(nameof(IsGrantEnabled));
                            return;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Controls whether Revoke section is enabled
    /// </summary>
    public bool IsRevokeEnabled
    {
        get => _isRevokeEnabled;
        set
        {
            if (SetProperty(ref _isRevokeEnabled, value))
            {
                if (value)
                {
                    // Enable: Create Revoke object if it doesn't exist
                    EnsureRevokeExists();
                }
                else
                {
                    // Disable: Check for data loss
                    if (_permission?.Revoke != null && !_permission.Revoke.IsEmpty())
                    {
                        var result = MessageBox.Show(
                            "The Revoke section contains data. Disabling it will discard this data when saving. Continue?",
                            "Data Loss Warning",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning);

                        if (result == MessageBoxResult.No)
                        {
                            // Revert the toggle
                            _isRevokeEnabled = true;
                            OnPropertyChanged(nameof(IsRevokeEnabled));
                            return;
                        }
                    }
                }
            }
        }
    }

    private void EnsureGrantExists()
    {
        if (_permission != null && _permission.Grant == null)
        {
            _permission.Grant = new IncidentSection();
            NotifyModified();
        }
    }

    private void EnsureRevokeExists()
    {
        if (_permission != null && _permission.Revoke == null)
        {
            _permission.Revoke = new IncidentSection();
            NotifyModified();
        }
    }

    #endregion

    #region Grant Section Properties

    public string? GrantCaller
    {
        get => _grantCaller;
        set
        {
            if (SetProperty(ref _grantCaller, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.Caller = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantRequestShort
    {
        get => _grantRequestShort;
        set
        {
            if (SetProperty(ref _grantRequestShort, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.RequestShort = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantRequestDescription
    {
        get => _grantRequestDescription;
        set
        {
            if (SetProperty(ref _grantRequestDescription, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.RequestDescription = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantAction
    {
        get => _grantAction;
        set
        {
            if (SetProperty(ref _grantAction, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.Action = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantBranch
    {
        get => _grantBranch;
        set
        {
            if (SetProperty(ref _grantBranch, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.Branch = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantOperatorGroup
    {
        get => _grantOperatorGroup;
        set
        {
            if (SetProperty(ref _grantOperatorGroup, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.OperatorGroup = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantOperator
    {
        get => _grantOperator;
        set
        {
            if (SetProperty(ref _grantOperator, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.Operator = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantCategory
    {
        get => _grantCategory;
        set
        {
            if (SetProperty(ref _grantCategory, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.Category = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantSubCategory
    {
        get => _grantSubCategory;
        set
        {
            if (SetProperty(ref _grantSubCategory, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.SubCategory = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantCallType
    {
        get => _grantCallType;
        set
        {
            if (SetProperty(ref _grantCallType, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.CallType = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantStatus
    {
        get => _grantStatus;
        set
        {
            if (SetProperty(ref _grantStatus, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.Status = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantImpact
    {
        get => _grantImpact;
        set
        {
            if (SetProperty(ref _grantImpact, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.Impact = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantPriority
    {
        get => _grantPriority;
        set
        {
            if (SetProperty(ref _grantPriority, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.Priority = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantDuration
    {
        get => _grantDuration;
        set
        {
            if (SetProperty(ref _grantDuration, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.Duration = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantEntryType
    {
        get => _grantEntryType;
        set
        {
            if (SetProperty(ref _grantEntryType, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.EntryType = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantUrgency
    {
        get => _grantUrgency;
        set
        {
            if (SetProperty(ref _grantUrgency, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.Urgency = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantProcessingStatus
    {
        get => _grantProcessingStatus;
        set
        {
            if (SetProperty(ref _grantProcessingStatus, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.ProcessingStatus = value;
                    NotifyModified();
                }
            }
        }
    }

    public bool GrantEnableGetAssets
    {
        get => _grantEnableGetAssets;
        set
        {
            if (SetProperty(ref _grantEnableGetAssets, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.EnableGetAssets = value;
                    NotifyModified();
                }
            }
        }
    }

    public bool GrantSkipNoAssetsFound
    {
        get => _grantSkipNoAssetsFound;
        set
        {
            if (SetProperty(ref _grantSkipNoAssetsFound, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.SkipNoAssetsFound = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantAssetsFilter
    {
        get => _grantAssetsFilter;
        set
        {
            if (SetProperty(ref _grantAssetsFilter, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.AssetsFilter = value;
                    NotifyModified();
                }
            }
        }
    }

    #endregion

    #region Revoke Section Properties

    public string? RevokeCaller
    {
        get => _revokeCaller;
        set
        {
            if (SetProperty(ref _revokeCaller, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.Caller = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeRequestShort
    {
        get => _revokeRequestShort;
        set
        {
            if (SetProperty(ref _revokeRequestShort, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.RequestShort = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeRequestDescription
    {
        get => _revokeRequestDescription;
        set
        {
            if (SetProperty(ref _revokeRequestDescription, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.RequestDescription = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeAction
    {
        get => _revokeAction;
        set
        {
            if (SetProperty(ref _revokeAction, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.Action = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeBranch
    {
        get => _revokeBranch;
        set
        {
            if (SetProperty(ref _revokeBranch, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.Branch = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeOperatorGroup
    {
        get => _revokeOperatorGroup;
        set
        {
            if (SetProperty(ref _revokeOperatorGroup, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.OperatorGroup = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeOperator
    {
        get => _revokeOperator;
        set
        {
            if (SetProperty(ref _revokeOperator, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.Operator = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeCategory
    {
        get => _revokeCategory;
        set
        {
            if (SetProperty(ref _revokeCategory, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.Category = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeSubCategory
    {
        get => _revokeSubCategory;
        set
        {
            if (SetProperty(ref _revokeSubCategory, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.SubCategory = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeCallType
    {
        get => _revokeCallType;
        set
        {
            if (SetProperty(ref _revokeCallType, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.CallType = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeStatus
    {
        get => _revokeStatus;
        set
        {
            if (SetProperty(ref _revokeStatus, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.Status = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeImpact
    {
        get => _revokeImpact;
        set
        {
            if (SetProperty(ref _revokeImpact, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.Impact = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokePriority
    {
        get => _revokePriority;
        set
        {
            if (SetProperty(ref _revokePriority, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.Priority = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeDuration
    {
        get => _revokeDuration;
        set
        {
            if (SetProperty(ref _revokeDuration, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.Duration = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeEntryType
    {
        get => _revokeEntryType;
        set
        {
            if (SetProperty(ref _revokeEntryType, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.EntryType = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeUrgency
    {
        get => _revokeUrgency;
        set
        {
            if (SetProperty(ref _revokeUrgency, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.Urgency = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeProcessingStatus
    {
        get => _revokeProcessingStatus;
        set
        {
            if (SetProperty(ref _revokeProcessingStatus, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.ProcessingStatus = value;
                    NotifyModified();
                }
            }
        }
    }

    public bool RevokeEnableGetAssets
    {
        get => _revokeEnableGetAssets;
        set
        {
            if (SetProperty(ref _revokeEnableGetAssets, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.EnableGetAssets = value;
                    NotifyModified();
                }
            }
        }
    }

    public bool RevokeSkipNoAssetsFound
    {
        get => _revokeSkipNoAssetsFound;
        set
        {
            if (SetProperty(ref _revokeSkipNoAssetsFound, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.SkipNoAssetsFound = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeAssetsFilter
    {
        get => _revokeAssetsFilter;
        set
        {
            if (SetProperty(ref _revokeAssetsFilter, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.AssetsFilter = value;
                    NotifyModified();
                }
            }
        }
    }

    #endregion

    /// <summary>
    /// Loads all properties from the current permission
    /// </summary>
    private void LoadPermission()
    {
        if (_permission == null)
        {
            ClearAll();
            return;
        }

        // Basic properties
        _id = _permission.Id;
        _displayName = _permission.DisplayName;

        // Set toggle states based on whether Grant/Revoke exist and have data
        _isGrantEnabled = _permission.Grant != null && !_permission.Grant.IsEmpty();
        _isRevokeEnabled = _permission.Revoke != null && !_permission.Revoke.IsEmpty();

        // Grant section
        if (_permission.Grant != null)
        {
            _grantCaller = _permission.Grant.Caller;
            _grantRequestShort = _permission.Grant.RequestShort;
            _grantRequestDescription = _permission.Grant.RequestDescription;
            _grantAction = _permission.Grant.Action;
            _grantBranch = _permission.Grant.Branch;
            _grantOperatorGroup = _permission.Grant.OperatorGroup;
            _grantOperator = _permission.Grant.Operator;
            _grantCategory = _permission.Grant.Category;
            _grantSubCategory = _permission.Grant.SubCategory;
            _grantCallType = _permission.Grant.CallType;
            _grantStatus = _permission.Grant.Status;
            _grantImpact = _permission.Grant.Impact;
            _grantPriority = _permission.Grant.Priority;
            _grantDuration = _permission.Grant.Duration;
            _grantEntryType = _permission.Grant.EntryType;
            _grantUrgency = _permission.Grant.Urgency;
            _grantProcessingStatus = _permission.Grant.ProcessingStatus;
            _grantEnableGetAssets = _permission.Grant.EnableGetAssets;
            _grantSkipNoAssetsFound = _permission.Grant.SkipNoAssetsFound;
            _grantAssetsFilter = _permission.Grant.AssetsFilter;
        }

        // Revoke section
        if (_permission.Revoke != null)
        {
            _revokeCaller = _permission.Revoke.Caller;
            _revokeRequestShort = _permission.Revoke.RequestShort;
            _revokeRequestDescription = _permission.Revoke.RequestDescription;
            _revokeAction = _permission.Revoke.Action;
            _revokeBranch = _permission.Revoke.Branch;
            _revokeOperatorGroup = _permission.Revoke.OperatorGroup;
            _revokeOperator = _permission.Revoke.Operator;
            _revokeCategory = _permission.Revoke.Category;
            _revokeSubCategory = _permission.Revoke.SubCategory;
            _revokeCallType = _permission.Revoke.CallType;
            _revokeStatus = _permission.Revoke.Status;
            _revokeImpact = _permission.Revoke.Impact;
            _revokePriority = _permission.Revoke.Priority;
            _revokeDuration = _permission.Revoke.Duration;
            _revokeEntryType = _permission.Revoke.EntryType;
            _revokeUrgency = _permission.Revoke.Urgency;
            _revokeProcessingStatus = _permission.Revoke.ProcessingStatus;
            _revokeEnableGetAssets = _permission.Revoke.EnableGetAssets;
            _revokeSkipNoAssetsFound = _permission.Revoke.SkipNoAssetsFound;
            _revokeAssetsFilter = _permission.Revoke.AssetsFilter;
        }

        // Notify all properties changed
        OnPropertyChanged(string.Empty);
    }

    private void ClearAll()
    {
        _id = string.Empty;
        _displayName = string.Empty;

        // Reset toggle states
        _isGrantEnabled = false;
        _isRevokeEnabled = false;

        // Clear all grant and revoke properties
        _grantCaller = null;
        _grantRequestShort = null;
        _grantRequestDescription = null;
        // ... (all other properties set to null/default)

        OnPropertyChanged(string.Empty);
    }

    private void NotifyModified()
    {
        _onModified?.Invoke();
    }
}
