using System;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp.Helpers;

using Vimal.Views;

using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;

namespace Vimal.Services
{
    public static class FirstRunDisplayService
    {
        private static bool shown = false;

        internal static async Task ShowIfAppropriateAsync()
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    if (SystemInformation.Instance.IsFirstRun && !shown)
                    {
                        shown = true;
                    }

                    var currPath = ApplicationData.Current.LocalSettings.Values["compilerPath"] as string ?? @"C:\Program Files\Kotlin\kotlinc\bin";
                    var dialog = new FirstRunDialog(currPath);
                    await dialog.ShowAsync();
                });
        }
    }
}
