using System;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp.Helpers;

using Vimal.Views;

using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;

namespace Vimal.Services
{
    public static class DisplayService
    {
        internal static async Task ShowSettingsAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    var currPath = ApplicationData.Current.LocalSettings.Values["compilerPath"] as string ?? @"C:\Program Files\Kotlin\kotlinc\bin";
                    var dialog = new SettingsDialog(currPath);
                    await dialog.ShowAsync();
                });
        }
    }
}
