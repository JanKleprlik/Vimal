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

string compilerPath = localSettings.Values["compilerPath"] as string ?? "";
string v2 = localSettings.Values["saveToFile"] as string ?? "";
string scriptData = localSettings.Values["scriptData"] as string ?? "";
string language = localSettings.Values["language"] as string ?? "kotlin";

string compilerFileName = "";
if (language == "kotlin")
    compilerFileName = "kotlinc.bat";
if (language == "swift")
    compilerFileName = "swift";    

int repetitions = localSettings.Values["repetitions"] as int? ?? 1;

//write the script into a file
await File.WriteAllTextAsync(scriptPath, scriptData);

int exitCode = 0;

for (int i = 0; i < repetitions; i++)
{
    Process proc = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = compilerFileName,
            Arguments = @"-script " + scriptPath,
            WorkingDirectory = compilerPath,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,

        }
    };
    
    proc.Start();
    
    while (!proc.StandardOutput.EndOfStream)
    {
        string line = proc.StandardOutput.ReadLine();
        ValueSet request = new ValueSet();
        request.Add("LINE", line);

        connection.SendMessageAsync(request);

    }
    proc.WaitForExit();

    if (proc.ExitCode != 0)
    {
        //Write error to output
        string line = proc.StandardError.ReadLine();
        ValueSet request = new ValueSet();
        request.Add("LINE", line);

        connection.SendMessageAsync(request);
        exitCode = proc.ExitCode;
        break;
    }
    exitCode = proc.ExitCode;

    // Sleep to make sure the output is flushed
    Thread.Sleep(50);
    ValueSet requestRepetition = new ValueSet();
    requestRepetition.Add("REP", (1+i).ToString());

    connection.SendMessageAsync(requestRepetition);
    Thread.Sleep(50);

}

//Add thread sleep so all messages go through before the process kills itself
Thread.Sleep(50);
ValueSet requestCode = new ValueSet();
requestCode.Add("CODE", exitCode.ToString());

connection.SendMessageAsync(requestCode);
Thread.Sleep(50);
