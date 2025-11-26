using HelloID.JsonEditor.Models;

namespace HelloID.JsonEditor.Services;

/// <summary>
/// Service for validating permission entries
/// </summary>
public class ValidationService : IValidationService
{
    public ValidationResult ValidateIncidentPermission(IncidentPermission permission)
    {
        var result = new ValidationResult();

        if (permission == null)
        {
            result.AddError("Permission cannot be null");
            return result;
        }

        // Validate Id
        if (string.IsNullOrWhiteSpace(permission.Id))
        {
            result.AddError("Id is required and cannot be empty");
        }

        // Validate DisplayName
        if (string.IsNullOrWhiteSpace(permission.DisplayName))
        {
            result.AddError("DisplayName is required and cannot be empty");
        }

        // Note: Grant and Revoke sections are optional - user may be creating a new file
        // and will fill them in later. We don't enforce that they must have content.

        return result;
    }

    public ValidationResult ValidateChangePermission(ChangePermission permission)
    {
        var result = new ValidationResult();

        if (permission == null)
        {
            result.AddError("Permission cannot be null");
            return result;
        }

        // Validate Id
        if (string.IsNullOrWhiteSpace(permission.Id))
        {
            result.AddError("Id is required and cannot be empty");
        }

        // Validate DisplayName
        if (string.IsNullOrWhiteSpace(permission.DisplayName))
        {
            result.AddError("DisplayName is required and cannot be empty");
        }

        // Note: Grant and Revoke sections are optional - user may be creating a new file
        // and will fill them in later. We don't enforce that they must have content.

        return result;
    }

    public ValidationResult ValidateUniqueIds<T>(List<T> permissions, Func<T, string> idSelector)
    {
        var result = new ValidationResult();

        if (permissions == null || permissions.Count == 0)
        {
            return result; // Empty list is valid
        }

        // Group by Id and find duplicates
        var duplicateGroups = permissions
            .GroupBy(idSelector)
            .Where(g => g.Count() > 1)
            .ToList();

        foreach (var group in duplicateGroups)
        {
            var id = group.Key;
            var count = group.Count();
            result.AddError($"Duplicate Id found: '{id}' appears {count} times");
        }

        // Check for empty Ids
        var emptyIdCount = permissions.Count(p => string.IsNullOrWhiteSpace(idSelector(p)));
        if (emptyIdCount > 0)
        {
            result.AddError($"Found {emptyIdCount} permission(s) with empty or null Id");
        }

        return result;
    }

    public ValidationResult ValidateIncidentPermissions(List<IncidentPermission> permissions)
    {
        var result = new ValidationResult();

        if (permissions == null)
        {
            result.AddError("Permissions list cannot be null");
            return result;
        }

        if (permissions.Count == 0)
        {
            result.AddError("Permissions list is empty");
            return result;
        }

        // Validate unique Ids
        var uniqueIdResult = ValidateUniqueIds(permissions, p => p.Id);
        if (!uniqueIdResult.IsValid)
        {
            result.AddErrors(uniqueIdResult.Errors);
        }

        // Validate each permission individually
        for (int i = 0; i < permissions.Count; i++)
        {
            var permissionResult = ValidateIncidentPermission(permissions[i]);
            if (!permissionResult.IsValid)
            {
                foreach (var error in permissionResult.Errors)
                {
                    result.AddError($"Permission #{i + 1} (Id: '{permissions[i]?.Id ?? "null"}'): {error}");
                }
            }
        }

        return result;
    }

    public ValidationResult ValidateChangePermissions(List<ChangePermission> permissions)
    {
        var result = new ValidationResult();

        if (permissions == null)
        {
            result.AddError("Permissions list cannot be null");
            return result;
        }

        if (permissions.Count == 0)
        {
            result.AddError("Permissions list is empty");
            return result;
        }

        // Validate unique Ids
        var uniqueIdResult = ValidateUniqueIds(permissions, p => p.Id);
        if (!uniqueIdResult.IsValid)
        {
            result.AddErrors(uniqueIdResult.Errors);
        }

        // Validate each permission individually
        for (int i = 0; i < permissions.Count; i++)
        {
            var permissionResult = ValidateChangePermission(permissions[i]);
            if (!permissionResult.IsValid)
            {
                foreach (var error in permissionResult.Errors)
                {
                    result.AddError($"Permission #{i + 1} (Id: '{permissions[i]?.Id ?? "null"}'): {error}");
                }
            }
        }

        return result;
    }
}
