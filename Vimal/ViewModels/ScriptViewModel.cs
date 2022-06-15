using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vimal.ViewModels
{
    public class ScriptViewModel : ObservableObject
    {
        private string _script;
        public string Script
        {
            get => _script;
            set => SetProperty(ref _script, value);
        }
    }
}
