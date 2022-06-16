using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;


// Establish connection
var connection = new AppServiceConnection();
connection.AppServiceName = "SampleInteropService";
connection.PackageFamilyName = Package.Current.Id.FamilyName;

AppServiceConnectionStatus status = await connection.OpenAsync();
if (status != AppServiceConnectionStatus.Success)
{
    // something went wrong ...
    Debug.WriteLine("Connection failed");
    Debug.WriteLine(status);
}


// Get script settings
var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
var scriptPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\script.kts";
string compilerPath = localSettings.Values["compilerPath"] as string ?? "";
string scriptData = localSettings.Values["scriptData"] as string ?? "";
string language = localSettings.Values["language"] as string ?? "kotlin";
int repetitions = localSettings.Values["repetitions"] as int? ?? 1;


//Pick language
string compilerFileName = "";
if (language == "kotlin")
    compilerFileName = "kotlinc.bat";
if (language == "swift")
    compilerFileName = "swift";    


//Write the script into a file
await File.WriteAllTextAsync(scriptPath, scriptData);


//Execute script
int exitCode = 0;
for (int i = 0; i < repetitions; i++)
{
    //Prepare process with script
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

    //Send output to UWP app
    while (!proc.StandardOutput.EndOfStream)
    {
        string line = proc.StandardOutput.ReadLine();
        ValueSet request = new ValueSet();
        request.Add("LINE", line);

        connection.SendMessageAsync(request);

    }
    
    proc.WaitForExit();
    
    //Send error to UWP app
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


    //Send number of repetitions to UWP app
    //Sleep to make sure the output is flushed
    Thread.Sleep(50);
    ValueSet requestRepetition = new ValueSet();
    requestRepetition.Add("REP", (1+i).ToString());

    connection.SendMessageAsync(requestRepetition);
    Thread.Sleep(50);
}

//Send exit code to UWP app
//Sleep so all messages go through before the process kills itself
Thread.Sleep(50);
ValueSet requestCode = new ValueSet();
requestCode.Add("CODE", exitCode.ToString());

connection.SendMessageAsync(requestCode);
Thread.Sleep(50);
