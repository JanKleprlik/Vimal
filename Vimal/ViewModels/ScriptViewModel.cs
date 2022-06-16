using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vimal.Models.Languages;
using Vimal.Services;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace Vimal.ViewModels
{
    public class ScriptViewModel : ObservableObject
    {
        private TextBlock outputTextBlock;
        private TextBlock scriptTextBlock;

        public ScriptViewModel(TextBlock outputTextBlock, TextBlock scriptTextBlock)
        {
            this.scriptTextBlock = scriptTextBlock;
            this.outputTextBlock = outputTextBlock;
            IsBusy = false;
        }
        
        #region properties
        
        private string _script;
        public string Script
        {
            get => _script;
            set => SetProperty(ref _script, value);
        }

        private string _returnCode;
        public string ReturnCode
        {
            get => _returnCode;
            set => SetProperty(ref _returnCode, value);
        }

        private string _output;
        public string Output
        {
            get => _output;
            set => SetProperty(ref _output, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

        private RelayCommand _runScriptCommand;
        public RelayCommand RunScriptCommand => _runScriptCommand ?? (_runScriptCommand = new RelayCommand(RunScript));


        private async void RunScript()
        {

            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                // Save args to AppData
                ApplicationData.Current.LocalSettings.Values["scriptData"] = Script;

                IsBusy = true;
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("KotlinParams");
            }
        }

        public void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                Script = (sender as TextBox).Text;
                scriptTextBlock.Inlines.Clear();
                SyntaxHighlightingService.Highlight(Script, scriptTextBlock, new Kotlin());
            }

            
        }




    }
}
