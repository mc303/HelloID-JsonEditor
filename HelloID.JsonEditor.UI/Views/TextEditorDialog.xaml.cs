using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace HelloID.JsonEditor.UI.Views;

/// <summary>
/// Popup text editor dialog for editing long text in a larger window
/// </summary>
public partial class TextEditorDialog : Window
{
    public string EditedText { get; private set; } = string.Empty;
    private List<string> _allVariables = new();

    public TextEditorDialog(string initialText, string fieldName)
    {
        InitializeComponent();

        Title = $"Text Editor - {fieldName}";
        EditorTextBox.Text = initialText ?? string.Empty;
        EditedText = initialText ?? string.Empty;

        // Load variables from replace.list
        LoadVariables();

        // Set up filtering
        VariableComboBox.AddHandler(TextBox.TextChangedEvent, new TextChangedEventHandler(VariableComboBox_TextChanged));

        // Focus the textbox without selecting text
        Loaded += (s, e) =>
        {
            EditorTextBox.Focus();
        };
    }

    private void LoadVariables()
    {
        try
        {
            // Get the application directory and look for replace.list
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var replaceListPath = Path.Combine(appDirectory, "replace.list");

            if (File.Exists(replaceListPath))
            {
                _allVariables = File.ReadAllLines(replaceListPath)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line => line.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase) // Remove duplicates (case-insensitive)
                    .OrderBy(line => line) // Sort alphabetically for easier browsing
                    .ToList();

                VariableComboBox.ItemsSource = _allVariables;
            }
        }
        catch
        {
            // Silently fail if replace.list is not found
        }
    }

    private void VariableComboBox_GotFocus(object sender, RoutedEventArgs e)
    {
        // Get the TextBox inside the ComboBox
        var textBox = VariableComboBox.Template.FindName("PART_EditableTextBox", VariableComboBox) as TextBox;
        if (textBox != null)
        {
            // Move cursor to end and prevent selection
            textBox.SelectionStart = textBox.Text.Length;
            textBox.SelectionLength = 0;
        }
    }

    private void VariableComboBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Get the TextBox inside the ComboBox to manage cursor position
        var textBox = VariableComboBox.Template.FindName("PART_EditableTextBox", VariableComboBox) as TextBox;
        var cursorPosition = textBox?.SelectionStart ?? 0;

        if (VariableComboBox.IsDropDownOpen == false)
        {
            VariableComboBox.IsDropDownOpen = true;
        }

        var searchText = VariableComboBox.Text;

        if (string.IsNullOrWhiteSpace(searchText))
        {
            // Show all variables if search is empty
            VariableComboBox.ItemsSource = _allVariables;
        }
        else
        {
            // Filter variables case-insensitively, matching anywhere in the string
            var filtered = _allVariables
                .Where(v => v.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

            VariableComboBox.ItemsSource = filtered;
        }

        // Restore cursor position after filtering
        if (textBox != null)
        {
            textBox.SelectionStart = cursorPosition;
            textBox.SelectionLength = 0;
        }
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
        EditedText = EditorTextBox.Text;
        DialogResult = true;
        Close();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void VariableComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (VariableComboBox.SelectedItem is string selectedVariable)
        {
            // Wrap the variable and copy to clipboard
            var wrappedVariable = $"$({selectedVariable})";
            Clipboard.SetText(wrappedVariable);
        }
    }

    private void Insert_Click(object sender, RoutedEventArgs e)
    {
        if (VariableComboBox.SelectedItem is string selectedVariable)
        {
            // Wrap the variable
            var wrappedVariable = $"$({selectedVariable})";

            // Get current cursor position
            var cursorPosition = EditorTextBox.SelectionStart;

            // Insert at cursor position
            EditorTextBox.Text = EditorTextBox.Text.Insert(cursorPosition, wrappedVariable);

            // Move cursor to end of inserted text
            EditorTextBox.SelectionStart = cursorPosition + wrappedVariable.Length;
            EditorTextBox.SelectionLength = 0;

            // Focus back to textbox
            EditorTextBox.Focus();
        }
    }
}
