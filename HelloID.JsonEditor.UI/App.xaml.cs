using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using HelloID.JsonEditor.Services;
using HelloID.JsonEditor.UI.ViewModels;

namespace HelloID.JsonEditor.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Configure dependency injection
        var services = new ServiceCollection();
        ConfigureServices(services);
        _serviceProvider = services.BuildServiceProvider();

        // Create and show main window
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private void ConfigureServices(ServiceCollection services)
    {
        // Register services
        services.AddSingleton<IFileService, JsonFileService>();
        services.AddSingleton<IValidationService, ValidationService>();

        // Register ViewModels
        services.AddSingleton<MainViewModel>();
        services.AddTransient<IncidentEditorViewModel>();
        services.AddTransient<ChangeEditorViewModel>();

        // Register MainWindow
        services.AddSingleton<MainWindow>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _serviceProvider?.Dispose();
        base.OnExit(e);
    }
}

