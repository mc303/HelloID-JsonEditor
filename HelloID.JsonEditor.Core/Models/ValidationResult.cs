namespace HelloID.JsonEditor.Models;

/// <summary>
/// Represents the result of a validation operation
/// </summary>
public class ValidationResult
{
    public bool IsValid => Errors.Count == 0;
    public List<string> Errors { get; }

    public ValidationResult()
    {
        Errors = new List<string>();
    }

    public ValidationResult(List<string> errors)
    {
        Errors = errors ?? new List<string>();
    }

    public ValidationResult(string error)
    {
        Errors = new List<string> { error };
    }

    public void AddError(string error)
    {
        Errors.Add(error);
    }

    public void AddErrors(IEnumerable<string> errors)
    {
        Errors.AddRange(errors);
    }

    public override string ToString()
    {
        return IsValid ? "Valid" : string.Join(Environment.NewLine, Errors);
    }
}
