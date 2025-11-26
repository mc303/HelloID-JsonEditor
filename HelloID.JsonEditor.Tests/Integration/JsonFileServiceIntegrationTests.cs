using FluentAssertions;
using HelloID.JsonEditor.Services;

namespace HelloID.JsonEditor.Tests.Integration;

public class JsonFileServiceIntegrationTests
{
    private readonly JsonFileService _fileService;
    private readonly ValidationService _validationService;
    private readonly string _testDataPath;

    public JsonFileServiceIntegrationTests()
    {
        _fileService = new JsonFileService();
        _validationService = new ValidationService();

        // Get the path to TestData folder (test data files are copied to output directory)
        var currentDirectory = Directory.GetCurrentDirectory();
        _testDataPath = Path.Combine(currentDirectory, "TestData");
    }

    [Fact]
    public async Task LoadIncidentFile_ExampleFile_LoadsSuccessfully()
    {
        // Arrange
        var filePath = Path.Combine(_testDataPath, "example.incident.json");

        // Skip test if file doesn't exist (for CI/CD scenarios)
        if (!File.Exists(filePath))
        {
            // Make path absolute for better error message
            var absolutePath = Path.GetFullPath(filePath);
            throw new FileNotFoundException($"Test data file not found at: {absolutePath}");
        }

        // Act
        var permissions = await _fileService.LoadIncidentFileAsync(filePath);

        // Assert
        permissions.Should().NotBeNull();
        permissions.Should().NotBeEmpty();
        permissions.Should().HaveCount(4, "example file contains 4 incident permissions");
    }

    [Fact]
    public async Task LoadChangeFile_ExampleFile_LoadsSuccessfully()
    {
        // Arrange
        var filePath = Path.Combine(_testDataPath, "example.change.json");

        // Skip test if file doesn't exist
        if (!File.Exists(filePath))
        {
            var absolutePath = Path.GetFullPath(filePath);
            throw new FileNotFoundException($"Test data file not found at: {absolutePath}");
        }

        // Act
        var permissions = await _fileService.LoadChangeFileAsync(filePath);

        // Assert
        permissions.Should().NotBeNull();
        permissions.Should().NotBeEmpty();
        permissions.Should().HaveCount(4, "example file contains 4 change permissions");
    }

    [Fact]
    public async Task LoadIncidentFile_ValidatesCorrectly()
    {
        // Arrange
        var filePath = Path.Combine(_testDataPath, "example.incident.json");

        if (!File.Exists(filePath))
        {
            var absolutePath = Path.GetFullPath(filePath);
            throw new FileNotFoundException($"Test data file not found at: {absolutePath}");
        }

        // Act
        var permissions = await _fileService.LoadIncidentFileAsync(filePath);
        var validationResult = _validationService.ValidateIncidentPermissions(permissions);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeTrue($"example file should be valid, but got errors: {string.Join(", ", validationResult.Errors)}");

        // Verify structure
        permissions[0].Id.Should().Be("I001");
        permissions[0].DisplayName.Should().NotBeEmpty();
        permissions[0].Grant.Should().NotBeNull();
        permissions[0].Revoke.Should().NotBeNull();
    }

    [Fact]
    public async Task LoadChangeFile_ValidatesCorrectly()
    {
        // Arrange
        var filePath = Path.Combine(_testDataPath, "example.change.json");

        if (!File.Exists(filePath))
        {
            var absolutePath = Path.GetFullPath(filePath);
            throw new FileNotFoundException($"Test data file not found at: {absolutePath}");
        }

        // Act
        var permissions = await _fileService.LoadChangeFileAsync(filePath);
        var validationResult = _validationService.ValidateChangePermissions(permissions);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeTrue($"example file should be valid, but got errors: {string.Join(", ", validationResult.Errors)}");

        // Verify structure
        permissions[0].Id.Should().Be("C001");
        permissions[0].DisplayName.Should().NotBeEmpty();
        permissions[0].Grant.Should().NotBeNull();
        permissions[0].Revoke.Should().NotBeNull();
    }

    [Fact]
    public async Task DetectPermissionType_IncidentFile_ReturnsIncident()
    {
        // Arrange
        var filePath = Path.Combine(_testDataPath, "example.incident.json");

        if (!File.Exists(filePath))
        {
            var absolutePath = Path.GetFullPath(filePath);
            throw new FileNotFoundException($"Test data file not found at: {absolutePath}");
        }

        // Act
        var detectedType = await _fileService.DetectPermissionTypeAsync(filePath);

        // Assert
        detectedType.Should().NotBeNull();
        detectedType.Value.Should().Be(Models.PermissionType.Incident);
    }

    [Fact]
    public async Task DetectPermissionType_ChangeFile_ReturnsChange()
    {
        // Arrange
        var filePath = Path.Combine(_testDataPath, "example.change.json");

        if (!File.Exists(filePath))
        {
            var absolutePath = Path.GetFullPath(filePath);
            throw new FileNotFoundException($"Test data file not found at: {absolutePath}");
        }

        // Act
        var detectedType = await _fileService.DetectPermissionTypeAsync(filePath);

        // Assert
        detectedType.Should().NotBeNull();
        detectedType.Value.Should().Be(Models.PermissionType.Change);
    }

    [Fact]
    public async Task RoundTrip_IncidentFile_PreservesData()
    {
        // Arrange
        var originalPath = Path.Combine(_testDataPath, "example.incident.json");
        var tempPath = Path.Combine(Path.GetTempPath(), $"test_incident_{Guid.NewGuid()}.json");

        if (!File.Exists(originalPath))
        {
            var absolutePath = Path.GetFullPath(originalPath);
            throw new FileNotFoundException($"Test data file not found at: {absolutePath}");
        }

        try
        {
            // Act
            var originalPermissions = await _fileService.LoadIncidentFileAsync(originalPath);
            await _fileService.SaveIncidentFileAsync(tempPath, originalPermissions);
            var loadedPermissions = await _fileService.LoadIncidentFileAsync(tempPath);

            // Assert
            loadedPermissions.Should().HaveCount(originalPermissions.Count);

            for (int i = 0; i < originalPermissions.Count; i++)
            {
                loadedPermissions[i].Id.Should().Be(originalPermissions[i].Id);
                loadedPermissions[i].DisplayName.Should().Be(originalPermissions[i].DisplayName);
            }
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }
    }
}
