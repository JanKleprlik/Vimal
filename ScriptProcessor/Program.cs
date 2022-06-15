// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;


#region connection

var connection = new AppServiceConnection();
connection.AppServiceName = "SampleInteropService";
connection.PackageFamilyName = Package.Current.Id.FamilyName;

connection.RequestReceived += Connection_RequestReceived;

void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
{
    Console.WriteLine("Request received");
}

connection.ServiceClosed += Connection_ServiceClosed;

void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
{
    Console.WriteLine("Service closed");
}

AppServiceConnectionStatus status = await connection.OpenAsync();
if (status != AppServiceConnectionStatus.Success)
{
    // something went wrong ...
    Console.WriteLine("Connection failed");
    Console.WriteLine(status);
}

#endregion


var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

var scriptPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\script.kts";

string v = localSettings.Values["compilerPath"] as string ?? "";
string v2 = localSettings.Values["scriptPath"] as string ?? "";
string scriptData = localSettings.Values["scriptData"] as string ?? "";

//write the script into a file
await File.WriteAllTextAsync(scriptPath, scriptData);


Process proc = new Process
{
    StartInfo = new ProcessStartInfo
    {
        FileName = @"kotlinc.bat",
        Arguments = @"-script " + scriptPath,
        WorkingDirectory = @"C:\Program Files\Kotlin\kotlinc\bin",
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        CreateNoWindow = true,

    }
};
Console.WriteLine("Starting process...");
proc.Start();
Console.WriteLine("Process is running ...");
while (!proc.StandardOutput.EndOfStream)
{
    string line = proc.StandardOutput.ReadLine();
    ValueSet request = new ValueSet();
    request.Add("LINE", line);
    
    connection.SendMessageAsync(request);

    // Do something with line
    Console.WriteLine(line);
}
proc.WaitForExit();
Console.WriteLine("Process finished with exit code: " + proc.ExitCode);
if (proc.ExitCode != 0)
{
    //Write error to output
    Console.WriteLine(proc.StandardError.ReadToEnd());
}

connection.RequestReceived -= Connection_RequestReceived;