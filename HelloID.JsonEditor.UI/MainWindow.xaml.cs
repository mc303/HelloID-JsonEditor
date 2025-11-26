using System.Windows;
using Microsoft.Win32;
using HelloID.JsonEditor.UI.ViewModels;

namespace HelloID.JsonEditor.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        DataContext = _viewModel;

        // Wire up file dialog handlers
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        // Subscribe to commands that need UI dialogs
        // These will be implemented when we add menu items
    }

    private void NewIncidentFile_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.NewIncidentFileCommand.Execute(null);
    }

    private void NewChangeFile_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.NewChangeFileCommand.Execute(null);
    }

    private async void OpenFile_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "JSON Files (*.json)|*.json|Incident Files (*.incident.json)|*.incident.json|Change Files (*.change.json)|*.change.json|All Files (*.*)|*.*",
            Title = "Open Permissions File"
        };

        if (dialog.ShowDialog() == true)
        {
            await _viewModel.OpenFileCommand.ExecuteAsync(dialog.FileName);
        }
    }

    private void CloseFile_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.CloseFileCommand.Execute(null);
    }

    private async void SaveFile_Click(object sender, RoutedEventArgs e)
    {
        // If no file path exists, show Save As dialog
        if (string.IsNullOrEmpty(_viewModel.CurrentFilePath))
        {
            SaveFileAs_Click(sender, e);
            return;
        }

        await _viewModel.SaveCommand.ExecuteAsync(null);
    }

    private async void SaveFileAs_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            Filter = "JSON Files (*.json)|*.json|Incident Files (*.incident.json)|*.incident.json|Change Files (*.change.json)|*.change.json|All Files (*.*)|*.*",
            Title = "Save Permissions File",
            FileName = _viewModel.CurrentFileName ?? "permissions.json"
        };

        if (dialog.ShowDialog() == true)
        {
            await _viewModel.SaveAsAsync(dialog.FileName);
        }
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void AddIncident_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.AddNewIncidentCommand.Execute(null);
    }

    private void AddChange_Click(object sender, RoutedEventArgs e)
    {
        _viewModel.AddNewChangeCommand.Execute(null);
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
        if (_viewModel.DeleteCommand.CanExecute(null))
        {
            _viewModel.DeleteCommand.Execute(null);
        }
    }

    private void Duplicate_Click(object sender, RoutedEventArgs e)
    {
        if (_viewModel.DuplicateCommand.CanExecute(null))
        {
            _viewModel.DuplicateCommand.Execute(null);
        }
    }
}