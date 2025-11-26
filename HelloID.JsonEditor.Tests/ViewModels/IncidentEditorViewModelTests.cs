using FluentAssertions;
using HelloID.JsonEditor.Models;
using HelloID.JsonEditor.UI.ViewModels;

namespace HelloID.JsonEditor.Tests.ViewModels;

public class IncidentEditorViewModelTests
{
    [Fact]
    public void Permission_WhenSet_LoadsAllProperties()
    {
        // Arrange
        var viewModel = new IncidentEditorViewModel();
        var permission = new IncidentPermission
        {
            Id = "I001",
            DisplayName = "Test Permission",
            Grant = new IncidentSection
            {
                Caller = "test@test.com",
                RequestShort = "Test Request",
                Branch = "Baarn"
            },
            Revoke = new IncidentSection
            {
                Caller = "revoke@test.com",
                Status = "firstLine"
            }
        };

        // Act
        viewModel.Permission = permission;

        // Assert
        viewModel.Id.Should().Be("I001");
        viewModel.DisplayName.Should().Be("Test Permission");
        viewModel.GrantCaller.Should().Be("test@test.com");
        viewModel.GrantRequestShort.Should().Be("Test Request");
        viewModel.GrantBranch.Should().Be("Baarn");
        viewModel.RevokeCaller.Should().Be("revoke@test.com");
        viewModel.RevokeStatus.Should().Be("firstLine");
    }

    [Fact]
    public void Id_WhenChanged_UpdatesPermission()
    {
        // Arrange
        var viewModel = new IncidentEditorViewModel();
        var permission = new IncidentPermission { Id = "I001" };
        viewModel.Permission = permission;

        // Act
        viewModel.Id = "I002";

        // Assert
        permission.Id.Should().Be("I002");
    }

    [Fact]
    public void DisplayName_WhenChanged_UpdatesPermission()
    {
        // Arrange
        var viewModel = new IncidentEditorViewModel();
        var permission = new IncidentPermission { DisplayName = "Old Name" };
        viewModel.Permission = permission;

        // Act
        viewModel.DisplayName = "New Name";

        // Assert
        permission.DisplayName.Should().Be("New Name");
    }

    [Fact]
    public void GrantCaller_WhenChanged_UpdatesPermission()
    {
        // Arrange
        var viewModel = new IncidentEditorViewModel();
        var permission = new IncidentPermission
        {
            Grant = new IncidentSection { Caller = "old@test.com" }
        };
        viewModel.Permission = permission;

        // Act
        viewModel.GrantCaller = "new@test.com";

        // Assert
        permission.Grant.Caller.Should().Be("new@test.com");
    }

    [Fact]
    public void GrantEnableGetAssets_WhenChanged_UpdatesPermission()
    {
        // Arrange
        var viewModel = new IncidentEditorViewModel();
        var permission = new IncidentPermission
        {
            Grant = new IncidentSection { EnableGetAssets = false }
        };
        viewModel.Permission = permission;

        // Act
        viewModel.GrantEnableGetAssets = true;

        // Assert
        permission.Grant.EnableGetAssets.Should().BeTrue();
    }

    [Fact]
    public void Permission_WhenSetToNull_ClearsAllProperties()
    {
        // Arrange
        var viewModel = new IncidentEditorViewModel();
        var permission = new IncidentPermission
        {
            Id = "I001",
            DisplayName = "Test",
            Grant = new IncidentSection { Caller = "test@test.com" }
        };
        viewModel.Permission = permission;

        // Act
        viewModel.Permission = null;

        // Assert
        viewModel.Id.Should().BeEmpty();
        viewModel.DisplayName.Should().BeEmpty();
        viewModel.GrantCaller.Should().BeNull();
    }

    [Fact]
    public void OnModified_WhenPropertyChanged_InvokesCallback()
    {
        // Arrange
        var callbackInvoked = false;
        var viewModel = new IncidentEditorViewModel(() => callbackInvoked = true);
        var permission = new IncidentPermission
        {
            Id = "I001",
            DisplayName = "Test",
            Grant = new IncidentSection()
        };
        viewModel.Permission = permission;

        // Act
        viewModel.DisplayName = "New Name";

        // Assert
        callbackInvoked.Should().BeTrue();
    }

    [Fact]
    public void RevokeProperties_WhenChanged_UpdatePermission()
    {
        // Arrange
        var viewModel = new IncidentEditorViewModel();
        var permission = new IncidentPermission
        {
            Revoke = new IncidentSection
            {
                Caller = "old@test.com",
                Priority = "P1"
            }
        };
        viewModel.Permission = permission;

        // Act
        viewModel.RevokeCaller = "new@test.com";
        viewModel.RevokePriority = "P2";

        // Assert
        permission.Revoke.Caller.Should().Be("new@test.com");
        permission.Revoke.Priority.Should().Be("P2");
    }

    [Fact]
    public void BooleanProperties_WhenToggled_UpdatePermission()
    {
        // Arrange
        var viewModel = new IncidentEditorViewModel();
        var permission = new IncidentPermission
        {
            Grant = new IncidentSection { EnableGetAssets = false, SkipNoAssetsFound = false },
            Revoke = new IncidentSection { EnableGetAssets = false, SkipNoAssetsFound = false }
        };
        viewModel.Permission = permission;

        // Act
        viewModel.GrantEnableGetAssets = true;
        viewModel.GrantSkipNoAssetsFound = true;
        viewModel.RevokeEnableGetAssets = true;
        viewModel.RevokeSkipNoAssetsFound = true;

        // Assert
        permission.Grant.EnableGetAssets.Should().BeTrue();
        permission.Grant.SkipNoAssetsFound.Should().BeTrue();
        permission.Revoke.EnableGetAssets.Should().BeTrue();
        permission.Revoke.SkipNoAssetsFound.Should().BeTrue();
    }
}
