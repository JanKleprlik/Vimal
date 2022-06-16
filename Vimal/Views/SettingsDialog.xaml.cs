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
