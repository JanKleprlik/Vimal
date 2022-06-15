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
        
        public ScriptViewModel(TextBlock outputTextBlock)
        {
            this.outputTextBlock = outputTextBlock;

            App.AppServiceConnected += AppServiceConnected;
            App.AppServiceDisconnected += AppServiceDisconnected;
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
            outputTextBlock.Inlines.Clear();
            outputTextBlock.Inlines.Add(new Run { Text = " <<< Starting process ... >>>", Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.LightGreen) });
            outputTextBlock.Inlines.Add(new LineBreak());

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

        /// <summary>
        /// Handle calculation request from desktop process
        /// (dummy scenario to show that connection is bi-directional)
        /// </summary>
        private async void AppServiceConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            string data = (string)args.Request.Message["LINE"];

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    if (data.ToLower().Contains("error"))
                    {
                        outputTextBlock.Inlines.Add(new Run() { Text = data, Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Red) });
                    }
                    else if (data.ToLower().Contains("warning"))
                    {
                        outputTextBlock.Inlines.Add(new Run() { Text = data, Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.Yellow) });
                    }
                    else
                    {
                        outputTextBlock.Inlines.Add(new Run() { Text = data });
                    }

                    outputTextBlock.Inlines.Add(new LineBreak());
                }).AsTask();
        }

        /// <summary>
        /// When the desktop process is connected, get ready to send/receive requests
        /// </summary>
        private void AppServiceConnected(object sender, AppServiceTriggerDetails e)
        {
            App.Connection.RequestReceived += AppServiceConnection_RequestReceived;
            //await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    // enable UI to access  the connection
            //    btnRegKey.IsEnabled = true;
            //});
        }

        /// <summary>
        /// When the desktop process is disconnected, reconnect if needed
        /// </summary> 
        private void AppServiceDisconnected(object sender, EventArgs e)
        {
            //await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    // disable UI to access the connection
            //    btnRegKey.IsEnabled = false;

            //    // ask user if they want to reconnect
            //    Reconnect();
            //});
        }

    }
}
