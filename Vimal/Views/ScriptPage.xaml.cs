﻿using System;
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

namespace Vimal.Views
{
    /// <summary>
    /// A page containing a script
    /// </summary>
    public sealed partial class ScriptPage : Page
    {
        public ScriptViewModel ViewModel { get; }
        private const int ScrollLoopbackTimeout = 500;
        private object _lastScrollingElement;
        private int _lastScrollChange = Environment.TickCount;

        public ScriptPage()
        {

            this.InitializeComponent();
            ViewModel = new ScriptViewModel(FindName("outputTextBlock") as TextBlock, FindName("scriptEditor") as TextBlock);
        }


        #region EditBox

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a text file.
            Windows.Storage.Pickers.FileOpenPicker open =
                new Windows.Storage.Pickers.FileOpenPicker();
            open.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            open.FileTypeFilter.Add(".kts");

            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                using (Windows.Storage.Streams.IRandomAccessStream randAccStream =
                    await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    // Load the file into the Document property of the RichEditBox.
                    IBuffer buffer = new Windows.Storage.Streams.Buffer(
                        (uint)randAccStream.Size);
                    await randAccStream.ReadAsync(buffer,
                        (uint)randAccStream.Size, InputStreamOptions.None);
                    ViewModel.Script = System.Text.Encoding.Default.GetString(buffer.ToArray());
                }
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".txt", ".kts" });

            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Document";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until we
                // finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);
                // write to file
                using (Windows.Storage.Streams.IRandomAccessStream randAccStream =
                    await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                {
                    await randAccStream.WriteAsync(CryptographicBuffer.ConvertStringToBinary(ViewModel.Script, BinaryStringEncoding.Utf8));
                    //editor.Text.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, randAccStream);
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

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(typeof(SettingsPage), ViewModel);
        }

        private void FindBoxHighlightMatches()
        {
            FindBoxRemoveHighlights();

            //Color highlightBackgroundColor = (Color)App.Current.Resources["SystemColorHighlightColor"];
            //Color highlightForegroundColor = (Color)App.Current.Resources["SystemColorHighlightTextColor"];

            //string textToFind = "";// findBox.Text;
            //if (textToFind != null)
            //{
            //    ITextRange searchRange = editor.Document.GetRange(0, 0);
            //    while (searchRange.FindText(textToFind, TextConstants.MaxUnitCount, FindOptions.None) > 0)
            //    {
            //        searchRange.CharacterFormat.BackgroundColor = highlightBackgroundColor;
            //        searchRange.CharacterFormat.ForegroundColor = highlightForegroundColor;
            //    }
            //}
        }

        private void FindBoxRemoveHighlights()
        {
            //ITextRange documentRange = editor.Document.GetRange(0, TextConstants.MaxUnitCount);
            //SolidColorBrush defaultBackground = editor.Background as SolidColorBrush;
            //SolidColorBrush defaultForeground = editor.Foreground as SolidColorBrush;

            //documentRange.CharacterFormat.BackgroundColor = defaultBackground.Color;
            //documentRange.CharacterFormat.ForegroundColor = defaultForeground.Color;
        }

        private void Editor_GotFocus(object sender, RoutedEventArgs e)
        {
            //editor.Document.GetText(TextGetOptions.UseCrlf, out string currentRawText);

            //// reset colors to correct defaults for Focused state
            //ITextRange documentRange = editor.Document.GetRange(0, TextConstants.MaxUnitCount);
            //SolidColorBrush background = (SolidColorBrush)App.Current.Resources["TextControlBackgroundFocused"];

            //if (background != null)
            //{
            //    documentRange.CharacterFormat.BackgroundColor = background.Color;
            //}
        }

        private void Editor_TextChanged(object sender, RoutedEventArgs e)
        {
            //editor.Document.Selection.CharacterFormat.ForegroundColor = currentColor;
        }

        #endregion

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
