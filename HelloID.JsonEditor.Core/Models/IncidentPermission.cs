using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace HelloID.JsonEditor.Models;

/// <summary>
/// Represents an incident permission entry in the incident.json file
/// </summary>
public class IncidentPermission : INotifyPropertyChanged
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
    public IncidentSection? Grant { get; set; }

    [JsonPropertyName("Revoke")]
    public IncidentSection? Revoke { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

/// <summary>
/// Represents the Grant or Revoke section of an incident permission
/// </summary>
public class IncidentSection
{
    [JsonPropertyName("Caller")]
    public string? Caller { get; set; }

    [JsonPropertyName("RequestShort")]
    public string? RequestShort { get; set; }

    [JsonPropertyName("RequestDescription")]
    public string? RequestDescription { get; set; }

    [JsonPropertyName("Action")]
    public string? Action { get; set; }

    [JsonPropertyName("Branch")]
    public string? Branch { get; set; }

    [JsonPropertyName("OperatorGroup")]
    public string? OperatorGroup { get; set; }

    [JsonPropertyName("Operator")]
    public string? Operator { get; set; }

    [JsonPropertyName("Category")]
    public string? Category { get; set; }

    [JsonPropertyName("SubCategory")]
    public string? SubCategory { get; set; }

    [JsonPropertyName("CallType")]
    public string? CallType { get; set; }

    [JsonPropertyName("Status")]
    public string? Status { get; set; }

    [JsonPropertyName("Impact")]
    public string? Impact { get; set; }

    [JsonPropertyName("Priority")]
    public string? Priority { get; set; }

    [JsonPropertyName("Duration")]
    public string? Duration { get; set; }

    [JsonPropertyName("EntryType")]
    public string? EntryType { get; set; }

    [JsonPropertyName("Urgency")]
    public string? Urgency { get; set; }

    [JsonPropertyName("ProcessingStatus")]
    public string? ProcessingStatus { get; set; }

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
        return string.IsNullOrWhiteSpace(Caller) &&
               string.IsNullOrWhiteSpace(RequestShort) &&
               string.IsNullOrWhiteSpace(RequestDescription) &&
               string.IsNullOrWhiteSpace(Action) &&
               string.IsNullOrWhiteSpace(Branch) &&
               string.IsNullOrWhiteSpace(OperatorGroup) &&
               string.IsNullOrWhiteSpace(Operator) &&
               string.IsNullOrWhiteSpace(Category) &&
               string.IsNullOrWhiteSpace(SubCategory) &&
               string.IsNullOrWhiteSpace(CallType) &&
               string.IsNullOrWhiteSpace(Status) &&
               string.IsNullOrWhiteSpace(Impact) &&
               string.IsNullOrWhiteSpace(Priority) &&
               string.IsNullOrWhiteSpace(Duration) &&
               string.IsNullOrWhiteSpace(EntryType) &&
               string.IsNullOrWhiteSpace(Urgency) &&
               string.IsNullOrWhiteSpace(ProcessingStatus) &&
               !EnableGetAssets &&
               !SkipNoAssetsFound &&
               string.IsNullOrWhiteSpace(AssetsFilter);
    }
}
