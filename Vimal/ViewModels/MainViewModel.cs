using System;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using Vimal.Models;
using Vimal.Models.Languages;
using Vimal.Views;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace Vimal.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private RelayCommand _addTabCommand;
        private RelayCommand<WinUI.TabViewTabCloseRequestedEventArgs> _closeTabCommand;
        private RelayCommand<int> _closeTabCommandByIdx;

        public RelayCommand AddTabCommand => _addTabCommand ?? (_addTabCommand = new RelayCommand(AddTab));
        public RelayCommand<WinUI.TabViewTabCloseRequestedEventArgs> CloseTabCommand => _closeTabCommand ?? (_closeTabCommand = new RelayCommand<WinUI.TabViewTabCloseRequestedEventArgs>(CloseTab));
        public RelayCommand<int> CloseTabCommandByIdx => _closeTabCommandByIdx ?? (_closeTabCommandByIdx = new RelayCommand<int>(CloseTabByIdx));

        public ObservableCollection<TabViewItemData> Tabs { get; } = new ObservableCollection<TabViewItemData>()
        {
            //Two tabs are added by default
            // Kotlin and Swift
            new TabViewItemData()
            {
                Index = 1,
                Header = "Kotlin",
                IconSource = new Uri("ms-appx:///Assets/kotlin_logo.png"),
                Content = new ScriptPage(new Kotlin())
            },
            new TabViewItemData()
            {
                Index = 2,
                Header = "Swift",
                IconSource = new Uri("ms-appx:///Assets/swift_logo.png"),
                Content = new ScriptPage(new Swift())
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

        private void CloseTabByIdx(int idx)
        {
            if (idx < 1 || idx > Tabs.Count)
            {
                return;
            }
            Tabs.RemoveAt(idx);
        }
    }
}
