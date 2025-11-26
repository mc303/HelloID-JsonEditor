using System.Text.Json.Serialization;

namespace HelloID.JsonEditor.Models;

/// <summary>
/// Represents the Identification object that contains the Id
/// </summary>
public class Identification
{
    /// <summary>
    /// The unique identifier
    /// </summary>
    [JsonPropertyName("Id")]
    public string? Id { get; set; }
}
