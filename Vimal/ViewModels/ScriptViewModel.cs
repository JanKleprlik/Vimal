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

namespace Vimal.ViewModels
{
    public class ScriptViewModel : ObservableObject
    {
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
        
        #endregion
        private RelayCommand _runScriptCommand;
        public RelayCommand RunScriptCommand => _runScriptCommand ?? (_runScriptCommand = new RelayCommand(RunScript));


        private async void RunScript()
        {
            Debug.WriteLine("Starting process...");

            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                // Save args to AppData
                //TODO: redo the paths to have them saved somewhere locally
                ApplicationData.Current.LocalSettings.Values["compilerPath"] = @"C:\Program Files\Kotlin\kotlinc\bin\kotlinc.bat";//ViewModel.CompilerPath;
                ApplicationData.Current.LocalSettings.Values["scriptPath"] = @"C:\Users\klepr\source\repos\JB\Kotlin\HelloWorld.kts";// ViewModel.ScriptPath;
                ApplicationData.Current.LocalSettings.Values["scriptData"] = Script;

                App.AppServiceConnected += AppServiceConnected;
                App.AppServiceDisconnected += AppServiceDisconnected;

                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync("KotlinParams");
            }

            //ValueSet request = new ValueSet();
            //request.Add("KEY", "VALUE");
            //AppServiceResponse response = await App.connection.SendMessageAsync(request);
            //Debug.WriteLine("Response: " + response.Message);

            Debug.WriteLine("Process is running ...");
            Debug.WriteLine("");
            Debug.WriteLine("Press any key to exit.");
        }

        /// <summary>
        /// Handle calculation request from desktop process
        /// (dummy scenario to show that connection is bi-directional)
        /// </summary>
        private async void AppServiceConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            string d1 = (string)args.Request.Message["LINE"];

            //ValueSet response = new ValueSet();
            //response.Add("RESULT", "OK");
            //await args.Request.SendResponseAsync(response);

            Debug.WriteLine(d1);
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                Output += d1;
                Output += Environment.NewLine;
            }
            ).AsTask();

            // log the request in the UI for demo purposes
            //await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    tbRequests.Text += string.Format("Request: {0} + {1} --> Response = {2}\r\n", d1, d2, result);
            //});
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
