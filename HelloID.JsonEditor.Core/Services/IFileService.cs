using HelloID.JsonEditor.Models;

namespace HelloID.JsonEditor.Services;

/// <summary>
/// Service interface for file operations (loading and saving permission JSON files)
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Loads an incident permission file from the specified path
    /// </summary>
    /// <param name="filePath">Full path to the incident.json file</param>
    /// <returns>List of incident permissions</returns>
    /// <exception cref="FileNotFoundException">Thrown when file doesn't exist</exception>
    /// <exception cref="JsonException">Thrown when JSON is invalid</exception>
    Task<List<IncidentPermission>> LoadIncidentFileAsync(string filePath);

    /// <summary>
    /// Loads a change permission file from the specified path
    /// </summary>
    /// <param name="filePath">Full path to the change.json file</param>
    /// <returns>List of change permissions</returns>
    /// <exception cref="FileNotFoundException">Thrown when file doesn't exist</exception>
    /// <exception cref="JsonException">Thrown when JSON is invalid</exception>
    Task<List<ChangePermission>> LoadChangeFileAsync(string filePath);

    /// <summary>
    /// Saves incident permissions to the specified file path
    /// </summary>
    /// <param name="filePath">Full path where to save the file</param>
    /// <param name="permissions">List of incident permissions to save</param>
    Task SaveIncidentFileAsync(string filePath, List<IncidentPermission> permissions);

    /// <summary>
    /// Saves change permissions to the specified file path
    /// </summary>
    /// <param name="filePath">Full path where to save the file</param>
    /// <param name="permissions">List of change permissions to save</param>
    Task SaveChangeFileAsync(string filePath, List<ChangePermission> permissions);

    /// <summary>
    /// Validates if the file at the given path is a valid JSON file
    /// </summary>
    /// <param name="filePath">Path to the file to validate</param>
    /// <param name="errorMessage">Output parameter containing error details if validation fails</param>
    /// <returns>True if valid, false otherwise</returns>
    bool ValidateJsonFile(string filePath, out string errorMessage);

    /// <summary>
    /// Detects the permission type from the JSON file structure
    /// </summary>
    /// <param name="filePath">Path to the JSON file</param>
    /// <returns>Detected permission type or null if unable to determine</returns>
    Task<PermissionType?> DetectPermissionTypeAsync(string filePath);
}
