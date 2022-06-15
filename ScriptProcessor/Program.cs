// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

Console.WriteLine("Hello, World!");

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


#region parameters
var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

string v = localSettings.Values["compilerPath"] as string ?? "";
string v2 = localSettings.Values["scriptPath"] as string ?? "";
string v3 = localSettings.Values["scriptData"] as string ?? "";


Console.WriteLine(v);
Console.WriteLine(v2);

// writ all args to console
foreach (var arg in args)
{
    Console.WriteLine(arg);
}

#endregion


Process proc = new Process
{
    StartInfo = new ProcessStartInfo
    {
        FileName = @"kotlinc.bat",
        Arguments = @"-script C:\Users\klepr\source\repos\JB\Kotlin\HelloWorld.kts",
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
    
    await connection.SendMessageAsync(request);

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