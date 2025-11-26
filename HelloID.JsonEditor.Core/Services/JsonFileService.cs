using System.Text.Json;
using HelloID.JsonEditor.Models;

namespace HelloID.JsonEditor.Services;

/// <summary>
/// Implementation of file service for JSON operations
/// </summary>
public class JsonFileService : IFileService
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
        PropertyNameCaseInsensitive = true
    };

    public async Task<List<IncidentPermission>> LoadIncidentFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        try
        {
            var json = await File.ReadAllTextAsync(filePath);
            var permissions = JsonSerializer.Deserialize<List<IncidentPermission>>(json, _jsonOptions);

            return permissions ?? new List<IncidentPermission>();
        }
        catch (JsonException ex)
        {
            throw new JsonException($"Invalid JSON format in file: {filePath}", ex);
        }
    }

    public async Task<List<ChangePermission>> LoadChangeFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        try
        {
            var json = await File.ReadAllTextAsync(filePath);
            var permissions = JsonSerializer.Deserialize<List<ChangePermission>>(json, _jsonOptions);

            return permissions ?? new List<ChangePermission>();
        }
        catch (JsonException ex)
        {
            throw new JsonException($"Invalid JSON format in file: {filePath}", ex);
        }
    }

    public async Task SaveIncidentFileAsync(string filePath, List<IncidentPermission> permissions)
    {
        try
        {
            // Create backup of existing file
            if (File.Exists(filePath))
            {
                var backupPath = $"{filePath}.backup";
                File.Copy(filePath, backupPath, overwrite: true);
            }

            var json = JsonSerializer.Serialize(permissions, _jsonOptions);
            await File.WriteAllTextAsync(filePath, json);
        }
        catch (Exception ex)
        {
            throw new IOException($"Failed to save file: {filePath}", ex);
        }
    }

    public async Task SaveChangeFileAsync(string filePath, List<ChangePermission> permissions)
    {
        try
        {
            // Create backup of existing file
            if (File.Exists(filePath))
            {
                var backupPath = $"{filePath}.backup";
                File.Copy(filePath, backupPath, overwrite: true);
            }

            var json = JsonSerializer.Serialize(permissions, _jsonOptions);
            await File.WriteAllTextAsync(filePath, json);
        }
        catch (Exception ex)
        {
            throw new IOException($"Failed to save file: {filePath}", ex);
        }
    }

    public bool ValidateJsonFile(string filePath, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (!File.Exists(filePath))
        {
            errorMessage = "File does not exist";
            return false;
        }

        try
        {
            var json = File.ReadAllText(filePath);

            // Try to parse as generic JSON to check validity
            using var document = JsonDocument.Parse(json);

            if (document.RootElement.ValueKind != JsonValueKind.Array)
            {
                errorMessage = "JSON must be an array of permission objects";
                return false;
            }

            return true;
        }
        catch (JsonException ex)
        {
            errorMessage = $"Invalid JSON: {ex.Message}";
            return false;
        }
        catch (Exception ex)
        {
            errorMessage = $"Error reading file: {ex.Message}";
            return false;
        }
    }

    public async Task<PermissionType?> DetectPermissionTypeAsync(string filePath)
    {
        try
        {
            var json = await File.ReadAllTextAsync(filePath);
            using var document = JsonDocument.Parse(json);

            if (document.RootElement.ValueKind != JsonValueKind.Array ||
                document.RootElement.GetArrayLength() == 0)
            {
                return null;
            }

            var firstElement = document.RootElement[0];

            // Check if Grant/Revoke sections exist
            if (!firstElement.TryGetProperty("Grant", out var grantSection))
            {
                return null;
            }

            // Check for incident-specific properties
            if (grantSection.TryGetProperty("Caller", out _) ||
                grantSection.TryGetProperty("RequestShort", out _))
            {
                return PermissionType.Incident;
            }

            // Check for change-specific properties
            if (grantSection.TryGetProperty("Requester", out _) ||
                grantSection.TryGetProperty("BriefDescription", out _))
            {
                return PermissionType.Change;
            }

            return null;
        }
        catch
        {
            return null;
        }
    }
}
