using FluentAssertions;
using HelloID.JsonEditor.Models;
using HelloID.JsonEditor.UI.ViewModels;

namespace HelloID.JsonEditor.Tests.ViewModels;

public class ChangeEditorViewModelTests
{
    [Fact]
    public void Permission_WhenSet_LoadsAllProperties()
    {
        // Arrange
        var viewModel = new ChangeEditorViewModel();
        var permission = new ChangePermission
        {
            Id = "C001",
            DisplayName = "Test Permission",
            Grant = new ChangeSection
            {
                Requester = "test@test.com",
                Request = "Test Request",
                Template = "Ws 006"
            },
            Revoke = new ChangeSection
            {
                Requester = "revoke@test.com",
                ChangeType = "Simple"
            }
        };

        // Act
        viewModel.Permission = permission;

        // Assert
        viewModel.Id.Should().Be("C001");
        viewModel.DisplayName.Should().Be("Test Permission");
        viewModel.GrantRequester.Should().Be("test@test.com");
        viewModel.GrantRequest.Should().Be("Test Request");
        viewModel.GrantTemplate.Should().Be("Ws 006");
        viewModel.RevokeRequester.Should().Be("revoke@test.com");
        viewModel.RevokeChangeType.Should().Be("Simple");
    }

    [Fact]
    public void Id_WhenChanged_UpdatesPermission()
    {
        // Arrange
        var viewModel = new ChangeEditorViewModel();
        var permission = new ChangePermission { Id = "C001" };
        viewModel.Permission = permission;

        // Act
        viewModel.Id = "C002";

        // Assert
        permission.Id.Should().Be("C002");
    }

    [Fact]
    public void DisplayName_WhenChanged_UpdatesPermission()
    {
        // Arrange
        var viewModel = new ChangeEditorViewModel();
        var permission = new ChangePermission { DisplayName = "Old Name" };
        viewModel.Permission = permission;

        // Act
        viewModel.DisplayName = "New Name";

        // Assert
        permission.DisplayName.Should().Be("New Name");
    }

    [Fact]
    public void GrantRequester_WhenChanged_UpdatesPermission()
    {
        // Arrange
        var viewModel = new ChangeEditorViewModel();
        var permission = new ChangePermission
        {
            Grant = new ChangeSection { Requester = "old@test.com" }
        };
        viewModel.Permission = permission;

        // Act
        viewModel.GrantRequester = "new@test.com";

        // Assert
        permission.Grant.Requester.Should().Be("new@test.com");
    }

    [Fact]
    public void GrantEnableGetAssets_WhenChanged_UpdatesPermission()
    {
        // Arrange
        var viewModel = new ChangeEditorViewModel();
        var permission = new ChangePermission
        {
            Grant = new ChangeSection { EnableGetAssets = false }
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
        var viewModel = new ChangeEditorViewModel();
        var permission = new ChangePermission
        {
            Id = "C001",
            DisplayName = "Test",
            Grant = new ChangeSection { Requester = "test@test.com" }
        };
        viewModel.Permission = permission;

        // Act
        viewModel.Permission = null;

        // Assert
        viewModel.Id.Should().BeEmpty();
        viewModel.DisplayName.Should().BeEmpty();
        viewModel.GrantRequester.Should().BeNull();
    }

    [Fact]
    public void OnModified_WhenPropertyChanged_InvokesCallback()
    {
        // Arrange
        var callbackInvoked = false;
        var viewModel = new ChangeEditorViewModel(() => callbackInvoked = true);
        var permission = new ChangePermission
        {
            Id = "C001",
            DisplayName = "Test",
            Grant = new ChangeSection()
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
        var viewModel = new ChangeEditorViewModel();
        var permission = new ChangePermission
        {
            Revoke = new ChangeSection
            {
                Requester = "old@test.com",
                Priority = "P1"
            }
        };
        viewModel.Permission = permission;

        // Act
        viewModel.RevokeRequester = "new@test.com";
        viewModel.RevokePriority = "P2";

        // Assert
        permission.Revoke.Requester.Should().Be("new@test.com");
        permission.Revoke.Priority.Should().Be("P2");
    }

    [Fact]
    public void BooleanProperties_WhenToggled_UpdatePermission()
    {
        // Arrange
        var viewModel = new ChangeEditorViewModel();
        var permission = new ChangePermission
        {
            Grant = new ChangeSection { EnableGetAssets = false, SkipNoAssetsFound = false },
            Revoke = new ChangeSection { EnableGetAssets = false, SkipNoAssetsFound = false }
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

    [Fact]
    public void ChangeSpecificProperties_WhenChanged_UpdatePermission()
    {
        // Arrange
        var viewModel = new ChangeEditorViewModel();
        var permission = new ChangePermission
        {
            Grant = new ChangeSection
            {
                Template = "Old Template",
                ChangeType = "Simple",
                BriefDescription = "Old Description"
            }
        };
        viewModel.Permission = permission;

        // Act
        viewModel.GrantTemplate = "New Template";
        viewModel.GrantChangeType = "Extensive";
        viewModel.GrantBriefDescription = "New Description";

        // Assert
        permission.Grant.Template.Should().Be("New Template");
        permission.Grant.ChangeType.Should().Be("Extensive");
        permission.Grant.BriefDescription.Should().Be("New Description");
    }
}
