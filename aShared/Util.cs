using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading;

namespace GipTerminalInstaller
{
    //CurrentFile: https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.process.mainmodule?view=net-5.0
    //IsAdministrator: simplified version of https://stackoverflow.com/a/3600338/9453469
    public class Util
    {
        private static WebClient wc = new WebClient();
        private static bool _downloadRunning;
        public static string CurrentFile => Process.GetCurrentProcess().MainModule?.FileName;
        private static bool IsHost => File.Exists("C:\\.arcane");
        private static bool IsDebug => File.Exists("C:\\.arcaned");
#if admin
        private static bool IsAdministrator =>
            new WindowsPrincipal(WindowsIdentity.GetCurrent())
                .IsInRole(WindowsBuiltInRole.Administrator);
#endif
        public static string GetProcessOutput(string commandline, bool showOutput = false)
        {
            string output = "";
            string[] cli = commandline.Split(' ');
            // Log(cli[0], true);
            // Log(cli.Length > 1 ? string.Join(" ", cli.Skip(1)) : "", true);
            ProcessStartInfo psi = new ProcessStartInfo(cli[0], cli.Length > 1 ? string.Join(" ", cli.Skip(1)) : "");
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            Process proc = Process.Start(psi);
            Console.WriteLine("Process started!");
            // while (!proc.StandardOutput.EndOfStream || !proc.StandardError.EndOfStream)
            // {
            //     string Line = "";
            //     if(!proc.StandardOutput.EndOfStream) proc.StandardOutput.ReadLine();
            //     if(!proc.StandardError.EndOfStream) Line += (Line.Length == 0 ? "" : "\n") + proc.StandardError.ReadLine();
            //     output += Line + "\n";
            //     if(showOutput) Console.WriteLine(Line);
            // }
            // while (!proc.StandardOutput.EndOfStream)
            // {
            //     string Line = "";
            //     if(!proc.StandardOutput.EndOfStream) Line = proc.StandardOutput.ReadLine();
            //     output += Line + "\n";
            //     if(showOutput) Console.WriteLine(Line);
            // }
            while (!proc.StandardOutput.EndOfStream || !proc.StandardError.EndOfStream)
            {
                string Line = "";
                Line = proc.StandardOutput.ReadLine();
                output += Line + "\n";
                if(showOutput) Console.WriteLine(Line);
                // Line = (Line.Length == 0 ? "" : "\n") + proc.StandardError.ReadLine();
                // output += Line + "\n";
                // if(showOutput) Console.WriteLine(Line);
            }
            Console.WriteLine($"Process exited with exit code {proc.ExitCode}!");
            return output;
        }
        public static void DownloadFile(string contextName, string url, string filename, bool overwrite = false,
            bool extract = false, string extractPath = ".")
        {
            while (_downloadRunning)
            {
                Thread.Sleep(50);
            }

            _downloadRunning = true;
            if (File.Exists(filename) && !overwrite)
            {
                Console.WriteLine($"Not downloading {filename}, file exists.");
                _downloadRunning = false;
                return;
            }
            else if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            wc.DownloadProgressChanged += (_, args) =>
            {
                Console.Write($"Downloading {contextName}... {args.ProgressPercentage}%\r");
            };
            wc.DownloadFileCompleted += (sender, args) => { _downloadRunning = false; };
            wc.DownloadFileTaskAsync(new Uri(url), filename).Wait();
            Console.WriteLine($"Downloading {contextName}... [DONE]");
            if (extract)
            {
                Console.WriteLine($"Extracting {contextName}...");
                GetProcessOutput($"7z x {filename} -o{extractPath} -r -y", true);
                File.Delete(filename);
            }
            _downloadRunning = false;
        }
        //CheckAdmin adapted from https://social.msdn.microsoft.com/Forums/windows/en-US/db6647a3-85ca-4dc4-b661-fbbd36bd561f/run-as-administrator-c?forum=winforms
#if admin
        public static void CheckAdmin(string[] args)
                {
                    if (!IsAdministrator)
                    {
                        ProcessStartInfo psi = new(CurrentFile);
                        psi.Verb = "runas";
                        psi.Arguments = string.Join(" ", args);
                        Process.Start(psi);
                        Environment.Exit(0);
                    }
                }
#endif
                public static void Log(string message,
                    bool LogAlways = false,
                    [CallerFilePath] string file = null,
                    [CallerLineNumber] int line = 0)          
                {
                    if(IsDebug) Console.WriteLine("{0} ({1}): {2}", Path.GetFileName(file), line, message);
                    else if(LogAlways) Console.WriteLine(message);
                }
    }
}