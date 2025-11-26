using CommunityToolkit.Mvvm.ComponentModel;

namespace HelloID.JsonEditor.UI.ViewModels;

/// <summary>
/// Base class for all ViewModels in the application
/// Provides common functionality like INotifyPropertyChanged and IsBusy tracking
/// </summary>
public abstract class ViewModelBase : ObservableObject
{
    private bool _isBusy;
    private string _statusMessage = string.Empty;

    /// <summary>
    /// Indicates whether the ViewModel is currently performing an operation
    /// </summary>
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
            {
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }
    }

    /// <summary>
    /// Inverse of IsBusy for binding to UI controls
    /// </summary>
    public bool IsNotBusy => !IsBusy;

    /// <summary>
    /// Status message to display to the user
    /// </summary>
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    /// <summary>
    /// Sets the busy state and optional status message
    /// </summary>
    protected void SetBusy(bool isBusy, string statusMessage = "")
    {
        IsBusy = isBusy;
        StatusMessage = statusMessage;
    }
}
