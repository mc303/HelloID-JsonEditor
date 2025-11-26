using System.Windows;
using CommunityToolkit.Mvvm.Input;
using HelloID.JsonEditor.Models;

namespace HelloID.JsonEditor.UI.ViewModels;

/// <summary>
/// ViewModel for editing an individual change permission
/// Provides two-way binding for all properties
/// </summary>
public partial class ChangeEditorViewModel : ViewModelBase
{
    private ChangePermission? _permission;
    private readonly Action? _onModified;
    private bool _isGrantEnabled;
    private bool _isRevokeEnabled;

    /// <summary>
    /// The change permission being edited
    /// </summary>
    public ChangePermission? Permission
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
    private string? _grantRequester;
    private string? _grantRequest;
    private string? _grantAction;
    private string? _grantBriefDescription;
    private string? _grantTemplate;
    private string? _grantCategory;
    private string? _grantSubCategory;
    private string? _grantChangeType;
    private string? _grantImpact;
    private string? _grantBenefit;
    private string? _grantPriority;
    private bool _grantEnableGetAssets;
    private bool _grantSkipNoAssetsFound;
    private string? _grantAssetsFilter;

    // Revoke section properties
    private string? _revokeRequester;
    private string? _revokeRequest;
    private string? _revokeAction;
    private string? _revokeBriefDescription;
    private string? _revokeTemplate;
    private string? _revokeCategory;
    private string? _revokeSubCategory;
    private string? _revokeChangeType;
    private string? _revokeImpact;
    private string? _revokeBenefit;
    private string? _revokePriority;
    private bool _revokeEnableGetAssets;
    private bool _revokeSkipNoAssetsFound;
    private string? _revokeAssetsFilter;

    public ChangeEditorViewModel(Action? onModified = null)
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
            _permission.Grant = new ChangeSection();
            NotifyModified();
        }
    }

    private void EnsureRevokeExists()
    {
        if (_permission != null && _permission.Revoke == null)
        {
            _permission.Revoke = new ChangeSection();
            NotifyModified();
        }
    }

    #endregion

    #region Grant Section Properties

    public string? GrantRequester
    {
        get => _grantRequester;
        set
        {
            if (SetProperty(ref _grantRequester, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.Requester = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantRequest
    {
        get => _grantRequest;
        set
        {
            if (SetProperty(ref _grantRequest, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.Request = value;
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

    public string? GrantBriefDescription
    {
        get => _grantBriefDescription;
        set
        {
            if (SetProperty(ref _grantBriefDescription, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.BriefDescription = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? GrantTemplate
    {
        get => _grantTemplate;
        set
        {
            if (SetProperty(ref _grantTemplate, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.Template = value;
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

    public string? GrantChangeType
    {
        get => _grantChangeType;
        set
        {
            if (SetProperty(ref _grantChangeType, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.ChangeType = value;
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

    public string? GrantBenefit
    {
        get => _grantBenefit;
        set
        {
            if (SetProperty(ref _grantBenefit, value))
            {
                if (_permission?.Grant != null)
                {
                    _permission.Grant.Benefit = value;
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

    public string? RevokeRequester
    {
        get => _revokeRequester;
        set
        {
            if (SetProperty(ref _revokeRequester, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.Requester = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeRequest
    {
        get => _revokeRequest;
        set
        {
            if (SetProperty(ref _revokeRequest, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.Request = value;
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

    public string? RevokeBriefDescription
    {
        get => _revokeBriefDescription;
        set
        {
            if (SetProperty(ref _revokeBriefDescription, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.BriefDescription = value;
                    NotifyModified();
                }
            }
        }
    }

    public string? RevokeTemplate
    {
        get => _revokeTemplate;
        set
        {
            if (SetProperty(ref _revokeTemplate, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.Template = value;
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

    public string? RevokeChangeType
    {
        get => _revokeChangeType;
        set
        {
            if (SetProperty(ref _revokeChangeType, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.ChangeType = value;
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

    public string? RevokeBenefit
    {
        get => _revokeBenefit;
        set
        {
            if (SetProperty(ref _revokeBenefit, value))
            {
                if (_permission?.Revoke != null)
                {
                    _permission.Revoke.Benefit = value;
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
            _grantRequester = _permission.Grant.Requester;
            _grantRequest = _permission.Grant.Request;
            _grantAction = _permission.Grant.Action;
            _grantBriefDescription = _permission.Grant.BriefDescription;
            _grantTemplate = _permission.Grant.Template;
            _grantCategory = _permission.Grant.Category;
            _grantSubCategory = _permission.Grant.SubCategory;
            _grantChangeType = _permission.Grant.ChangeType;
            _grantImpact = _permission.Grant.Impact;
            _grantBenefit = _permission.Grant.Benefit;
            _grantPriority = _permission.Grant.Priority;
            _grantEnableGetAssets = _permission.Grant.EnableGetAssets;
            _grantSkipNoAssetsFound = _permission.Grant.SkipNoAssetsFound;
            _grantAssetsFilter = _permission.Grant.AssetsFilter;
        }

        // Revoke section
        if (_permission.Revoke != null)
        {
            _revokeRequester = _permission.Revoke.Requester;
            _revokeRequest = _permission.Revoke.Request;
            _revokeAction = _permission.Revoke.Action;
            _revokeBriefDescription = _permission.Revoke.BriefDescription;
            _revokeTemplate = _permission.Revoke.Template;
            _revokeCategory = _permission.Revoke.Category;
            _revokeSubCategory = _permission.Revoke.SubCategory;
            _revokeChangeType = _permission.Revoke.ChangeType;
            _revokeImpact = _permission.Revoke.Impact;
            _revokeBenefit = _permission.Revoke.Benefit;
            _revokePriority = _permission.Revoke.Priority;
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
        _grantRequester = null;
        _grantRequest = null;
        _grantAction = null;
        _grantBriefDescription = null;
        _grantTemplate = null;
        _grantCategory = null;
        _grantSubCategory = null;
        _grantChangeType = null;
        _grantImpact = null;
        _grantBenefit = null;
        _grantPriority = null;
        _grantEnableGetAssets = false;
        _grantSkipNoAssetsFound = false;
        _grantAssetsFilter = null;

        _revokeRequester = null;
        _revokeRequest = null;
        _revokeAction = null;
        _revokeBriefDescription = null;
        _revokeTemplate = null;
        _revokeCategory = null;
        _revokeSubCategory = null;
        _revokeChangeType = null;
        _revokeImpact = null;
        _revokeBenefit = null;
        _revokePriority = null;
        _revokeEnableGetAssets = false;
        _revokeSkipNoAssetsFound = false;
        _revokeAssetsFilter = null;

        OnPropertyChanged(string.Empty);
    }

    private void NotifyModified()
    {
        _onModified?.Invoke();
    }
}
