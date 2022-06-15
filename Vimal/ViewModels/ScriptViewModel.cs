using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }
        
        #region properties
        
        private string _script;
        public string Script
        {
            get => _script;
            set => SetProperty(ref _script, value);
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
            //outputTextBlock.Inlines.Clear();
            //outputTextBlock.Inlines.Add(new Run { Text = " <<< Starting process ... >>>", Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.LightGreen) });
            //outputTextBlock.Inlines.Add(new LineBreak());

            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                // Save args to AppData
                //TODO: redo the paths to have them saved somewhere locally
                ApplicationData.Current.LocalSettings.Values["compilerPath"] = @"C:\Program Files\Kotlin\kotlinc\bin\kotlinc.bat";//ViewModel.CompilerPath;
                ApplicationData.Current.LocalSettings.Values["scriptPath"] = @"C:\Users\klepr\source\repos\JB\Kotlin\HelloWorld.kts";// ViewModel.ScriptPath;
                ApplicationData.Current.LocalSettings.Values["scriptData"] = Script;

                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("KotlinParams");
            }
        }

        public void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Debug.WriteLine("Changed");
            if (sender is TextBox){
                Script = (sender as TextBox).Text;
            }
        }




    }
}
