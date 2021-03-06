using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using Vimal.Services;
using System.Diagnostics;
using Vimal.ViewModels;
using Windows.UI.Xaml.Documents;
using Windows.ApplicationModel.AppService;
using Windows.UI.Core;
using Vimal.Models.Languages;

namespace Vimal.Views
{
    /// <summary>
    /// A page containing a script
    /// </summary>
    public sealed partial class ScriptPage : Page
    {
        public ScriptPage(ILanguage language)
        {
            this.InitializeComponent();
            ViewModel = new ScriptViewModel(language, FindName("scriptEditor") as TextBlock);

            if (!wasSet)
            {
                wasSet = true;
                App.AppServiceDisconnected += AppServiceDisconnected;
                App.AppServiceConnected += AppServiceConnected;
            }
        }

        private static bool wasSet = false;
        public ScriptViewModel ViewModel { get; }
        private const int ScrollLoopbackTimeout = 500;
        private object _lastScrollingElement;
        private int _lastScrollChange = Environment.TickCount;


        /// <summary>
        /// Handle script output from process
        /// </summary>
        private async void AppServiceConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            // Log script output
            if (args.Request.Message.ContainsKey("LINE"))
            {
                string data = (string)args.Request.Message["LINE"];

                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    SyntaxHighlightingService.Highlight(data, outputTextBlock, new OutputLanguage());
                    outputTextBlock.Inlines.Add(new LineBreak());
                    
                }).AsTask();
            }
            //Loop has finished
            if (args.Request.Message.ContainsKey("REP"))
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    int currentRep = Int32.Parse(args.Request.Message["REP"] as string);

                    ComputeAndDisplayEstimatedTimeRemaining(currentRep);

                }).AsTask();
            }
            //Script has finished
            if (args.Request.Message.ContainsKey("CODE"))
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    ViewModel.ReturnCode = "Return code: " + (string)args.Request.Message["CODE"];
                    ViewModel.IsBusy = false;
                    if ((string)args.Request.Message["CODE"] == "0")
                    {
                        ViewModel.ProgressText = "Done";
                    }
                    else
                    {
                        ViewModel.ProgressText = "Error";
                    }
                }).AsTask();
            }
        }

        private void ComputeAndDisplayEstimatedTimeRemaining(int currentRep)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan remainingTime = DateTime.Now - ViewModel.ScriptStartTime;
            remainingTime *= (ViewModel.Repetitions - currentRep);
            ViewModel.ProgressText = $"Left: {remainingTime.Hours}h {remainingTime.Minutes}m {remainingTime.Seconds}s";
            ViewModel.ScriptStartTime = currentTime;
        }

        /// <summary>
        /// When the desktop process is connected, get ready to send/receive requests
        /// </summary>
        private void AppServiceConnected(object sender, AppServiceTriggerDetails e)
        {
            App.Connection.RequestReceived += AppServiceConnection_RequestReceived;
        }

        /// <summary>
        /// When the desktop process is disconnected
        /// </summary> 
        private void AppServiceDisconnected(object sender, EventArgs e) { }

        /// <summary>
        /// Open file button handler
        /// </summary>
        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a text file.
            FileOpenPicker open = new FileOpenPicker();
            open.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            open.FileTypeFilter.Add(".kts");

            StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                using (IRandomAccessStream randAccStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    // Load the file into the Script property
                    IBuffer buffer = new Windows.Storage.Streams.Buffer((uint)randAccStream.Size);
                    await randAccStream.ReadAsync(buffer, (uint)randAccStream.Size, InputStreamOptions.None);
                    ViewModel.Script = System.Text.Encoding.Default.GetString(buffer.ToArray());
                }
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Scripts", new List<string>() { ".swift", ".kts" });

            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "script";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until we
                // finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);
                // write to file
                using (IRandomAccessStream randAccStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await randAccStream.WriteAsync(CryptographicBuffer.ConvertStringToBinary(ViewModel.Script, BinaryStringEncoding.Utf8));
                }

                // Let Windows know that we're finished changing the file so the
                // other app can update the remote version of the file.
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status != FileUpdateStatus.Complete)
                {
                    Windows.UI.Popups.MessageDialog errorBox =
                        new Windows.UI.Popups.MessageDialog("File " + file.Name + " couldn't be saved.");
                    await errorBox.ShowAsync();
                }
            }
        }

        private async void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            await DisplayService.ShowSettingsAsync();
        }

        private void SynchronizedScrollerOnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (_lastScrollingElement != sender && Environment.TickCount - _lastScrollChange < ScrollLoopbackTimeout) return;

            _lastScrollingElement = sender;
            _lastScrollChange = Environment.TickCount;

            ScrollViewer sourceScrollViewer;
            ScrollViewer targetScrollViewer;
            if (sender == scrollVieverScriptBlock)
            {
                sourceScrollViewer = scrollVieverScriptBlock;
                targetScrollViewer = scrollVieverScriptBox;
            }
            else
            {
                sourceScrollViewer = scrollVieverScriptBox;
                targetScrollViewer = scrollVieverScriptBlock;
            }

            targetScrollViewer.ChangeView(null, sourceScrollViewer.VerticalOffset, null);
        }
    }
}
