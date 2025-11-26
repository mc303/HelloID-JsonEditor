using FluentAssertions;
using HelloID.JsonEditor.Models;
using HelloID.JsonEditor.Services;

namespace HelloID.JsonEditor.Tests.Services;

public class ValidationServiceTests
{
    private readonly ValidationService _validationService;

    public ValidationServiceTests()
    {
        _validationService = new ValidationService();
    }

    #region IncidentPermission Tests

    [Fact]
    public void ValidateIncidentPermission_ValidPermission_ReturnsValid()
    {
        // Arrange
        var permission = new IncidentPermission
        {
            Id = "I001",
            DisplayName = "Test Permission",
            Grant = new IncidentSection
            {
                Caller = "test@test.com",
                RequestShort = "Test request"
            }
        };

        // Act
        var result = _validationService.ValidateIncidentPermission(permission);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ValidateIncidentPermission_NullPermission_ReturnsInvalid()
    {
        // Act
        var result = _validationService.ValidateIncidentPermission(null!);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Permission cannot be null");
    }

    [Fact]
    public void ValidateIncidentPermission_EmptyId_ReturnsInvalid()
    {
        // Arrange
        var permission = new IncidentPermission
        {
            Id = "",
            DisplayName = "Test Permission",
            Grant = new IncidentSection { Caller = "test@test.com" }
        };

        // Act
        var result = _validationService.ValidateIncidentPermission(permission);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Id is required and cannot be empty");
    }

    [Fact]
    public void ValidateIncidentPermission_EmptyDisplayName_ReturnsInvalid()
    {
        // Arrange
        var permission = new IncidentPermission
        {
            Id = "I001",
            DisplayName = "",
            Grant = new IncidentSection { Caller = "test@test.com" }
        };

        // Act
        var result = _validationService.ValidateIncidentPermission(permission);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("DisplayName is required and cannot be empty");
    }

    [Fact]
    public void ValidateIncidentPermission_BothSectionsEmpty_ReturnsInvalid()
    {
        // Arrange
        var permission = new IncidentPermission
        {
            Id = "I001",
            DisplayName = "Test Permission",
            Grant = new IncidentSection(),
            Revoke = new IncidentSection()
        };

        // Act
        var result = _validationService.ValidateIncidentPermission(permission);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("At least one section (Grant or Revoke) must have content");
    }

    [Fact]
    public void ValidateIncidentPermission_OnlyGrantHasContent_ReturnsValid()
    {
        // Arrange
        var permission = new IncidentPermission
        {
            Id = "I001",
            DisplayName = "Test Permission",
            Grant = new IncidentSection { Caller = "test@test.com" },
            Revoke = new IncidentSection()
        };

        // Act
        var result = _validationService.ValidateIncidentPermission(permission);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateIncidentPermission_OnlyRevokeHasContent_ReturnsValid()
    {
        // Arrange
        var permission = new IncidentPermission
        {
            Id = "I001",
            DisplayName = "Test Permission",
            Grant = new IncidentSection(),
            Revoke = new IncidentSection { Caller = "test@test.com" }
        };

        // Act
        var result = _validationService.ValidateIncidentPermission(permission);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateIncidentPermission_MultipleErrors_ReturnsAllErrors()
    {
        // Arrange
        var permission = new IncidentPermission
        {
            Id = "",
            DisplayName = "",
            Grant = new IncidentSection(),
            Revoke = new IncidentSection()
        };

        // Act
        var result = _validationService.ValidateIncidentPermission(permission);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
        result.Errors.Should().Contain("Id is required and cannot be empty");
        result.Errors.Should().Contain("DisplayName is required and cannot be empty");
        result.Errors.Should().Contain("At least one section (Grant or Revoke) must have content");
    }

    #endregion

    #region ChangePermission Tests

    [Fact]
    public void ValidateChangePermission_ValidPermission_ReturnsValid()
    {
        // Arrange
        var permission = new ChangePermission
        {
            Id = "C001",
            DisplayName = "Test Permission",
            Grant = new ChangeSection
            {
                Requester = "test@test.com",
                Request = "Test request"
            }
        };

        // Act
        var result = _validationService.ValidateChangePermission(permission);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ValidateChangePermission_NullPermission_ReturnsInvalid()
    {
        // Act
        var result = _validationService.ValidateChangePermission(null!);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Permission cannot be null");
    }

    [Fact]
    public void ValidateChangePermission_EmptyId_ReturnsInvalid()
    {
        // Arrange
        var permission = new ChangePermission
        {
            Id = "",
            DisplayName = "Test Permission",
            Grant = new ChangeSection { Requester = "test@test.com" }
        };

        // Act
        var result = _validationService.ValidateChangePermission(permission);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Id is required and cannot be empty");
    }

    [Fact]
    public void ValidateChangePermission_BothSectionsEmpty_ReturnsInvalid()
    {
        // Arrange
        var permission = new ChangePermission
        {
            Id = "C001",
            DisplayName = "Test Permission",
            Grant = new ChangeSection(),
            Revoke = new ChangeSection()
        };

        // Act
        var result = _validationService.ValidateChangePermission(permission);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("At least one section (Grant or Revoke) must have content");
    }

    #endregion

    #region ValidateUniqueIds Tests

    [Fact]
    public void ValidateUniqueIds_AllIdsUnique_ReturnsValid()
    {
        // Arrange
        var permissions = new List<IncidentPermission>
        {
            new() { Id = "I001", DisplayName = "Test 1" },
            new() { Id = "I002", DisplayName = "Test 2" },
            new() { Id = "I003", DisplayName = "Test 3" }
        };

        // Act
        var result = _validationService.ValidateUniqueIds(permissions, p => p.Id);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ValidateUniqueIds_DuplicateIds_ReturnsInvalid()
    {
        // Arrange
        var permissions = new List<IncidentPermission>
        {
            new() { Id = "I001", DisplayName = "Test 1" },
            new() { Id = "I002", DisplayName = "Test 2" },
            new() { Id = "I001", DisplayName = "Test 3" } // Duplicate
        };

        // Act
        var result = _validationService.ValidateUniqueIds(permissions, p => p.Id);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("Duplicate Id found: 'I001'"));
    }

    [Fact]
    public void ValidateUniqueIds_EmptyId_ReturnsInvalid()
    {
        // Arrange
        var permissions = new List<IncidentPermission>
        {
            new() { Id = "I001", DisplayName = "Test 1" },
            new() { Id = "", DisplayName = "Test 2" }
        };

        // Act
        var result = _validationService.ValidateUniqueIds(permissions, p => p.Id);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("empty or null Id"));
    }

    [Fact]
    public void ValidateUniqueIds_EmptyList_ReturnsValid()
    {
        // Arrange
        var permissions = new List<IncidentPermission>();

        // Act
        var result = _validationService.ValidateUniqueIds(permissions, p => p.Id);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    #endregion

    #region ValidateIncidentPermissions Tests

    [Fact]
    public void ValidateIncidentPermissions_ValidList_ReturnsValid()
    {
        // Arrange
        var permissions = new List<IncidentPermission>
        {
            new()
            {
                Id = "I001",
                DisplayName = "Test 1",
                Grant = new IncidentSection { Caller = "test@test.com" }
            },
            new()
            {
                Id = "I002",
                DisplayName = "Test 2",
                Revoke = new IncidentSection { Caller = "test@test.com" }
            }
        };

        // Act
        var result = _validationService.ValidateIncidentPermissions(permissions);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ValidateIncidentPermissions_NullList_ReturnsInvalid()
    {
        // Act
        var result = _validationService.ValidateIncidentPermissions(null!);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Permissions list cannot be null");
    }

    [Fact]
    public void ValidateIncidentPermissions_EmptyList_ReturnsInvalid()
    {
        // Arrange
        var permissions = new List<IncidentPermission>();

        // Act
        var result = _validationService.ValidateIncidentPermissions(permissions);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Permissions list is empty");
    }

    [Fact]
    public void ValidateIncidentPermissions_InvalidPermission_ReturnsInvalid()
    {
        // Arrange
        var permissions = new List<IncidentPermission>
        {
            new()
            {
                Id = "",
                DisplayName = "Test 1",
                Grant = new IncidentSection { Caller = "test@test.com" }
            }
        };

        // Act
        var result = _validationService.ValidateIncidentPermissions(permissions);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("Permission #1"));
        result.Errors.Should().Contain(e => e.Contains("Id is required"));
    }

    [Fact]
    public void ValidateIncidentPermissions_DuplicateIds_ReturnsInvalid()
    {
        // Arrange
        var permissions = new List<IncidentPermission>
        {
            new()
            {
                Id = "I001",
                DisplayName = "Test 1",
                Grant = new IncidentSection { Caller = "test@test.com" }
            },
            new()
            {
                Id = "I001",
                DisplayName = "Test 2",
                Grant = new IncidentSection { Caller = "test@test.com" }
            }
        };

        // Act
        var result = _validationService.ValidateIncidentPermissions(permissions);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("Duplicate Id found: 'I001'"));
    }

    #endregion

    #region ValidateChangePermissions Tests

    [Fact]
    public void ValidateChangePermissions_ValidList_ReturnsValid()
    {
        // Arrange
        var permissions = new List<ChangePermission>
        {
            new()
            {
                Id = "C001",
                DisplayName = "Test 1",
                Grant = new ChangeSection { Requester = "test@test.com" }
            },
            new()
            {
                Id = "C002",
                DisplayName = "Test 2",
                Revoke = new ChangeSection { Requester = "test@test.com" }
            }
        };

        // Act
        var result = _validationService.ValidateChangePermissions(permissions);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ValidateChangePermissions_NullList_ReturnsInvalid()
    {
        // Act
        var result = _validationService.ValidateChangePermissions(null!);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Permissions list cannot be null");
    }

    [Fact]
    public void ValidateChangePermissions_EmptyList_ReturnsInvalid()
    {
        // Arrange
        var permissions = new List<ChangePermission>();

        // Act
        var result = _validationService.ValidateChangePermissions(permissions);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain("Permissions list is empty");
    }

    #endregion
}
