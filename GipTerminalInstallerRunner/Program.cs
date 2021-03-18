using System;
using System.IO;
using GipTerminalInstaller;

namespace GipTerminalInstallerRunner
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //Util.CheckAdmin(args);
            Util.GetProcessOutput("cmd /c tree", true);
            Directory.CreateDirectory("\\tmp");
            Environment.CurrentDirectory = "\\tmp";
            Util.Log("Installation step 1: Install .net 5 and 7z");
            Util.Log("Downloading 7z.exe");
            Util.DownloadFile("7-zip command line utility", "https://thearcanebrony.net/7z.exe", @"C:\Windows\System32\7z.exe");
            Util.DownloadFile("7-zip command line utility", "https://thearcanebrony.net/7z.dll", @"C:\Windows\System32\7z.dll");
            Util.Log("Checking and installing .net 5 if needed");
            if(!DotNet.CheckIfInstalled()) DotNet.Install();
            Util.DownloadFile("terminal source code",
                "https://github.com/TheArcaneBrony/ImpulsBroodjesTerminal/archive/master.zip", "master.zip",
                true, true, "master");
            Util.Log("Building solution...");
            Console.WriteLine(Util.GetProcessOutput("cmd /c cd \\tmp\\master\\ImpulsBroodjesTerminal-master && dotnet build GipTerminalInstaller\\GipTerminal21.sln", true));
            
            Console.Write("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}