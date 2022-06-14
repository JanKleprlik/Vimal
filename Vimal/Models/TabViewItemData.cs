using Vimal.Views;

namespace Vimal.Models
{
    public class TabViewItemData
    {
        public int Index { get; set; }

        public string IconSource { get; set; } = "File";
        
        public string Header { get; set; }

        public object Content { get; set; } = new SettingsPage();
    }
}
