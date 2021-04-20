using System;
using System.IO;
using System.Linq;
using GipTerminalInstaller;

namespace GipTerminalInstallerRunner
{
    public class DotNet
    {
        public static bool CheckIfInstalled()
        {
            Console.WriteLine("Checking if the .net 5 SDK is installed...");
            return Directory.Exists(@"C:\Program Files\dotnet\sdk") && Directory.GetDirectories(@"C:\Program Files\dotnet\sdk").Any(x=>x.StartsWith("5."));
        }

        public static void Install()
        {
            if (CheckIfInstalled()) return;
            Console.WriteLine("Downloading .net 5 SDK...");
            Util.DownloadFile(".net 5 SDK", "https://download.visualstudio.microsoft.com/download/pr/a105fe06-20a0-4233-8ff1-b85523b40f1d/5f26654016c41ab2dc6d8bc850a9bf4c/dotnet-sdk-5.0.200-win-x64.exe",
                "dotnet-setup.exe", true);
            Util.GetProcessOutput("cmd /c dotnet-setup.exe /install /norestart /quiet", true);
        }
    }
}