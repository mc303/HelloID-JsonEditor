using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace HelloID.JsonEditor.Models;

/// <summary>
/// Represents a change permission entry in the change.json file
/// </summary>
public class ChangePermission : INotifyPropertyChanged
{
    private Identification _identification = new();
    private string _displayName = string.Empty;

    [JsonPropertyName("Identification")]
    public Identification Identification
    {
        get => _identification;
        set
        {
            _identification = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Id));
        }
    }

    /// <summary>
    /// Convenience property to get/set the Id directly
    /// </summary>
    [JsonIgnore]
    public string Id
    {
        get => Identification?.Id ?? string.Empty;
        set
        {
            if (Identification == null)
                Identification = new Identification();
            Identification.Id = value;
            OnPropertyChanged();
        }
    }

    [JsonPropertyName("DisplayName")]
    public string DisplayName
    {
        get => _displayName;
        set
        {
            if (_displayName != value)
            {
                _displayName = value;
                OnPropertyChanged();
            }
        }
    }

    [JsonPropertyName("Grant")]
    public ChangeSection? Grant { get; set; }

    [JsonPropertyName("Revoke")]
    public ChangeSection? Revoke { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

/// <summary>
/// Represents the Grant or Revoke section of a change permission
/// </summary>
public class ChangeSection
{
    [JsonPropertyName("Requester")]
    public string? Requester { get; set; }

    [JsonPropertyName("Request")]
    public string? Request { get; set; }

    [JsonPropertyName("Action")]
    public string? Action { get; set; }

    [JsonPropertyName("BriefDescription")]
    public string? BriefDescription { get; set; }

    [JsonPropertyName("Template")]
    public string? Template { get; set; }

    [JsonPropertyName("Category")]
    public string? Category { get; set; }

    [JsonPropertyName("SubCategory")]
    public string? SubCategory { get; set; }

    [JsonPropertyName("ChangeType")]
    public string? ChangeType { get; set; }

    [JsonPropertyName("Impact")]
    public string? Impact { get; set; }

    [JsonPropertyName("Benefit")]
    public string? Benefit { get; set; }

    [JsonPropertyName("Priority")]
    public string? Priority { get; set; }

    [JsonPropertyName("EnableGetAssets")]
    public bool EnableGetAssets { get; set; }

    [JsonPropertyName("SkipNoAssetsFound")]
    public bool SkipNoAssetsFound { get; set; }

    [JsonPropertyName("AssetsFilter")]
    public string? AssetsFilter { get; set; }

    /// <summary>
    /// Checks if this section is empty (all fields are null or default)
    /// </summary>
    public bool IsEmpty()
    {
        return string.IsNullOrWhiteSpace(Requester) &&
               string.IsNullOrWhiteSpace(Request) &&
               string.IsNullOrWhiteSpace(Action) &&
               string.IsNullOrWhiteSpace(BriefDescription) &&
               string.IsNullOrWhiteSpace(Template) &&
               string.IsNullOrWhiteSpace(Category) &&
               string.IsNullOrWhiteSpace(SubCategory) &&
               string.IsNullOrWhiteSpace(ChangeType) &&
               string.IsNullOrWhiteSpace(Impact) &&
               string.IsNullOrWhiteSpace(Benefit) &&
               string.IsNullOrWhiteSpace(Priority) &&
               !EnableGetAssets &&
               !SkipNoAssetsFound &&
               string.IsNullOrWhiteSpace(AssetsFilter);
    }
}
