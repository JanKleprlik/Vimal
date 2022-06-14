using Vimal.Views;

namespace Vimal.Models
{
    public class TabViewItemData
    {
        public int Index { get; set; }

        //TODO: add icons 
        public string IconSource { get; set; } = "List";
        
        public string Header { get; set; }

        public object Content { get; set; }
    }
}
