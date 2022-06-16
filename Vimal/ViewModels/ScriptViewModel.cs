using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using Vimal.Models.Languages;
using Vimal.Services;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Vimal.ViewModels
{
    public class ScriptViewModel : ObservableObject
    {

        public ScriptViewModel(ILanguage lang, TextBlock scriptTextBlock)
        {
            this.language = lang;
            this.scriptTextBlock = scriptTextBlock;
            IsBusy = false;
            Repetitions = 1;
        }


        #region properties

        private ILanguage language { get; }
        private TextBlock scriptTextBlock { get; }

        public DateTime ScriptStartTime { get; set; }

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

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private int _repetitions;
        public int Repetitions
        {
            get => _repetitions;
            set => SetProperty(ref _repetitions, value);
        }

        private string _progressText;
        public string ProgressText
        {
            get => _progressText;
            set => SetProperty(ref _progressText, value);
        }

        #endregion

        private RelayCommand _runScriptCommand;
        public RelayCommand RunScriptCommand => _runScriptCommand ?? (_runScriptCommand = new RelayCommand(RunScript));


        /// <summary>
        /// Adds highlighting to the code and displays it
        /// </summary>
        public void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                Script = (sender as TextBox).Text;
                scriptTextBlock.Inlines.Clear();
                SyntaxHighlightingService.Highlight(Script, scriptTextBlock, language);
            }

            
        }

        private async void RunScript()
        {
            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                ApplicationData.Current.LocalSettings.Values["scriptData"] = Script;
                ApplicationData.Current.LocalSettings.Values["repetitions"] = Repetitions;
                ApplicationData.Current.LocalSettings.Values["language"] = language.Language.ToString().ToLower();

                IsBusy = true;
                ProgressText = "Calculating time ...";
                ScriptStartTime = DateTime.Now;
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("KotlinParams");

            }
        }
    }
}
