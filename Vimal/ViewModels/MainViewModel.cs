using System;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using Vimal.Helpers;
using Vimal.Models;
using Vimal.Services;
using Vimal.Views;
using Windows.UI.Xaml.Media.Animation;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace Vimal.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private RelayCommand _addTabCommand;
        private RelayCommand _settingsCommand;
        private RelayCommand<WinUI.TabViewTabCloseRequestedEventArgs> _closeTabCommand;
        private RelayCommand<int> _closeTabCommandByIdx;
        private RelayCommand<int> _renameTabCommandByIdx;

        public RelayCommand AddTabCommand => _addTabCommand ?? (_addTabCommand = new RelayCommand(AddTab));
        public RelayCommand SettingsCommand => _settingsCommand ?? (_settingsCommand = new RelayCommand(OpenSettigns));
        public RelayCommand<WinUI.TabViewTabCloseRequestedEventArgs> CloseTabCommand => _closeTabCommand ?? (_closeTabCommand = new RelayCommand<WinUI.TabViewTabCloseRequestedEventArgs>(CloseTab));
        public RelayCommand<int> CloseTabCommandByIdx => _closeTabCommandByIdx ?? (_closeTabCommandByIdx = new RelayCommand<int>(CloseTabByIdx));
        public RelayCommand<int> RenameTabCommandByIdx => _renameTabCommandByIdx ?? (_renameTabCommandByIdx = new RelayCommand<int>(RenameTabByIdx));

        public ObservableCollection<TabViewItemData> Tabs { get; } = new ObservableCollection<TabViewItemData>()
        {
            new TabViewItemData()
            {
                Index = 1,
                Header = "Kotlin",
                Content = new ScriptPage()
            },
            new TabViewItemData()
            {
                Index = 2,
                Header = "Swift",
                Content = new ScriptPage()
            }
        };

        public MainViewModel()
        {
        }

        private void AddTab()
        {
            int newIndex = Tabs.Any() ? Tabs.Max(t => t.Index) + 1 : 1;
            Tabs.Add(new TabViewItemData()
            {
                Index = newIndex,
                Header = $"Item {newIndex}",
                Content = $"This is the content for Item {newIndex}"
            });
        }

        private void CloseTab(WinUI.TabViewTabCloseRequestedEventArgs args)
        {
            if (args.Item is TabViewItemData item)
            {
                Tabs.Remove(item);
            }
        }

        private void RenameTabByIdx(int idx)
        {
            if (Tabs.Any(t => t.Index == idx))
            {
                var item = Tabs.First(t => t.Index == idx);
                item.Header = "new header";
            }
        }

        private void CloseTabByIdx(int idx)
        {
            if (idx < 1 || idx > Tabs.Count)
            {
                return;
            }
            Tabs.RemoveAt(idx);
        }

        private void OpenSettigns()
        {
            //NavigationService.GoBack();
            //NavigationService.Navigate(typeof(SettingsPage), null);
        }
    }
}
