using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HelloID.JsonEditor.UI.Views;

namespace HelloID.JsonEditor.UI.Behaviors;

/// <summary>
/// Attached behavior that enables double-click to open a larger text editor popup
/// </summary>
public static class TextBoxEditorBehavior
{
    public static readonly DependencyProperty EnablePopupEditorProperty =
        DependencyProperty.RegisterAttached(
            "EnablePopupEditor",
            typeof(bool),
            typeof(TextBoxEditorBehavior),
            new PropertyMetadata(false, OnEnablePopupEditorChanged));

    public static bool GetEnablePopupEditor(DependencyObject obj)
    {
        return (bool)obj.GetValue(EnablePopupEditorProperty);
    }

    public static void SetEnablePopupEditor(DependencyObject obj, bool value)
    {
        obj.SetValue(EnablePopupEditorProperty, value);
    }

    private static void OnEnablePopupEditorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TextBox textBox)
        {
            if ((bool)e.NewValue)
            {
                textBox.MouseDoubleClick += TextBox_MouseDoubleClick;
            }
            else
            {
                textBox.MouseDoubleClick -= TextBox_MouseDoubleClick;
            }
        }
    }

    private static void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            // Get the field name from the binding path or use a default
            string fieldName = "Text";
            if (textBox.GetBindingExpression(TextBox.TextProperty)?.ParentBinding?.Path?.Path is string path)
            {
                fieldName = path;
            }

            // Open the text editor dialog
            var dialog = new TextEditorDialog(textBox.Text, fieldName)
            {
                Owner = Window.GetWindow(textBox)
            };

            if (dialog.ShowDialog() == true)
            {
                textBox.Text = dialog.EditedText;
            }

            e.Handled = true;
        }
    }
}
