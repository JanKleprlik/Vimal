using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Vimal.Views
{
    public sealed partial class SettingsDialog : ContentDialog
    {
        public SettingsDialog(string currentPath)
        {
            // TODO: Update the contents of this dialog with any important information you want to show when the app is used for the first time.
            RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
            InitializeComponent();
            (FindName("CompilerPathTextBox") as TextBox).Text = currentPath;
        }

        private void SaveCompilerPath(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var path = (FindName("CompilerPathTextBox") as TextBox).Text;
            ApplicationData.Current.LocalSettings.Values["compilerPath"] = path;
        }
    }
}
