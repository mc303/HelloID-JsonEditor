using HelloID.JsonEditor.Models;

namespace HelloID.JsonEditor.Services;

/// <summary>
/// Service interface for validating permission entries
/// </summary>
public interface IValidationService
{
    /// <summary>
    /// Validates a single incident permission entry
    /// </summary>
    ValidationResult ValidateIncidentPermission(IncidentPermission permission);

    /// <summary>
    /// Validates a single change permission entry
    /// </summary>
    ValidationResult ValidateChangePermission(ChangePermission permission);

    /// <summary>
    /// Validates that all IDs in a list are unique
    /// </summary>
    ValidationResult ValidateUniqueIds<T>(List<T> permissions, Func<T, string> idSelector);

    /// <summary>
    /// Validates all incident permissions in a list
    /// </summary>
    ValidationResult ValidateIncidentPermissions(List<IncidentPermission> permissions);

    /// <summary>
    /// Validates all change permissions in a list
    /// </summary>
    ValidationResult ValidateChangePermissions(List<ChangePermission> permissions);
}
