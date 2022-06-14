﻿using Microsoft.UI.Xaml.Controls;
using System;
using Vimal.Models;
using Vimal.ViewModels;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Vimal.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            
            coreTitleBar.ExtendViewIntoTitleBar = true;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            Window.Current.SetTitleBar(CustomDragRegion);
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (FlowDirection == FlowDirection.LeftToRight)
            {
                CustomDragRegion.MinWidth = sender.SystemOverlayRightInset;
                ShellTitlebarInset.MinWidth = sender.SystemOverlayLeftInset;
            }
            else
            {
                CustomDragRegion.MinWidth = sender.SystemOverlayLeftInset;
                ShellTitlebarInset.MinWidth = sender.SystemOverlayRightInset;
            }

            CustomDragRegion.Height = ShellTitlebarInset.Height = sender.Height;
        }

        #region Keyboard navigation
        private void NewTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var senderTabView = args.Element as TabView;
            ViewModel.AddTabCommand.Execute(null);
            args.Handled = true;
        }

        private void CloseSelectedTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var InvokedTabView = (args.Element as TabView);
            ViewModel.CloseTabCommandByIdx.Execute(InvokedTabView.SelectedIndex);

            args.Handled = true;
        }

        private void NavigateToNumberedTabKeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var InvokedTabView = (args.Element as TabView);

            int tabToSelect = 0;

            switch (sender.Key)
            {
                case Windows.System.VirtualKey.Number1:
                    tabToSelect = 0;
                    break;
                case Windows.System.VirtualKey.Number2:
                    tabToSelect = 1;
                    break;
                case Windows.System.VirtualKey.Number3:
                    tabToSelect = 2;
                    break;
                case Windows.System.VirtualKey.Number4:
                    tabToSelect = 3;
                    break;
                case Windows.System.VirtualKey.Number5:
                    tabToSelect = 4;
                    break;
                case Windows.System.VirtualKey.Number6:
                    tabToSelect = 5;
                    break;
                case Windows.System.VirtualKey.Number7:
                    tabToSelect = 6;
                    break;
                case Windows.System.VirtualKey.Number8:
                    tabToSelect = 7;
                    break;
                case Windows.System.VirtualKey.Number9:
                    // Select the last tab
                    tabToSelect = InvokedTabView.TabItems.Count - 1;
                    break;
            }

            // Only select the tab if it is in the list
            if (tabToSelect < InvokedTabView.TabItems.Count)
            {
                InvokedTabView.SelectedIndex = tabToSelect;
            }
            args.Handled = true;
        }
        #endregion

        //#region EditBox

        //private async void OpenButton_Click(object sender, RoutedEventArgs e)
        //{
        //    // Open a text file.
        //    Windows.Storage.Pickers.FileOpenPicker open =
        //        new Windows.Storage.Pickers.FileOpenPicker();
        //    open.SuggestedStartLocation =
        //        Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
        //    open.FileTypeFilter.Add(".rtf");

        //    Windows.Storage.StorageFile file = await open.PickSingleFileAsync();

        //    if (file != null)
        //    {
        //        using (Windows.Storage.Streams.IRandomAccessStream randAccStream =
        //            await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
        //        {
        //            // Load the file into the Document property of the RichEditBox.
        //            editor.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, randAccStream);
        //        }
        //    }
        //}

        //private async void SaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    FileSavePicker savePicker = new FileSavePicker();
        //    savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

        //    // Dropdown of file types the user can save the file as
        //    savePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });

        //    // Default file name if the user does not type one in or select a file to replace
        //    savePicker.SuggestedFileName = "New Document";

        //    StorageFile file = await savePicker.PickSaveFileAsync();
        //    if (file != null)
        //    {
        //        // Prevent updates to the remote version of the file until we
        //        // finish making changes and call CompleteUpdatesAsync.
        //        CachedFileManager.DeferUpdates(file);
        //        // write to file
        //        using (Windows.Storage.Streams.IRandomAccessStream randAccStream =
        //            await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
        //        {
        //            editor.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, randAccStream);
        //        }

        //        // Let Windows know that we're finished changing the file so the
        //        // other app can update the remote version of the file.
        //        FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
        //        if (status != FileUpdateStatus.Complete)
        //        {
        //            Windows.UI.Popups.MessageDialog errorBox =
        //                new Windows.UI.Popups.MessageDialog("File " + file.Name + " couldn't be saved.");
        //            await errorBox.ShowAsync();
        //        }
        //    }
        //}

        //private void BoldButton_Click(object sender, RoutedEventArgs e)
        //{
        //    editor.Document.Selection.CharacterFormat.Bold = FormatEffect.Toggle;
        //}

        //private void ItalicButton_Click(object sender, RoutedEventArgs e)
        //{
        //    editor.Document.Selection.CharacterFormat.Italic = FormatEffect.Toggle;
        //}

        //private void ColorButton_Click(object sender, RoutedEventArgs e)
        //{
        //    // Extract the color of the button that was clicked.
        //    Button clickedColor = (Button)sender;
        //    var rectangle = (Windows.UI.Xaml.Shapes.Rectangle)clickedColor.Content;
        //    var color = ((Windows.UI.Xaml.Media.SolidColorBrush)rectangle.Fill).Color;

        //    editor.Document.Selection.CharacterFormat.ForegroundColor = color;

        //    fontColorButton.Flyout.Hide();
        //    editor.Focus(Windows.UI.Xaml.FocusState.Keyboard);
        //}

        //private void FindBoxHighlightMatches()
        //{
        //    FindBoxRemoveHighlights();

        //    Color highlightBackgroundColor = (Color)App.Current.Resources["SystemColorHighlightColor"];
        //    Color highlightForegroundColor = (Color)App.Current.Resources["SystemColorHighlightTextColor"];

        //    string textToFind = findBox.Text;
        //    if (textToFind != null)
        //    {
        //        ITextRange searchRange = editor.Document.GetRange(0, 0);
        //        while (searchRange.FindText(textToFind, TextConstants.MaxUnitCount, FindOptions.None) > 0)
        //        {
        //            searchRange.CharacterFormat.BackgroundColor = highlightBackgroundColor;
        //            searchRange.CharacterFormat.ForegroundColor = highlightForegroundColor;
        //        }
        //    }
        //}

        //private void FindBoxRemoveHighlights()
        //{
        //    ITextRange documentRange = editor.Document.GetRange(0, TextConstants.MaxUnitCount);
        //    SolidColorBrush defaultBackground = editor.Background as SolidColorBrush;
        //    SolidColorBrush defaultForeground = editor.Foreground as SolidColorBrush;

        //    documentRange.CharacterFormat.BackgroundColor = defaultBackground.Color;
        //    documentRange.CharacterFormat.ForegroundColor = defaultForeground.Color;
        //}

        //private void Editor_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    editor.Document.GetText(TextGetOptions.UseCrlf, out string currentRawText);

        //    // reset colors to correct defaults for Focused state
        //    ITextRange documentRange = editor.Document.GetRange(0, TextConstants.MaxUnitCount);
        //    SolidColorBrush background = (SolidColorBrush)App.Current.Resources["TextControlBackgroundFocused"];

        //    if (background != null)
        //    {
        //        documentRange.CharacterFormat.BackgroundColor = background.Color;
        //    }
        //}
        //,0

        //private void Editor_TextChanged(object sender, RoutedEventArgs e)
        //{
        //    editor.Document.Selection.CharacterFormat.ForegroundColor = currentColor;
        //}

        //#endregion
    }
}
