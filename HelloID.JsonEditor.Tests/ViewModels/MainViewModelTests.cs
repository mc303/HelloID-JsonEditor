using FluentAssertions;
using Moq;
using HelloID.JsonEditor.Models;
using HelloID.JsonEditor.Services;
using HelloID.JsonEditor.UI.ViewModels;

namespace HelloID.JsonEditor.Tests.ViewModels;

public class MainViewModelTests
{
    private readonly Mock<IFileService> _mockFileService;
    private readonly Mock<IValidationService> _mockValidationService;
    private readonly MainViewModel _viewModel;

    public MainViewModelTests()
    {
        _mockFileService = new Mock<IFileService>();
        _mockValidationService = new Mock<IValidationService>();
        _viewModel = new MainViewModel(_mockFileService.Object, _mockValidationService.Object);
    }

    [Fact]
    public void Constructor_WithNullFileService_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new MainViewModel(null!, _mockValidationService.Object));
    }

    [Fact]
    public void Constructor_WithNullValidationService_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new MainViewModel(_mockFileService.Object, null!));
    }

    [Fact]
    public async Task OpenFileAsync_WithIncidentFile_LoadsPermissions()
    {
        // Arrange
        var testFilePath = "test.incident.json";
        var testPermissions = new List<IncidentPermission>
        {
            new() { Id = "I001", DisplayName = "Test Permission" }
        };

        _mockFileService
            .Setup(x => x.DetectPermissionTypeAsync(testFilePath))
            .ReturnsAsync(PermissionType.Incident);

        _mockFileService
            .Setup(x => x.LoadIncidentFileAsync(testFilePath))
            .ReturnsAsync(testPermissions);

        _mockValidationService
            .Setup(x => x.ValidateIncidentPermissions(It.IsAny<List<IncidentPermission>>()))
            .Returns(new ValidationResult());

        // Act
        await _viewModel.OpenFileCommand.ExecuteAsync(testFilePath);

        // Assert
        _viewModel.IncidentPermissions.Should().HaveCount(1);
        _viewModel.IncidentPermissions[0].Id.Should().Be("I001");
        _viewModel.CurrentPermissionType.Should().Be(PermissionType.Incident);
        _viewModel.IsIncidentFile.Should().BeTrue();
        _viewModel.IsChangeFile.Should().BeFalse();
        _viewModel.HasUnsavedChanges.Should().BeFalse();
    }

    [Fact]
    public async Task OpenFileAsync_WithChangeFile_LoadsPermissions()
    {
        // Arrange
        var testFilePath = "test.change.json";
        var testPermissions = new List<ChangePermission>
        {
            new() { Id = "C001", DisplayName = "Test Permission" }
        };

        _mockFileService
            .Setup(x => x.DetectPermissionTypeAsync(testFilePath))
            .ReturnsAsync(PermissionType.Change);

        _mockFileService
            .Setup(x => x.LoadChangeFileAsync(testFilePath))
            .ReturnsAsync(testPermissions);

        _mockValidationService
            .Setup(x => x.ValidateChangePermissions(It.IsAny<List<ChangePermission>>()))
            .Returns(new ValidationResult());

        // Act
        await _viewModel.OpenFileCommand.ExecuteAsync(testFilePath);

        // Assert
        _viewModel.ChangePermissions.Should().HaveCount(1);
        _viewModel.ChangePermissions[0].Id.Should().Be("C001");
        _viewModel.CurrentPermissionType.Should().Be(PermissionType.Change);
        _viewModel.IsChangeFile.Should().BeTrue();
        _viewModel.IsIncidentFile.Should().BeFalse();
    }

    [Fact]
    public async Task OpenFileAsync_WithUndetectableType_DoesNotLoad()
    {
        // Arrange
        var testFilePath = "test.json";

        _mockFileService
            .Setup(x => x.DetectPermissionTypeAsync(testFilePath))
            .ReturnsAsync((PermissionType?)null);

        // Act
        await _viewModel.OpenFileCommand.ExecuteAsync(testFilePath);

        // Assert
        _viewModel.IncidentPermissions.Should().BeEmpty();
        _viewModel.ChangePermissions.Should().BeEmpty();
        _viewModel.CurrentPermissionType.Should().BeNull();
        _viewModel.StatusMessage.Should().Contain("Unable to determine file type");
    }

    [Fact]
    public void AddNewIncident_CreatesNewPermission()
    {
        // Act
        _viewModel.AddNewIncidentCommand.Execute(null);

        // Assert
        _viewModel.IncidentPermissions.Should().HaveCount(1);
        _viewModel.IncidentPermissions[0].Id.Should().Be("I001");
        _viewModel.IncidentPermissions[0].DisplayName.Should().Be("New Incident Permission");
        _viewModel.HasUnsavedChanges.Should().BeTrue();
        _viewModel.SelectedPermission.Should().Be(_viewModel.IncidentPermissions[0]);
    }

    [Fact]
    public void AddNewIncident_GeneratesUniqueIds()
    {
        // Arrange - Add first permission
        _viewModel.AddNewIncidentCommand.Execute(null);

        // Act - Add second permission
        _viewModel.AddNewIncidentCommand.Execute(null);

        // Assert
        _viewModel.IncidentPermissions.Should().HaveCount(2);
        _viewModel.IncidentPermissions[0].Id.Should().Be("I001");
        _viewModel.IncidentPermissions[1].Id.Should().Be("I002");
    }

    [Fact]
    public void AddNewChange_CreatesNewPermission()
    {
        // Act
        _viewModel.AddNewChangeCommand.Execute(null);

        // Assert
        _viewModel.ChangePermissions.Should().HaveCount(1);
        _viewModel.ChangePermissions[0].Id.Should().Be("C001");
        _viewModel.ChangePermissions[0].DisplayName.Should().Be("New Change Permission");
        _viewModel.HasUnsavedChanges.Should().BeTrue();
    }

    [Fact]
    public void Delete_WithIncidentPermission_RemovesFromCollection()
    {
        // Arrange
        _viewModel.AddNewIncidentCommand.Execute(null);
        var permission = _viewModel.IncidentPermissions[0];
        _viewModel.SelectedPermission = permission;
        _viewModel.HasUnsavedChanges = false; // Reset

        // Act
        _viewModel.DeleteCommand.Execute(null);

        // Assert
        _viewModel.IncidentPermissions.Should().BeEmpty();
        _viewModel.SelectedPermission.Should().BeNull();
        _viewModel.HasUnsavedChanges.Should().BeTrue();
    }

    [Fact]
    public void Delete_WithChangePermission_RemovesFromCollection()
    {
        // Arrange
        _viewModel.AddNewChangeCommand.Execute(null);
        var permission = _viewModel.ChangePermissions[0];
        _viewModel.SelectedPermission = permission;
        _viewModel.HasUnsavedChanges = false; // Reset

        // Act
        _viewModel.DeleteCommand.Execute(null);

        // Assert
        _viewModel.ChangePermissions.Should().BeEmpty();
        _viewModel.SelectedPermission.Should().BeNull();
        _viewModel.HasUnsavedChanges.Should().BeTrue();
    }

    [Fact]
    public void Delete_WithNoSelection_CannotExecute()
    {
        // Arrange
        _viewModel.SelectedPermission = null;

        // Act & Assert
        _viewModel.DeleteCommand.CanExecute(null).Should().BeFalse();
    }

    [Fact]
    public void Duplicate_WithIncidentPermission_CreatesCopy()
    {
        // Arrange
        _viewModel.AddNewIncidentCommand.Execute(null);
        var original = _viewModel.IncidentPermissions[0];
        original.DisplayName = "Original Permission";
        _viewModel.SelectedPermission = original;

        // Act
        _viewModel.DuplicateCommand.Execute(null);

        // Assert
        _viewModel.IncidentPermissions.Should().HaveCount(2);
        var duplicate = _viewModel.IncidentPermissions[1];
        duplicate.Id.Should().Be("I002");
        duplicate.DisplayName.Should().Be("Original Permission (Copy)");
        _viewModel.SelectedPermission.Should().Be(duplicate);
        _viewModel.HasUnsavedChanges.Should().BeTrue();
    }

    [Fact]
    public void Duplicate_WithChangePermission_CreatesCopy()
    {
        // Arrange
        _viewModel.AddNewChangeCommand.Execute(null);
        var original = _viewModel.ChangePermissions[0];
        original.DisplayName = "Original Permission";
        _viewModel.SelectedPermission = original;

        // Act
        _viewModel.DuplicateCommand.Execute(null);

        // Assert
        _viewModel.ChangePermissions.Should().HaveCount(2);
        var duplicate = _viewModel.ChangePermissions[1];
        duplicate.Id.Should().Be("C002");
        duplicate.DisplayName.Should().Be("Original Permission (Copy)");
    }

    [Fact]
    public void MarkAsModified_SetsHasUnsavedChanges()
    {
        // Arrange
        _viewModel.HasUnsavedChanges = false;

        // Act
        _viewModel.MarkAsModified();

        // Assert
        _viewModel.HasUnsavedChanges.Should().BeTrue();
    }

    [Fact]
    public void WindowTitle_IncludesFileName()
    {
        // Act
        var title = _viewModel.WindowTitle;

        // Assert
        title.Should().Contain("TOPdesk Permissions Editor");
        title.Should().Contain("Untitled");
    }

    [Fact]
    public void WindowTitle_WithUnsavedChanges_ShowsAsterisk()
    {
        // Arrange
        _viewModel.HasUnsavedChanges = true;

        // Act
        var title = _viewModel.WindowTitle;

        // Assert
        title.Should().EndWith("*");
    }

    [Fact]
    public async Task SaveAsync_WithValidIncidentPermissions_SavesFile()
    {
        // Arrange
        var testFilePath = "test.incident.json";
        await LoadTestIncidentFile(testFilePath);
        _viewModel.HasUnsavedChanges = true;

        _mockValidationService
            .Setup(x => x.ValidateIncidentPermissions(It.IsAny<List<IncidentPermission>>()))
            .Returns(new ValidationResult());

        // Act
        await _viewModel.SaveCommand.ExecuteAsync(null);

        // Assert
        _mockFileService.Verify(x =>
            x.SaveIncidentFileAsync(testFilePath, It.IsAny<List<IncidentPermission>>()), Times.Once);
        _viewModel.HasUnsavedChanges.Should().BeFalse();
    }

    [Fact]
    public async Task SaveAsync_WithValidationErrors_DoesNotSave()
    {
        // Arrange
        var testFilePath = "test.incident.json";
        await LoadTestIncidentFile(testFilePath);
        _viewModel.HasUnsavedChanges = true;

        var validationResult = new ValidationResult();
        validationResult.AddError("Test error");

        _mockValidationService
            .Setup(x => x.ValidateIncidentPermissions(It.IsAny<List<IncidentPermission>>()))
            .Returns(validationResult);

        // Act
        await _viewModel.SaveCommand.ExecuteAsync(null);

        // Assert
        _mockFileService.Verify(x =>
            x.SaveIncidentFileAsync(It.IsAny<string>(), It.IsAny<List<IncidentPermission>>()), Times.Never);
        _viewModel.HasUnsavedChanges.Should().BeTrue();
        _viewModel.StatusMessage.Should().Contain("Validation failed");
    }

    [Fact]
    public void SaveCommand_WithoutUnsavedChanges_CannotExecute()
    {
        // Arrange
        _viewModel.HasUnsavedChanges = false;

        // Act & Assert
        _viewModel.SaveCommand.CanExecute(null).Should().BeFalse();
    }

    private async Task LoadTestIncidentFile(string filePath)
    {
        var testPermissions = new List<IncidentPermission>
        {
            new() { Id = "I001", DisplayName = "Test" }
        };

        _mockFileService
            .Setup(x => x.DetectPermissionTypeAsync(filePath))
            .ReturnsAsync(PermissionType.Incident);

        _mockFileService
            .Setup(x => x.LoadIncidentFileAsync(filePath))
            .ReturnsAsync(testPermissions);

        _mockValidationService
            .Setup(x => x.ValidateIncidentPermissions(It.IsAny<List<IncidentPermission>>()))
            .Returns(new ValidationResult());

        await _viewModel.OpenFileCommand.ExecuteAsync(filePath);
    }
}
