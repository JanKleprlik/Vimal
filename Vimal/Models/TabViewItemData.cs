using System;
using Vimal.Views;

namespace Vimal.Models
{
    public class TabViewItemData
    {
        public int Index { get; set; }

        //TODO: add icons 
        public Uri IconSource { get; set; } = new Uri("ms-appx:///Assets/kotlin_logo.png");


        public string Header { get; set; }

        public object Content { get; set; }
    }
}
